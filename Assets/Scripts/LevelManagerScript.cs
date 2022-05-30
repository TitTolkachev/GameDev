using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManagerScript : MonoBehaviour
{

    public int fieldWidth, fieldHeight;
    public float paddingX, paddingY;

    public GameObject cellPref;

    public Transform cellParent;

    void Start()
    {
        CreateLevel();
    }

    void CreateLevel()
    {
        Vector3 worldVec = Camera.main.ScreenToWorldPoint(new Vector3(0, Screen.height, 0));

        for(int i = 0; i < fieldHeight; i++)
        {
            for(int j = 0; j < fieldWidth; j++)
            {
                CreateCell(j, i, worldVec);
            }
        }
    }

    void CreateCell(int x, int y, Vector3 wV)
    {
        GameObject tmpCell = Instantiate(cellPref);

        tmpCell.transform.parent = cellParent;

        float sprSizeX = tmpCell.GetComponent<SpriteRenderer>().bounds.size.x;
        float sprSizeY = tmpCell.GetComponent<SpriteRenderer>().bounds.size.y;

        tmpCell.transform.position = new Vector3(paddingX*(x+1) + wV.x + sprSizeX * x, -paddingY*(y+1) + wV.y + sprSizeY * (-y), 0);
    }
}
