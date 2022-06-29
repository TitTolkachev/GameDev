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
        AllTowers.Add(new Tower(1, "Barrel on fire", 0, 2, .3f, 50, "TowerSprites/Tower1", 50));
        AllTowers.Add(new Tower(1, "Santa's gift", 1, 5, 1.2f, 75, "TowerSprites/Tower2", 20));
        AllTowers.Add(new Tower(2, "Archer Tower", 2, 3.5f, 1.7f, 45, "TowerSprites/ArcherTower", 40));
        AllTowers.Add(new Tower(2, "Bone Destroyer", 3, 7, 1.2f, 40, "TowerSprites/Archer1", 20));
        AllTowers.Add(new Tower(2, "Wood Elf", 4, 10, 1, 105, "TowerSprites/Archer2", 50));
        AllTowers.Add(new Tower(2, "Red Hood", 5, 12, 1, 70, "TowerSprites/Archer3", 30));

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
