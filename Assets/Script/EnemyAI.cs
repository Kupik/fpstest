using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public ParticleSystem muzzle;
    public Transform point;
    public float radiusDetect = 10f;
    public float radiusDeAtack = 4f;
    public float cooldown = 0.5f;
    public LayerMask playerLayerMask;
    public int dmg = 25;

    private NavMeshAgent enemy;
    private Transform player;
    private float lastAttackTime;


    private void Start()
    {
        enemy = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }


    public void Update()
    {

        float dist = Vector3.Distance(transform.position, player.position);

        if (dist <= radiusDetect)
        {
            transform.LookAt(player);

            if (dist <= radiusDeAtack)
            {
                PlayerHP playerHp = player.GetComponent<PlayerHP>();
                if (playerHp != null)
                {
                    Attack(playerHp);
                }
            }
            else
            {
                enemy.SetDestination(player.position);
            }
        }
    }



    void Attack(PlayerHP playerHp)
    {
        if (Time.time - lastAttackTime > cooldown)
        {
            Vector3 shootDirection = (player.position - point.position).normalized;
            if (Physics.Raycast(point.position, shootDirection, out RaycastHit hit, radiusDeAtack, playerLayerMask))
            {
                lastAttackTime = Time.time;
                Debug.Log("Attack");

                if (hit.collider.CompareTag("Player"))
                {
                    PlayerHP targetHP = hit.collider.GetComponent<PlayerHP>();
                    if (targetHP != null)
                    {
                        muzzle.gameObject.SetActive(true);
                        muzzle.Play();
                        targetHP.TakeDamage(dmg);

                    }
                }
            }
        }
    }


}

