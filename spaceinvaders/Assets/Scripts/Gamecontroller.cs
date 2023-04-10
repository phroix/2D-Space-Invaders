using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
class Gamecontroller : MonoBehaviour
{
    // Controller
    private GameObject enemyGroup;
    private EnemyController controller;
    private GameObject bossGroup;
    private EnemyController bossGroupController;
    private GameObject boss;
    private Boss bossController;

    //Shop Canvas
    public GameObject shopCanvas;
    public GameObject menuCanvas;
    public GameObject prefabEnemyGroup;
    public GameObject prefabBossGroup;
    Barricades[] barricades;

    //EndlessGame
    private static bool endless = false;
    private int anzEnemyGroupsLoaded = 0;
    private int bossSpawnIntervall = 5;

    // Scores
    private int highscore = 0;
    private int points = 0;
    private int enemies = 0;
    private int bossCount = 0;
    private int bossLife = -1;
    private int coins = 0;

    // Text UI Elements 
    public Text highscoreText;
    public Text pointsText;
    public Text coinText;

    private void Start()
    {
        menuCanvas.SetActive(false);
        Time.timeScale = 0;
        //get the barricades to increase their health
        barricades = FindObjectsOfType<Barricades>();
        //shopCanvas = GameObject.Find("ShopCanvas");
        //Debug.Log(GameObject.Find("ShopCanvas"));

        //Load enemy group on Start
        if (endless)
        {
            loadNewEnemyGroup();
        }
        else
        {
            // get the EnemyController to get the living enemyies
            enemyGroup = GameObject.Find("Enemy_Group01");
            controller = enemyGroup.GetComponent<EnemyController>();
            enemies = controller.anzahlEne;
        }

        // Retrieve the Highscore from the PlayerPrefs
        highscore = PlayerPrefs.GetInt("highscore", 0);
        highscoreText.text = "Highscore: " + highscore;

        // Set Points at start of game to 0
        pointsText.text = "Points: " + points.ToString();

        // Retrieve the Coins gained so far from the PlayerPrefs
        coins = PlayerPrefs.GetInt("coins");
        coinText.text = "Coins: " + coins.ToString();
    }

    public static void activateEndlessGame()
    {
        endless = true;
    }

    private void FixedUpdate()
    {

        PauseGame();
        if (endless)
        {

            // Only get the Elements once
            if (bossGroup == null && (anzEnemyGroupsLoaded % bossSpawnIntervall) == 0)
            {
                bossGroup = GameObject.Find("Boss_Group");
                bossGroupController = bossGroup.GetComponent<EnemyController>();
                bossCount = bossGroupController.anzahlEne;

                boss = bossGroup.transform.GetChild(0).gameObject;
                bossController = boss.GetComponent<Boss>();
            }



            // Get Active Boss Life
            if (bossController != null)
            {
                bossLife = bossController.Leben;

                if (bossGroupController.anzahlEne < bossCount)
                {
                    // If Boss or Boss Minion gets killed add 15 Points to highscore
                    bossCount = bossGroupController.anzahlEne;
                    points += 15;
                    pointsText.text = "Points: " + points.ToString();

                    // If Enemy from Boss gets Killed a new Coin gets saved in Player Prefs
                    coins += 1;
                    PlayerPrefs.SetInt("coins", coins);
                    coinText.text = "Coins: " + coins.ToString();

                    // Check if Boss gets Killed
                    if (bossLife == 0)
                    {
                        // Error: Dosent set the Life or add the Highscore
                        bossLife = -1;
                        points += 35;

                        // If Boss gets Killed get more Coins
                        coins += 9;
                        PlayerPrefs.SetInt("coins", coins);
                        coinText.text = "Coins: " + coins.ToString();
                    }

                    // If the new points is bigger than the Highscore it gets saved in PlayerPrefs and Highscore shows the new Points
                    if (highscore < points)
                    {
                        PlayerPrefs.SetInt("highscore", points);
                        highscoreText.text = "Highscore: " + points.ToString();
                    }

                }
            }
        }

        // If a Enemy dies the score gets increased and displayed
        if (controller.anzahlEne < enemies)
        {
            enemies = controller.anzahlEne;
            Debug.Log(enemies);
            points += 10;
            pointsText.text = "Points: " + points.ToString();

            // If Enemy gets Killed a new Coin gets saved in Player Prefs
            coins += 1;
            PlayerPrefs.SetInt("coins", coins);
            coinText.text = "Coins: " + coins.ToString();

            // If the new points is bigger than the Highscore it gets saved in PlayerPrefs and Highscore shows the new Points
            if (highscore < points)
            {
                PlayerPrefs.SetInt("highscore", points);
                highscoreText.text = "Highscore: " + points.ToString();
            }

            Debug.Log("in Update vor Abfrage: " + enemies);
            if (enemies == 0)
            {
                Debug.Log("Enemies are 0 now!");
                //Load New Set of Enemys, when endlessgame activatet
                if (endless)
                {
                    loadNewEnemyGroup();
                }
                else
                {
                    SceneManager.LoadScene("WinnerScene");
                }
            }
        }
    }

    /**
     * Loads New Enemy Group
     * -> if doable, loads Boss after certain amount of "rounds"
     */
    public void loadNewEnemyGroup()
    {
        anzEnemyGroupsLoaded++;

        float yCoordinate = 1.9f;

        if (enemyGroup != null)
        {
            Destroy(enemyGroup);
        }
        if (bossGroup != null)
        {
            Destroy(bossGroup);
        }

        //Loads a new prefab into the gameScene
        if ((anzEnemyGroupsLoaded % bossSpawnIntervall) == 0)
        {
            bossGroup = Instantiate(prefabBossGroup, new Vector3(0, yCoordinate, 0), Quaternion.identity) as GameObject;
        }
        else
        {
            enemyGroup = Instantiate(prefabEnemyGroup, new Vector3(0, yCoordinate, 0), Quaternion.identity) as GameObject;
            controller = enemyGroup.GetComponent<EnemyController>();
            enemies = controller.anzahlEne;
        }
    }

    public void PauseGame()
    {
        if (Input.GetButton("Cancel"))
        {
            Time.timeScale = 0;
            menuCanvas.SetActive(true);
        }
    }

    public void ResumeGame()
    {
        Time.timeScale = 1;
        menuCanvas.SetActive(false);
    }

    public void BuyBarricadesHealth()
    {
        if (coins >= 25)
        {
            for (int i = 0; i < barricades.Length; i++)
            {
                barricades[i].IncreaseHealth();
            }
            coins -= 25;
            coinText.text = "Coins: " + coins.ToString();
        }
    }

    public void StartGame()
    {
        Time.timeScale = 1;
        if (shopCanvas != null)
        {
            shopCanvas.SetActive(false);

        }
    }
}