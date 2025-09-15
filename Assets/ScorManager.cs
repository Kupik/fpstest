using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.UI;


public class ScorManager : MonoBehaviour
{

    public GameObject PlayerHealth;
    public GameObject[] EnemyHealth;

    public GameObject Win;
    public GameObject Lose;
    public GameObject LoseMatch;
    public GameObject WinMatch;
    private int atatckP;
    private int atatckE;


    
    public GameObject ScoreT;
    public GameObject ScoreCT;
    public int scoreT = 0;
    public int scoreCT = 0;


    private bool roundEnd = false;


    private bool Paus = false;


    public void Start()
    {

        Win.SetActive(false);
        Lose.SetActive(false);
        LoseMatch.SetActive(false);
        WinMatch.SetActive(false);

        scoreT = PlayerPrefs.GetInt("T", 0);
        scoreCT = PlayerPrefs.GetInt("CT", 0);

        UpdateScore();

    }


    public void DeclansezAtac(int damageJucator, int damageInamic)
    {
        WinBattle(damageJucator, damageInamic);
    }

    public void WinBattle(int playerAttack, int enemyAttack)
    {

        atatckE = enemyAttack;
        atatckP = playerAttack;
        if (roundEnd) return;
        bool allEnemiesDead = true;

        foreach (GameObject enemy in EnemyHealth)
        {
            Enemy enemyHealth = enemy.GetComponent<Enemy>();
            if (enemyHealth != null && enemy.activeSelf)
            {
                enemyHealth.TakeDamage(playerAttack);

                if (enemyHealth.GetCurrentHealth() > 0)
                {
                    allEnemiesDead = false;
                }
            }
        }

        foreach (GameObject enemy in EnemyHealth)
        {

            Enemy enemyHealth = enemy.GetComponent<Enemy>();
            if (enemyHealth != null)
            {
               
                enemyHealth.TakeDamage(atatckP);


            }

            if (allEnemiesDead)
            {
                WinMatchB();
                return;
            }

           
            PlayerHP playerHealth = PlayerHealth.GetComponent<PlayerHP>();


          
            enemyHealth.TakeDamage(playerAttack);

      
            if (enemyHealth.GetCurrentHealth() <= 0)
            {
                WinMatchB();
                return;
            }

            playerHealth.TakeDamage(enemyAttack);

        
            if (playerHealth.currentHealth <= 0)
            {
                LoseMatchB();

            }



            if (scoreT >= 2)
            {

                LoseMatchBattle();

            }

            if (scoreCT >= 2)
            {
                WinMatchBattle();

            }
        }

    }
    public void LoseMatchB()
    {
        
        if (roundEnd) return;

        scoreT++;
        roundEnd = true;
        UpdateScore();
        SaveScore();
        Lose.SetActive(true);
        StartCoroutine(SlowRefresRestartScene(0.7f));
    }

    public void WinMatchB()
    {
        
        if (roundEnd) return;

        scoreCT++;
        roundEnd = true;
        UpdateScore();
        SaveScore();
        Win.SetActive(true);
        StartCoroutine(SlowRefresRestartScene(0.7f));
    }


    public void WinMatchBattle()
    {

        Win.SetActive(false);
        WinMatch.SetActive(true);
        StartCoroutine(StartNewMatch());

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }



    public void LoseMatchBattle()
    {

        Lose.SetActive(false);
        LoseMatch.SetActive(true);
        Paus = true;
        StartCoroutine(StartNewMatch());

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

    }



    public IEnumerator SlowRefresRestartScene(float slow)
    {
        yield return new WaitForSeconds(slow);
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name); // restartam scena
    }

    public void UpdateScore()
    {
        ScoreT.GetComponent<Text>().text = "T" + scoreT;
        ScoreCT.GetComponent<Text>().text = "CT" + scoreCT;
    }

    public void SaveScore()
    {
        PlayerPrefs.SetInt("T", scoreT);
        PlayerPrefs.SetInt("CT", scoreCT);
        PlayerPrefs.Save(); 
    }

    public IEnumerator StartNewMatch()
    {
        yield return new WaitForSeconds(1f);
        scoreT = 0;
        scoreCT = 0;
        UpdateScore();
        SaveScore();


        roundEnd = false;

        Win.SetActive(false);
        Lose.SetActive(false);
        LoseMatch.SetActive(false);
        WinMatch.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;


    }

}
