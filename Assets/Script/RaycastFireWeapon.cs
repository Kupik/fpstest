using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class RaycastFireWeapon : MonoBehaviour
{
    public WeaponData weaponData;

    public Transform firePoint;
    public float fireRate = 0.5f;
    public float range = 100f;

    public int currentAmmo;
    public int reserveAmmo;

    public ParticleSystem muzzleFlash;
    public GameObject impactEffect;

    private PlayerInput playerInput;
    private InputAction fireAction;
    private InputAction reloadAction;
    private float nextFireTime = 0f;
    private Camera playerCamera;
    private bool isShooting = false;

    public Animator animator;

    void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;   
        Cursor.visible = false; 
        
        playerInput = GetComponent<PlayerInput>();
        fireAction = playerInput.actions["Fire"];
        reloadAction = playerInput.actions["Reload"];
    }

    void OnEnable()
    {
        fireAction.performed += OnFirePerformed;
        fireAction.canceled += OnFireCanceled;
        reloadAction.performed += OnReloadPerformed;
    }

    void OnDisable()
    {
        fireAction.performed -= OnFirePerformed;
        fireAction.canceled -= OnFireCanceled;
        reloadAction.performed -= OnReloadPerformed;
    }

    void Start()
    {
        playerCamera = Camera.main;
        InitializeAmmo();
    }

    void InitializeAmmo()
    {
        currentAmmo = weaponData.bullet;
        reserveAmmo = weaponData.ammo_pack;
    }

    void Update()
    {

        if (isShooting && Time.time >= nextFireTime && currentAmmo > 0)
        {
            StartCoroutine(Shoot());
            nextFireTime = Time.time + fireRate;
        }
    }

    private void OnFirePerformed(InputAction.CallbackContext context)
    {
        isShooting = true;
    }

    private void OnFireCanceled(InputAction.CallbackContext context)
    {
        isShooting = false;
    }

    private void OnReloadPerformed(InputAction.CallbackContext context)
    {
        if (reserveAmmo > 0 && currentAmmo < weaponData.bullet)
        {
            StartCoroutine(Reload());
        }
    }

    public IEnumerator Shoot()
    {


         currentAmmo--;
        animator.SetBool("Fire",true);
        if (muzzleFlash != null)
            muzzleFlash.gameObject.SetActive(true);
        muzzleFlash.Play();
        yield return new WaitForSeconds(0.1f);

        RaycastHit hit;
        Vector3 rayOrigin = playerCamera.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0));

        if (Physics.Raycast(rayOrigin, playerCamera.transform.forward, out hit, range))
        {
            Enemy enemy = hit.transform.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(weaponData.damage);
            }
            if (impactEffect != null)
            {
                GameObject impactGO = Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal));
                Destroy(impactGO, 2f);
            }
        }
        animator.SetBool("Fire",false);

    }

    public IEnumerator Reload()
    {

        int ammoNeeded = weaponData.bullet - currentAmmo;
        int ammoToReload = Mathf.Min(ammoNeeded, reserveAmmo);

        currentAmmo += ammoToReload;
        reserveAmmo -= ammoToReload;
        animator.SetBool("Reload", true);

        yield return new WaitForSeconds(2f);


        animator.SetBool("Reload", false);

    }
}