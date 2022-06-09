using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellScript : MonoBehaviour
{

    public bool hasTower = false;

    public Color BaseColor, CurrColor, DestroyColor;

    public GameObject ShopPref, TowerPref, DestroyTowerPref;

    private void OnMouseEnter()
    {
        if (FindObjectOfType<ShopScript>() == null
            && !FindObjectOfType<LevelManagerScript>().GameIsPaused
            && !FindObjectOfType<LevelManagerScript>().DestroyIsOpen)
        {
            if (!hasTower)
                GetComponent<SpriteRenderer>().color = CurrColor;
            else
                GetComponent<SpriteRenderer>().color = DestroyColor;
        }
    }

    private void OnMouseExit()
    {
        GetComponent<SpriteRenderer>().color = BaseColor;
    }

    private void OnMouseDown()
    {
        if (FindObjectOfType<ShopScript>() == null
            && !FindObjectOfType<LevelManagerScript>().GameIsPaused
            && !FindObjectOfType<LevelManagerScript>().DestroyIsOpen)
        {
            if (!hasTower)
            {
                GameObject shopObject = Instantiate(ShopPref);
                shopObject.transform.SetParent(GameObject.Find("Canvas").transform, false);
                shopObject.GetComponent<ShopScript>().selfCell = this;
            }
            else
            {
                FindObjectOfType<LevelManagerScript>().destroyingTower = GetComponentInChildren<TowerScript>();
                FindObjectOfType<LevelManagerScript>().DestroyIsOpen = true;
                GameObject destroyTowerObject = Instantiate(DestroyTowerPref);
                destroyTowerObject.transform.SetParent(GameObject.Find("CanvasForDestroyPanel").transform, false);
                destroyTowerObject.transform.position = new Vector3(
                    gameObject.transform.position.x + TowerPref.GetComponent<SpriteRenderer>().bounds.size.x / 2,
                    gameObject.transform.position.y + 0.3f,
                    gameObject.transform.position.z);
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

    public void DestroyTower()
    {
        FindObjectOfType<LevelManagerScript>().GameMoney += FindObjectOfType<TowerScript>().selfTower.Price / 2;
        FindObjectOfType<LevelManagerScript>().destroyingTower.selfTower.health = 0;
        CancelDestroying();
    }

    public void CancelDestroying()
    {
        FindObjectOfType<LevelManagerScript>().DestroyIsOpen = false;
        Destroy(GameObject.Find("DestroyTowerPanel(Clone)"));
    }
}
