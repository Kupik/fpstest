using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
       
        //WinAndLose manager = FindObjectOfType<WinAndLose>();
        //if (manager != null)
        //{
        //    manager.EnemyDied();
        //}

        gameObject.SetActive(false);

    }

    public int GetCurrentHealth()
    {
        return currentHealth;
    }
}
