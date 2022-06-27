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

        AllTowers.Add(new Tower(1, "Tower_1", 0, 2, .3f, 50, "TowerSprites/Tower1"));
        AllTowers.Add(new Tower(1, "Tower_2", 1, 5, 1, 70, "TowerSprites/Tower2"));
        AllTowers.Add(new Tower(2, "Archer Tower", 2, 7, 1, 50, "TowerSprites/ArcherTower"));
        AllTowers.Add(new Tower(2, "Archer1", 3, 7, 1, 50, "TowerSprites/Archer1"));
        AllTowers.Add(new Tower(2, "Archer2", 4, 7, 1, 50, "TowerSprites/Archer2"));
        AllTowers.Add(new Tower(2, "Archer3", 5, 7, 1, 50, "TowerSprites/Archer3"));

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
