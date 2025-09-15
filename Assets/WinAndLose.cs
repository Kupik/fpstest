using System.Collections;
using System.Diagnostics.Tracing;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class WinAndLose : MonoBehaviour
{
    public GameObject Win;
    public GameObject Lose;
    public GameObject LoseMatch;
    public GameObject WinMatch;

    public GameObject PlayerHealth;
    public GameObject[] EnemyHealth;


    // Attack hpDMG
    private int atatckP;
    private int atatckE;


    // Score T AND CT
    public GameObject ScoreT;
    public GameObject ScoreCT;
    public int scoreT = 0;
    public int scoreCT = 0;

    // round
    private bool roundEnd = false;

    // nu activ
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


    public void Update()
    {
        WinBattle(atatckP, atatckE);

    }

    public void WinBattle(int playerAttack, int enemyAttack)
    {
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
                // Aplică daune
                enemyHealth.TakeDamage(atatckP);


            }

            if (allEnemiesDead)
            {
                WinMatchB();
                return;
            }

            PlayerHP playerHealth = PlayerHealth.GetComponent<PlayerHP>();

            atatckE = enemyAttack;
            atatckP = playerAttack;

            // atacă inamicul
            enemyHealth.TakeDamage(playerAttack);

            //  inamicul a fost ucis
            if (enemyHealth.GetCurrentHealth() <= 0)
            {
                WinMatchB();
                return;
            }

            playerHealth.TakeDamage(enemyAttack);

            //  playerul a fost ucis
            if (playerHealth.currentHealth <= 0)
            {
                LoseMatchB();

            }



            if (scoreT >= 6)
            {

                LoseMatchBattle();

            }

            if (scoreCT >= 6)
            {
                WinMatchBattle();

            }
        }

    }
    public void LoseMatchB()
    {
        // Verific dacă meciul e deja încheiat
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
        // Verifică dacă meciul este deja încheiat
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
        PlayerPrefs.Save(); // salvez scoru
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