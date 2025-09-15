using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SwitchWeapon : MonoBehaviour
{
    public List<WeaponData> weapons = new List<WeaponData>();

    private int currentIndexWeapon = 0;

    private GameObject currentWeapon;

    private PlayerInput playerInput;
    private InputAction switchWeapon;


    private void Awake()
    {

        playerInput = GetComponent<PlayerInput>();
        switchWeapon = playerInput.actions["SwitchWeapon"];
    }

    private void OnEnable()
    {
        switchWeapon.performed += SwitchWeapons;

    }

    private void OnDisable()
    {
        switchWeapon.performed -= SwitchWeapons;

    }


    private void Start()
    {
        if (weapons.Count > 0)
        {
            EquipedWeapom(0);
        }

    }


    private void SwitchWeapons(InputAction.CallbackContext ctx)
    {
        int numb = 0;
        string bindName = ctx.control.name;
        if (bindName == "1") numb = 1;
        else if (bindName == "2") numb = 2;
        else if (bindName == "3") numb = 3;
        else return;

        int ind = numb - 1;
        EquipedWeapom(ind);

    }


    private void EquipedWeapom(int ind)
    {
        currentIndexWeapon = ind;

        if (currentWeapon != null)
        {
            Destroy(currentWeapon);
        }

        WeaponData weaponData = weapons[currentIndexWeapon];
        if (weaponData.prefab_Weapon != null)
        {

            currentWeapon = Instantiate(weaponData.prefab_Weapon, transform);
        }
        else
        {
            Debug.Log("prefabul nui setat");
        }


    }

}
