using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerProjectileScript : MonoBehaviour
{
    Transform target;
    public TowerProjectile selfProjectile;
    public int projectileRange;
    public int orientation = 1; //1 - летит вправо, -1 - влево
    private float distance = 0;

    private void Start()
    {
        GetComponent<SpriteRenderer>().sprite = selfProjectile.Spr;
        GetComponent<SpriteRenderer>().flipX = orientation == -1;
    }

    void Update()
    {
        Move();
    }

    public void SetTarget(Transform enemy)
    {
        target = enemy;
    }

    private void Move()
    {
        if (selfProjectile.type == 1)
        {
            if (target != null)
            {
                if (Vector2.Distance(transform.position, target.position) < .1f)
                {
                    target.GetComponent<EnemyScript>().TakeDamage(selfProjectile.damage);
                    Destroy(gameObject);
                }
                else
                {
                    Vector2 dir = target.position - transform.position;

                    transform.Translate(dir.normalized * Time.deltaTime * selfProjectile.speed);
                }
            }
            else
                Destroy(gameObject);
        }
        else if (selfProjectile.type == 2)
        {

            Transform nearestEnemy = null;
            float nearestEnemyDistance = Mathf.Infinity;

            foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy"))
            {
                float currDistance = Mathf.Abs(transform.position.x - enemy.transform.position.x);

                if (currDistance < nearestEnemyDistance
                    && enemy.GetComponent<EnemyScript>().IsAlive
                    && Mathf.Abs(transform.position.y - enemy.transform.position.y) < 0.5)
                {
                    nearestEnemy = enemy.transform;
                    nearestEnemyDistance = currDistance;
                }
            }

            if(nearestEnemy != null && Mathf.Abs(transform.position.x - nearestEnemy.position.x) < 0.2f)
            {
                nearestEnemy.GetComponent<EnemyScript>().TakeDamage(selfProjectile.damage);
                Destroy(gameObject);
            }
            else if (distance >= projectileRange 
                || Mathf.Abs(transform.position.x - FindObjectOfType<LevelManagerScript>().spawnPointRight.transform.position.x) < 0.2f
                || Mathf.Abs(transform.position.x - FindObjectOfType<LevelManagerScript>().spawnPointLeft.transform.position.x) < 0.2f)
            {
                Destroy(gameObject);
            }
            else
            {
                distance += Time.deltaTime * selfProjectile.speed;
                transform.position = new Vector3(transform.position.x + orientation * Time.deltaTime * selfProjectile.speed, 
                    transform.position.y, 
                    transform.position.z);
            }
        }
    }
}
