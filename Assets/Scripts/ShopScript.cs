using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopScript : MonoBehaviour
{
    public GameObject ItemPref;
    public Transform ItemGrid;

    public List<Tower> AllTowers = new List<Tower>();

    public CellScript selfCell;

    void Start()
    {

        AllTowers.Add(new Tower(1, "Tower_1", 0, 2, .3f, 50, "TowerSprites/FTower"));
        AllTowers.Add(new Tower(1, "Tower_2", 1, 5, 1, 70, "TowerSprites/STower"));
        AllTowers.Add(new Tower(2, "Archer", 2, 7, 1, 50, "TowerSprites/ArcherTower"));

        foreach (Tower tower in AllTowers)
        {
            GameObject tmpItem = Instantiate(ItemPref);
            tmpItem.transform.SetParent(ItemGrid, false);
            tmpItem.GetComponent<ShopItemScript>().SetStartData(tower, selfCell);
        }
    }

    public void CloseShop()
    {
        Destroy(gameObject);
    }
}
