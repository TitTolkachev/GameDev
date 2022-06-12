using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    public int enemyOrientationType;//1 - правый, 2 - левый
    public int health = 30;
    public float speed = 1;

    public float CoolDown;
    public float FirstCoolDownToAttack = 0.5f;
    public float DefaultCoolDownToAttack = 2;

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
        Vector3 dir;
        if (enemyOrientationType == 1)
            dir = new(-1, 0, 0);
        else 
            dir = new(1, 0, 0);

        transform.Translate(speed * Time.deltaTime * dir.normalized);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Cell") && collision.GetComponent<CellScript>().hasTower)
        {
            IsAttacking = true;
            CoolDown = FirstCoolDownToAttack;
        }
        if(collision.CompareTag("Finish"))
        {
            FindObjectOfType<LevelManagerScript>().enemiesOnScreen -= 1;
            FindObjectOfType<LevelManagerScript>().health -= 1;
            Destroy(gameObject);
        }
    }

    private IEnumerator ToDamage(Collider2D collision, float time)
    {
        yield return new WaitForSeconds(time);
        collision.GetComponentInChildren<TowerScript>().TakeDamage(10);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Cell") && collision.GetComponent<CellScript>().hasTower && IsAlive)
        {
            IsAttacking = true;
            CoolDown -= Time.deltaTime;
            if (CoolDown <= 0)
            {
                CoolDown = DefaultCoolDownToAttack;
                anim.SetTrigger("attack");
                StartCoroutine(ToDamage(collision, 0.4f));
            }
        }
        List<Collider2D> colliders = new();
        this.GetComponent<Collider2D>().GetContacts(colliders);

        bool flag = true;
        foreach (Collider2D coll in colliders)
        {
            if (coll.CompareTag("Cell") && coll.GetComponent<CellScript>().hasTower)
                flag = false;
        }
        if (flag)
            IsAttacking = false;
    }

}
