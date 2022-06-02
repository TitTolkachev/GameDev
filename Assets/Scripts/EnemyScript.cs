using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    int health = 30;
    int speed = 2;

    void Update()
    {
        Move();
        CheckIsAlive();
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
    }

    void CheckIsAlive()
    {
        if (health <= 0)
        {
            FindObjectOfType<LevelManagerScript>().enemiesOnScreen -= 1;
            FindObjectOfType<MoneyManagerScript>().GameMoney += 10;
            Destroy(gameObject);
        }
    }

    void Move()
    {
        Vector3 dir = new Vector3(-1, 0, 0);
        transform.Translate(speed * Time.deltaTime * dir.normalized);

        if(Mathf.Abs(FindObjectOfType<LevelManagerScript>().finishPoint.transform.position.x - transform.position.x) < 0.2f)
        {
            FindObjectOfType<LevelManagerScript>().enemiesOnScreen -= 1;
            Destroy(gameObject);
        }
    }
}
