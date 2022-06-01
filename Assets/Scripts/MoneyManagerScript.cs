using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoneyManagerScript : MonoBehaviour
{
    public MoneyManagerScript Instance;
    public Text MoneyText;
    public int GameMoney;

    void Awake()
    {
        Instance = this;
    }

    void Update()
    {
        MoneyText.text = GameMoney.ToString();
    }
}
