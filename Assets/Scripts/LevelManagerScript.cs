using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelManagerScript : MonoBehaviour
{

    public GameObject spawnPointRight;
    public GameObject spawnPointLeft;
    public GameObject finishPoint;
    public GameObject[] enemies;
    public Text HealthText;
    public Text MoneyText;
    public int levelType;//1 - с двух сторон, 2 - только справа
    public int health = 5;//Жизни
    public int GameMoney;//Деньги
    public int totalEnemies;//Врагов в уровне
    public int enemiesPerSpawn;//Врагов в новой волне
    public int enemiesOnScreen = 0;//Врагов на экране
    public int spawnedEnemies = 0;//Заспавнилось в уровне
    public float spawnDelay = 2;//Задержка между волнами

    public int fieldWidth, fieldHeight;
    public float paddingX, paddingY;

    public float right = 16, up = 4;

    public bool GameIsPaused = false;
    public bool DestroyIsOpen = false;
    public TowerScript destroyingTower;

    public GameObject PauseMenuUI;

    public GameObject cellPref;

    public GameObject nextLevelPortal;

    public Transform cellParent;

    void Start()
    {
        nextLevelPortal.SetActive(false);
        CreateLevel();
        StartCoroutine(Spawn());
    }

    void Update()
    {
        if (spawnedEnemies < totalEnemies || enemiesOnScreen > 0)
        {
            //Вызов меню
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (GameIsPaused)
                    Resume();
                else
                    Pause();
            }

            //Конец игры
            if (health <= 0)
            {
                ToLobbyButton();
            }

            //Перерасчет жизней и денег
            HealthText.text = "Health: " + health.ToString();
            MoneyText.text = GameMoney.ToString();
        }
        else if (health > 0)
            StartCoroutine(Win());
    }


    //----------------------------------------
    //----------------------------------------
    //~~~~~~~~~~~~~~~~SPAWNER~~~~~~~~~~~~~~~~~

    IEnumerator Spawn()
    {
        if (enemiesPerSpawn > 0 && spawnedEnemies < totalEnemies)
        {
            StartCoroutine(SpawnExtra(0));

            yield return new WaitForSeconds(spawnDelay + enemiesPerSpawn);
            StartCoroutine(Spawn());
        }
    }

    IEnumerator SpawnExtra(int it)
    {
        yield return new WaitForSeconds(1);

        if (it < enemiesPerSpawn)
        {
            float sprSizeY = cellPref.GetComponent<SpriteRenderer>().bounds.size.y;

            if (levelType == 1)//с двух сторон
            {
                int enemyType = Random.Range(1, 3);

                GameObject newEnemy = Instantiate(enemies[Random.Range(0, enemies.Length)]);

                int rnd = Random.Range(0, fieldHeight);

                //При помощи z-index делаю правильное наложение врагов друг на друга
                if (enemyType == 1)//правый
                    newEnemy.transform.position = new Vector3(spawnPointRight.transform.position.x, up - paddingY - sprSizeY/2 - 2 * paddingY * rnd - sprSizeY * rnd, rnd);
                if (enemyType == 2)//левый
                {
                    newEnemy.transform.position = new Vector3(spawnPointLeft.transform.position.x, up - paddingY - sprSizeY/2 - 2 * paddingY * rnd - sprSizeY * rnd, rnd);
                    newEnemy.GetComponent<SpriteRenderer>().flipX = true;
                }

                newEnemy.GetComponentInChildren<EnemyScript>().enemyOrientationType = enemyType;

                enemiesOnScreen += 1;
                spawnedEnemies += 1;
            }
            else if (levelType == 2)
            {
                GameObject newEnemy = Instantiate(enemies[Random.Range(0, enemies.Length)]);

                int rnd = Random.Range(0, fieldHeight);

                //При помощи z-index делаю правильное наложение врагов друг на друга
                newEnemy.transform.position = new Vector3(spawnPointRight.transform.position.x, up - paddingY - sprSizeY/2 - 2 * paddingY * rnd - sprSizeY * rnd, rnd);
                newEnemy.GetComponentInChildren<EnemyScript>().enemyOrientationType = 1;
                enemiesOnScreen += 1;
                spawnedEnemies += 1;
            }

            StartCoroutine(SpawnExtra(it + 1));
        }
    }

    //----------------------------------------
    //----------------------------------------
    //Создание сетки, на которую ставим башни:

    void CreateLevel()
    {
        //Правая половина
        for (int i = 0; i < fieldHeight; i++)
            for (int j = 0; j < fieldWidth; j++)
                CreateCell(j, i, "right");

        //Левая половина
        if (levelType == 1)
            for (int i = 0; i < fieldHeight; i++)
                for (int j = 0; j < fieldWidth; j++)
                    CreateCell(j, i, "left");
    }

    void CreateCell(int x, int y, string type)
    {
        GameObject tmpCell = Instantiate(cellPref);

        tmpCell.transform.parent = cellParent;

        float sprSizeX = tmpCell.GetComponent<SpriteRenderer>().bounds.size.x;
        float sprSizeY = tmpCell.GetComponent<SpriteRenderer>().bounds.size.y;

        if (type == "right")
        {
            if (x == 0 && y == 0)
                tmpCell.transform.position = new Vector3(right + paddingX, up - paddingY, 0);
            else if (y == 0)
                tmpCell.transform.position = new Vector3(right + paddingX - 2 * paddingX * x - sprSizeX * x, up - paddingY, 0);
            else if (x == 0)
                tmpCell.transform.position = new Vector3(right + paddingX, up - paddingY - 2 * paddingY * y - sprSizeY * y, 0);
            else
                tmpCell.transform.position = new Vector3(right + paddingX - 2 * paddingX * x - sprSizeX * x, up - paddingY - 2 * paddingY * y - sprSizeY * y, 0);
        }
        else if (type == "left")
        {
            if (x == 0 && y == 0)
                tmpCell.transform.position = new Vector3(-right - sprSizeX - paddingX, up - paddingY, 0);
            else if (y == 0)
                tmpCell.transform.position = new Vector3(-right - sprSizeX - paddingX + 2 * paddingX * x + sprSizeX * x, up - paddingY, 0);
            else if (x == 0)
                tmpCell.transform.position = new Vector3(-right - sprSizeX - paddingX, up - paddingY - 2 * paddingY * y - sprSizeY * y, 0);
            else
                tmpCell.transform.position = new Vector3(-right - sprSizeX - paddingX + 2 * paddingX * x + sprSizeX * x, up - paddingY - 2 * paddingY * y - sprSizeY * y, 0);
        }
    }


    //----------------------------------------
    //----------------------------------------
    //Функции для интерфейса:

    public void ToLobbyButton()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(1);
    }

    public void Resume()
    {
        PauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
    }

    public void Pause()
    {
        PauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
    }

    private IEnumerator Win()
    {
        if (PlayerPrefs.GetInt("LevelComplete") < SceneManager.GetActiveScene().buildIndex - 1)
            PlayerPrefs.SetInt("LevelComplete", SceneManager.GetActiveScene().buildIndex - 1);
        health = 0;
        yield return new WaitForSeconds(2);

        nextLevelPortal.SetActive(true);
    }
}
