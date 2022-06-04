using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    int health = 30;
    int speed = 1;

    public float CoolDown = 2;

    bool IsAttacking = false;
    public bool IsAlive = true;

    private Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();
    }
    void Update()
    {
        if (IsAlive)
        {
            if (!IsAttacking)
                Move();
            if (health <= 0)
                StartCoroutine(Die());
        }
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        anim.SetTrigger("getHurt");
    }

    IEnumerator Die()
    {
        IsAlive = false;
        anim.SetBool("isAlive", false);
        FindObjectOfType<LevelManagerScript>().enemiesOnScreen -= 1;
        FindObjectOfType<LevelManagerScript>().GameMoney += 10;
        yield return new WaitForSeconds(1);
        Destroy(gameObject);
    }

    void Move()
    {
        Vector3 dir = new Vector3(-1, 0, 0);
        transform.Translate(speed * Time.deltaTime * dir.normalized);

        if (Mathf.Abs(FindObjectOfType<LevelManagerScript>().finishPoint.transform.position.x - transform.position.x) < 0.2f)
        {
            FindObjectOfType<LevelManagerScript>().enemiesOnScreen -= 1;
            FindObjectOfType<LevelManagerScript>().health -= 1;
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Cell" && collision.GetComponent<CellScript>().hasTower)
        {
            IsAttacking = true;
        }
    }

    private IEnumerator ToDamage(Collider2D collision, float time)
    {
        yield return new WaitForSeconds(time);
        collision.GetComponentInChildren<TowerScript>().TakeDamage(10);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Cell" && collision.GetComponent<CellScript>().hasTower)
        {
            CoolDown -= Time.deltaTime;
            if (CoolDown <= 0)
            {
                CoolDown = 4;
                StartCoroutine(ToDamage(collision, 0.4f));
                anim.SetTrigger("attack");
            }
        }
        List<Collider2D> colliders = new List<Collider2D>();
        this.GetComponent<Collider2D>().GetContacts(colliders);

        bool flag = true;
        foreach (Collider2D coll in colliders)
        {
            if (coll.tag == "Cell" && coll.GetComponent<CellScript>().hasTower)
                flag = false;
        }
        if (flag)
            IsAttacking = false;
    }

}
