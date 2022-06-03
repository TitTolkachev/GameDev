using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ShopItemScript : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    Tower selfTower;
    CellScript selfCell;
    public Image TowerLogo;
    public Text TowerName, TowerPrice;

    public Color BaseColor, CurrColor;

    public void SetStartData(Tower tower, CellScript cell)
    {
        selfTower = tower;
        TowerLogo.sprite = tower.Spr;
        TowerName.text = tower.Name;
        TowerPrice.text = tower.Price.ToString();
        selfCell = cell;
    }

    public void OnPointerEnter(PointerEventData eventData)//���������
    {

        GetComponent<Image>().color = CurrColor;
    }

    public void OnPointerExit(PointerEventData eventData)//�����
    {
        GetComponent<Image>().color = BaseColor;
    }

    public void OnPointerClick(PointerEventData eventData)//�������
    {
        if(FindObjectOfType<LevelManagerScript>().GameMoney >= selfTower.Price)
        {
            selfCell.BuildTower(selfTower);
            FindObjectOfType<LevelManagerScript>().GameMoney -= selfTower.Price;
        }
    }
}
