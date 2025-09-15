using UnityEngine;

public class PlayerHP : MonoBehaviour
{
    public int HP = 100;
    public int currentHealth;

    void Start()
    {
        currentHealth = HP;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            //Die();
        }
    }

    //void Die()
    //{
       
    //}

    public int GetCurrentHealth()
    {
        return currentHealth;
    }
}
