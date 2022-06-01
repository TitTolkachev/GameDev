using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellScript : MonoBehaviour
{

    public bool hasTower = false;

    public Color BaseColor, CurrColor;

    public GameObject ShopPref, TowerPref;

    private void OnMouseEnter()
    {
        if (FindObjectOfType<ShopScript>() == null)
            GetComponent<SpriteRenderer>().color = CurrColor;
    }

    private void OnMouseExit()
    {
        GetComponent<SpriteRenderer>().color = BaseColor;
    }

    private void OnMouseDown()
    {
        if (FindObjectOfType<ShopScript>() == null)
        {
            if (!hasTower)
            {
                GameObject shopObject = Instantiate(ShopPref);
                shopObject.transform.SetParent(GameObject.Find("Canvas").transform, false);
                shopObject.GetComponent<ShopScript>().selfCell = this;
            }
        }
    }

    public void BuildTower(Tower tower)
    {
        GameObject tmpTower = Instantiate(TowerPref);
        tmpTower.transform.SetParent(transform, false);
        Vector2 towerPos = new Vector2(transform.position.x + tmpTower.GetComponent<SpriteRenderer>().bounds.size.x / 2,
                                       transform.position.y - tmpTower.GetComponent<SpriteRenderer>().bounds.size.y / 2);
        tmpTower.transform.position = towerPos;
        tmpTower.GetComponent<TowerScript>().selfType = (TowerType)tower.type;

        hasTower = true;
        FindObjectOfType<ShopScript>().CloseShop();
    }
}
