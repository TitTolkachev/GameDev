using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManagerScript : MonoBehaviour
{

    public GameObject spawnPoint;
    public GameObject finishPoint;
    public GameObject[] enemies;
    public int totalEnemies;//������ � ������
    public int enemiesPerSpawn;//������ � ����� �����
    public int enemiesOnScreen = 0;//������ �� ������
    public int spawnedEnemies = 0;//������������ � ������
    public float spawnDelay = 2;//�������� ����� ������

    public int fieldWidth, fieldHeight;
    public float paddingX, paddingY;

    int right = 8, up = 4;

    public GameObject cellPref;

    public Transform cellParent;

    void Start()
    {
        CreateLevel();
        StartCoroutine(Spawn());
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

            GameObject newEnemy = Instantiate(enemies[0]) as GameObject;

            int rnd = Random.Range(-4, 4);

            //��� ������ z-index ����� ���������� ��������� ������ ���� �� �����
            newEnemy.transform.position = new Vector3(spawnPoint.transform.position.x, rnd - 0.4f + 1, rnd + 4);
            enemiesOnScreen += 1;
            spawnedEnemies += 1;

            StartCoroutine(SpawnExtra(it + 1));
        }
    }

    //----------------------------------------
    //----------------------------------------
    //�������� �����, �� ������� ������ �����:

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
    //������� ��� ����������:

    public void ToLobbyButton()
    {
        SceneManager.LoadScene(0);
    }


}
