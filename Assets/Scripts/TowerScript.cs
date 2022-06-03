using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Tower
{
    public int health = 30;
    public string Name;
    public int type, Price;
    public float range, Cooldown, CurrCooldown = 0;
    public Sprite Spr;

    public Tower(string name, int type, float range, float cooldown, int price, string path)
    {
        this.range = range;
        this.type = type;
        Cooldown = cooldown;
        Spr = Resources.Load<Sprite>(path);
        Name = name;
        Price = price;
    }
}

public class TowerProjectile
{
    public float speed;
    public int damage;
    public Sprite Spr;

    public TowerProjectile(float speed, int dmg, string path)
    {
        this.speed = speed;
        damage = dmg;
        Spr = Resources.Load<Sprite>(path);
    }
}

public enum TowerType
{
    FIRST_TOWER,
    SECOND_TOWER
}


public class TowerScript : MonoBehaviour
{
    public GameObject projectile;
    public Tower selfTower;
    public TowerType selfType;

    public List<Tower> AllTowers = new List<Tower>();
    public List<TowerProjectile> AllProjectiles = new List<TowerProjectile>();

    private void Start()
    {
        AllTowers.Add(new Tower("Tower_1", 0, 2, .3f, 50, "TowerSprites/FTower"));
        AllTowers.Add(new Tower("Tower_2", 1, 5, 1, 70, "TowerSprites/STower"));

        AllProjectiles.Add(new TowerProjectile(7, 2, "ProjectilesSprites/FProjectile"));
        AllProjectiles.Add(new TowerProjectile(3, 10, "ProjectilesSprites/SProjectile"));

        selfTower = AllTowers[(int)selfType];
        GetComponent<SpriteRenderer>().sprite = selfTower.Spr;
    }

    private void Update()
    {
        if(CanShoot())
            SearchTarget();

        CheckIsAlive();

        if(selfTower.CurrCooldown > 0)
        {
            selfTower.CurrCooldown -= Time.deltaTime;
        }
    }

    bool CanShoot()
    {
        if(selfTower.CurrCooldown <= 0)
            return true;
        return false;
    }

    void CheckIsAlive()
    {
        if (selfTower.health <= 0)
        {
            gameObject.GetComponentInParent<CellScript>().hasTower = false;
            Destroy(gameObject);
        }
    }

    void SearchTarget()
    {
        Transform nearestEnemy = null;
        float nearestEnemyDistance = Mathf.Infinity;

        foreach(GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            float currDistance = Vector2.Distance(transform.position, enemy.transform.position);

            if(currDistance <= selfTower.range && currDistance < nearestEnemyDistance 
                && enemy.GetComponent<EnemyScript>().IsAlive)
            {
                nearestEnemy = enemy.transform;
                nearestEnemyDistance = currDistance;
            }
        }

        if (nearestEnemy != null)
            Shoot(nearestEnemy);
    }

    void Shoot(Transform enemy)
    {
        selfTower.CurrCooldown = selfTower.Cooldown;

        GameObject proj = Instantiate(projectile);
        proj.GetComponent<TowerProjectileScript>().selfProjectile = AllProjectiles[(int)selfType];

        proj.transform.position = transform.position;
        proj.GetComponent<TowerProjectileScript>().SetTarget(enemy);
    }

    public void TakeDamage(int damage)
    {
        selfTower.health -= damage;
    }
}
