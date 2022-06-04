using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelManagerScript : MonoBehaviour
{

    public GameObject spawnPoint;
    public GameObject finishPoint;
    public GameObject[] enemies;
    public Text HealthText;
    public Text MoneyText;
    public int health = 5;//Жизни
    public int GameMoney;//Деньги
    public int totalEnemies;//Врагов в уровне
    public int enemiesPerSpawn;//Врагов в новой волне
    public int enemiesOnScreen = 0;//Врагов на экране
    public int spawnedEnemies = 0;//Заспавнилось в уровне
    public float spawnDelay = 2;//Задержка между вонами

    public int fieldWidth, fieldHeight;
    public float paddingX, paddingY;

    int right = 8, up = 4;

    public bool GameIsPaused = false;
    public bool DestroyIsOpen = false;

    public GameObject PauseMenuUI;

    public GameObject cellPref;

    public Transform cellParent;

    void Start()
    {
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

            GameObject newEnemy = Instantiate(enemies[0]);

            int rnd = Random.Range(-4, 4);

            //При помощи z-index делаю правильное наложение врагов друг на друга
            newEnemy.transform.position = new Vector3(spawnPoint.transform.position.x, rnd - 0.4f + 1, rnd + 4);
            enemiesOnScreen += 1;
            spawnedEnemies += 1;

            StartCoroutine(SpawnExtra(it + 1));
        }
    }

    //----------------------------------------
    //----------------------------------------
    //Создание сетки, на которую ставим башни:

    void CreateLevel()
    {
        for (int i = 0; i < fieldHeight; i++)
            for (int j = 0; j < fieldWidth; j++)
                CreateCell(j, i);
    }

    void CreateCell(int x, int y)
    {
        GameObject tmpCell = Instantiate(cellPref);

        tmpCell.transform.parent = cellParent;

        float sprSizeX = tmpCell.GetComponent<SpriteRenderer>().bounds.size.x;
        float sprSizeY = tmpCell.GetComponent<SpriteRenderer>().bounds.size.y;

        if (x == 0 && y == 0)
            tmpCell.transform.position = new Vector3(right + paddingX, up - paddingY, 0);
        else if (y == 0)
            tmpCell.transform.position = new Vector3(right + paddingX - 2 * paddingX * x - sprSizeX * x, up - paddingY, 0);
        else if (x == 0)
            tmpCell.transform.position = new Vector3(right + paddingX, up - paddingY - 2 * paddingY * y - sprSizeY * y, 0);
        else
            tmpCell.transform.position = new Vector3(right + paddingX - 2 * paddingX * x - sprSizeX * x, up - paddingY - 2 * paddingY * y - sprSizeY * y, 0);
    }


    //----------------------------------------
    //----------------------------------------
    //Функции для интерфейса:

    public void ToLobbyButton()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
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
        yield return new WaitForSeconds(3);
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }
}
