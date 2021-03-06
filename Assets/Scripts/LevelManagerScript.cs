using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
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
    public int levelType;//1 - ? ???? ??????, 2 - ?????? ??????
    public int health = 5;//?????
    public int GameMoney;//??????
    public int totalEnemies;//?????? ? ??????
    public int enemiesPerSpawn;//?????? ? ????? ?????
    public int enemiesOnScreen = 0;//?????? ?? ??????
    public int spawnedEnemies = 0;//???????????? ? ??????
    public float spawnDelay = 2;//???????? ????? ???????

    public int fieldWidth, fieldHeight;
    public float paddingX, paddingY;

    public float right = 16, up = 4;

    public bool GameIsPaused = false;
    public bool DestroyIsOpen = false;
    public TowerScript destroyingTower;

    public GameObject PauseMenuUI;
    public GameObject Continue;
    public GameObject Settings;
    public GameObject Home;
    public GameObject Back;
    public GameObject Slider;

    public Slider _slider;

    public GameObject cellPref;

    public GameObject nextLevelPortal;

    public Transform cellParent;

    public GameObject LoseScreenPref;

    public bool LevelIsCompleted = false;
    private bool IsWaitingForPress = false;

    public float possibleEnemy = 1;

    void Start()
    {
        AudioSource[] auds = FindObjectsOfType<AudioSource>();
        for (int i = 0; i < auds.Length; i++)
            if (!auds[i].CompareTag("LevelMusic"))
                Destroy(auds[i].gameObject);

        nextLevelPortal.SetActive(false);
        CreateLevel();
        StartCoroutine(Spawn());

        _slider.onValueChanged.AddListener((v) =>
        {
            PauseMenuUI.GetComponent<AudioSource>().volume = v;
            PlayerPrefs.SetFloat("MusicVolume", v);
        });
    }

    void Update()
    {
        if (!LevelIsCompleted)
        {
            //????? ????
            if (health <= 0)
            {
                LevelIsCompleted = true;
                HealthText.text = "Health: 0";
                StopAllCoroutines();
                StartCoroutine(Replay());
                Lose();
            }

            if (spawnedEnemies < totalEnemies || enemiesOnScreen > 0)
            {
                //?????????? ??????
                HealthText.text = "Health: " + health.ToString();
            }
            else if (health > 0)
                StartCoroutine(Win());

            //????? ????
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (GameIsPaused)
                    Resume();
                else
                    Pause();
            }
        }
        else if (IsWaitingForPress && Input.anyKeyDown)
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

        //?????????? ?????
        MoneyText.text = GameMoney.ToString();
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
            enemiesPerSpawn = (int)Mathf.Ceil(enemiesPerSpawn * 1.2f);
            possibleEnemy += 0.5f;
            StartCoroutine(Spawn());
        }
    }

    IEnumerator SpawnExtra(int it)
    {
        yield return new WaitForSeconds(1);

        if (it < enemiesPerSpawn)
        {
            float sprSizeY = cellPref.GetComponent<SpriteRenderer>().bounds.size.y;

            if (levelType == 1)//? ???? ??????
            {
                int enemyType = Random.Range(1, 3);

                GameObject newEnemy = Instantiate(enemies[Random.Range(0, Mathf.Min((int) possibleEnemy, enemies.Length))]);

                int rnd = Random.Range(0, fieldHeight);

                //??? ?????? z-index ????? ?????????? ????????? ?????? ???? ?? ?????
                if (enemyType == 1)//??????
                    newEnemy.transform.position = new Vector3(spawnPointRight.transform.position.x, up - paddingY - sprSizeY / 2 - 2 * paddingY * rnd - sprSizeY * rnd, rnd);
                if (enemyType == 2)//?????
                {
                    newEnemy.transform.position = new Vector3(spawnPointLeft.transform.position.x, up - paddingY - sprSizeY / 2 - 2 * paddingY * rnd - sprSizeY * rnd, rnd);
                    newEnemy.GetComponent<SpriteRenderer>().transform.localScale = new Vector3(-1, 1, 1);
                }

                newEnemy.GetComponentInChildren<EnemyScript>().enemyOrientationType = enemyType;

                enemiesOnScreen += 1;
                spawnedEnemies += 1;
            }
            else if (levelType == 2)
            {
                GameObject newEnemy = Instantiate(enemies[Random.Range(0, Mathf.Min((int)possibleEnemy, enemies.Length))]);

                int rnd = Random.Range(0, fieldHeight);

                //??? ?????? z-index ????? ?????????? ????????? ?????? ???? ?? ?????
                newEnemy.transform.position = new Vector3(spawnPointRight.transform.position.x, up - paddingY - sprSizeY / 2 - 2 * paddingY * rnd - sprSizeY * rnd, rnd);
                newEnemy.GetComponentInChildren<EnemyScript>().enemyOrientationType = 1;
                enemiesOnScreen += 1;
                spawnedEnemies += 1;
            }

            StartCoroutine(SpawnExtra(it + 1));
        }
    }

    //----------------------------------------
    //----------------------------------------
    //???????? ?????, ?? ??????? ?????? ?????:

    void CreateLevel()
    {
        //?????? ????????
        for (int i = 0; i < fieldHeight; i++)
            for (int j = 0; j < fieldWidth; j++)
                CreateCell(j, i, "right");

        //????? ????????
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
                tmpCell.transform.position = new Vector3(right + paddingX, up - paddingY, 10 + up - paddingY);
            else if (y == 0)
                tmpCell.transform.position = new Vector3(right + paddingX - 2 * paddingX * x - sprSizeX * x, up - paddingY, 10 + up - paddingY);
            else if (x == 0)
                tmpCell.transform.position = new Vector3(right + paddingX, up - paddingY - 2 * paddingY * y - sprSizeY * y, 10 + up - paddingY - 2 * paddingY * y - sprSizeY * y);
            else
                tmpCell.transform.position = new Vector3(right + paddingX - 2 * paddingX * x - sprSizeX * x, up - paddingY - 2 * paddingY * y - sprSizeY * y, 10 + up - paddingY - 2 * paddingY * y - sprSizeY * y);
        }
        else if (type == "left")
        {
            if (x == 0 && y == 0)
                tmpCell.transform.position = new Vector3(-right - sprSizeX - paddingX, up - paddingY, 10 + up - paddingY);
            else if (y == 0)
                tmpCell.transform.position = new Vector3(-right - sprSizeX - paddingX + 2 * paddingX * x + sprSizeX * x, up - paddingY, 10 + up - paddingY);
            else if (x == 0)
                tmpCell.transform.position = new Vector3(-right - sprSizeX - paddingX, up - paddingY - 2 * paddingY * y - sprSizeY * y, 10 + up - paddingY - 2 * paddingY * y - sprSizeY * y);
            else
                tmpCell.transform.position = new Vector3(-right - sprSizeX - paddingX + 2 * paddingX * x + sprSizeX * x, up - paddingY - 2 * paddingY * y - sprSizeY * y, 10 + up - paddingY - 2 * paddingY * y - sprSizeY * y);
        }
    }


    //----------------------------------------
    //----------------------------------------
    //??????? ??? ??????????:

    IEnumerator Replay()
    {
        yield return new WaitForSeconds(2);
        IsWaitingForPress = true;
    }

    public void ToLobbyButton()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(1);
    }

    public void Lose()
    {
        //??????????? ?? ????????? ??????:
        for (int i = GameObject.Find("CanvasForDestroyPanel").transform.childCount - 1; i >= 0; i--)
            Destroy(GameObject.Find("CanvasForDestroyPanel").transform.GetChild(i).gameObject);
        DestroyIsOpen = false;
        if (FindObjectOfType<ShopScript>() != null)
            FindObjectOfType<ShopScript>().CloseShop();
        Resume();

        //??????? ?????? ?? ?????
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        for (int i = 0; i < enemies.Length; i++)
            Destroy(enemies[i].gameObject);

        //??????, ????? ?????? ?? ????????
        GameIsPaused = true;

        //????????? ??????:
        Instantiate(LoseScreenPref, GameObject.Find("Canvas").transform);
    }

    public void Resume()
    {
        PauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;

        if (GameObject.FindGameObjectWithTag("LevelMusic") != null)
        {
            GameObject.FindGameObjectWithTag("LevelMusic").GetComponentInChildren<AudioSource>().UnPause();
            GameObject.FindGameObjectWithTag("LevelMusic").GetComponentInChildren<AudioSource>().volume = PlayerPrefs.GetFloat("MusicVolume", 1f);
        }
    }

    public void Pause()
    {
        for (int i = GameObject.Find("CanvasForDestroyPanel").transform.childCount - 1; i >= 0; i--)
            Destroy(GameObject.Find("CanvasForDestroyPanel").transform.GetChild(i).gameObject);
        DestroyIsOpen = false;

        if (FindObjectOfType<ShopScript>() != null)
            FindObjectOfType<ShopScript>().CloseShop();

        PauseMenuUI.GetComponent<AudioSource>().volume = PlayerPrefs.GetFloat("MusicVolume", 1f);
        PauseMenuUI.SetActive(true);
        Continue.SetActive(true);
        Settings.SetActive(true);
        Home.SetActive(true);
        Back.SetActive(false);
        Slider.SetActive(false);
        Time.timeScale = 0f;
        GameIsPaused = true;

        if (GameObject.FindGameObjectWithTag("LevelMusic") != null)
            GameObject.FindGameObjectWithTag("LevelMusic").GetComponentInChildren<AudioSource>().Pause();
    }

    public void MenuSettings()
    {
        Continue.SetActive(false);
        Settings.SetActive(false);
        Home.SetActive(false);
        Back.SetActive(true);
        Slider.SetActive(true);
        Slider.GetComponent<UnityEngine.UI.Slider>().value = PlayerPrefs.GetFloat("MusicVolume", 1f);
    }

    public void MenuBack()
    {
        Continue.SetActive(true);
        Settings.SetActive(true);
        Home.SetActive(true);
        Back.SetActive(false);
        Slider.SetActive(false);
    }

    private IEnumerator Win()
    {
        LevelIsCompleted = true;
        if (PlayerPrefs.GetInt("LevelComplete") < SceneManager.GetActiveScene().buildIndex - 1)
            PlayerPrefs.SetInt("LevelComplete", SceneManager.GetActiveScene().buildIndex - 1);
        yield return new WaitForSeconds(2);

        nextLevelPortal.SetActive(true);
    }
}
