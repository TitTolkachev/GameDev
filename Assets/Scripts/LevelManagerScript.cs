using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManagerScript : MonoBehaviour
{

    public int fieldWidth, fieldHeight;
    public float paddingX, paddingY;

    int right = 8, up = 4;

    public GameObject cellPref;

    public Transform cellParent;

    void Start()
    {
        CreateLevel();
    }

    void CreateLevel()
    {
        for(int i = 0; i < fieldHeight; i++)
            for(int j = 0; j < fieldWidth; j++)
                CreateCell(j, i);
    }

    void CreateCell(int x, int y)
    {
        GameObject tmpCell = Instantiate(cellPref);

        tmpCell.transform.parent = cellParent;

        float sprSizeX = tmpCell.GetComponent<SpriteRenderer>().bounds.size.x;
        float sprSizeY = tmpCell.GetComponent<SpriteRenderer>().bounds.size.y;

        if(x == 0 && y == 0)
            tmpCell.transform.position = new Vector3(right + paddingX, up - paddingY, 0);
        else if(y == 0)
            tmpCell.transform.position = new Vector3(right + paddingX - 2 * paddingX * x - sprSizeX * x, up - paddingY, 0);
        else if(x == 0)
            tmpCell.transform.position = new Vector3(right + paddingX, up - paddingY - 2 * paddingY * y - sprSizeY * y, 0);
        else
            tmpCell.transform.position = new Vector3(right + paddingX - 2 * paddingX * x - sprSizeX * x, up - paddingY - 2 * paddingY * y - sprSizeY * y, 0);
    }
}
