using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    int health = 30;

    void Update()
    {
        CheckIsAlive();
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
    }

    void CheckIsAlive()
    {
        if(health <= 0)
        {
            FindObjectOfType<MoneyManagerScript>().GameMoney += 10;
            Destroy(gameObject);
        }
    }
}
