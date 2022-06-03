using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    int health = 30;
    int speed = 1;

    public float CoolDown = 2;

    bool IsAttacking = false;

    void Update()
    {
        if (!IsAttacking)
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Cell" && collision.GetComponent<CellScript>().hasTower)
        {
            IsAttacking = true;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Cell" && collision.GetComponent<CellScript>().hasTower)
        {
            CoolDown -= Time.deltaTime;
            if(CoolDown <= 0)
            {
                CoolDown = 4;
                collision.GetComponentInChildren<TowerScript>().TakeDamage(10);
            }
        }
        List<Collider2D> colliders = new List<Collider2D>();
        this.GetComponent<Collider2D>().GetContacts(colliders);

        bool flag = true;
        foreach(Collider2D coll in colliders)
        {
            if(coll.tag == "Cell" && coll.GetComponent<CellScript>().hasTower)
                flag = false;
        }
        if(flag)
            IsAttacking = false;
    }

}
