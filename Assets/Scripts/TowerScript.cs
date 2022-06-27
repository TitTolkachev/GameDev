using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Tower
{
    public int towerType = 1; //1 - самонаведение снарядов, 2 - стандартно линейно
    public int health = 30;
    public string Name;
    public int type, Price;
    public float range, Cooldown, CurrCooldown = 0;
    public Sprite Spr;

    public Tower(int towerType, string name, int type, float range, float cooldown, int price, string path)
    {
        this.towerType = towerType;
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
    public int type = 1; //1 - самонаведение снарядов, 2 - стандартно линейно
    public float speed;
    public int damage;
    public Sprite Spr;

    public TowerProjectile(int type, float speed, int dmg, string path)
    {
        this.type = type;
        this.speed = speed;
        damage = dmg;
        Spr = Resources.Load<Sprite>(path);
    }
}

public enum TowerType
{
    FIRST_TOWER,
    SECOND_TOWER,
    ARCHER_TOWER,
    ARCHER_1,
    ARCHER_2,
    ARCHER_3
}


public class TowerScript : MonoBehaviour
{
    private Animator anim;
    public GameObject projectile;
    public Tower selfTower;
    public TowerType selfType;

    public bool isDying = false;

    public List<Tower> AllTowers = new List<Tower>();
    public List<TowerProjectile> AllProjectiles = new List<TowerProjectile>();

    private void Start()
    {
        anim = GetComponent<Animator>();
        if ((int)selfType == 0)
            anim.runtimeAnimatorController = Resources.Load("Animation/Controller/Tower1") as RuntimeAnimatorController;
        if ((int)selfType == 1)
            anim.runtimeAnimatorController = Resources.Load("Animation/Controller/Tower2") as RuntimeAnimatorController;
        if ((int)selfType == 2)
            anim.runtimeAnimatorController = Resources.Load("Animation/Controller/ArcherTower") as RuntimeAnimatorController;
        if ((int)selfType == 3)
            anim.runtimeAnimatorController = Resources.Load("Animation/Controller/Archer1") as RuntimeAnimatorController;
        if ((int)selfType == 4)
            anim.runtimeAnimatorController = Resources.Load("Animation/Controller/Archer2") as RuntimeAnimatorController;
        if ((int)selfType == 5)
            anim.runtimeAnimatorController = Resources.Load("Animation/Controller/Archer3") as RuntimeAnimatorController;

        AllTowers.Add(new Tower(1, "Tower_1", 0, 2, .3f, 50, "TowerSprites/Tower1"));
        AllTowers.Add(new Tower(1, "Tower_2", 1, 5, 1, 70, "TowerSprites/Tower2"));
        AllTowers.Add(new Tower(2, "Archer Tower", 2, 7, 1, 50, "TowerSprites/ArcherTower"));
        AllTowers.Add(new Tower(2, "Archer1", 3, 7, 1, 50, "TowerSprites/Archer1"));
        AllTowers.Add(new Tower(2, "Archer2", 4, 7, 1, 50, "TowerSprites/Archer2"));
        AllTowers.Add(new Tower(2, "Archer3", 5, 7, 1, 50, "TowerSprites/Archer3"));

        AllProjectiles.Add(new TowerProjectile(1, 7, 2, "ProjectilesSprites/Projectile1"));
        AllProjectiles.Add(new TowerProjectile(1, 3, 10, "ProjectilesSprites/Projectile2"));
        AllProjectiles.Add(new TowerProjectile(2, 5, 10, "ProjectilesSprites/ArcherTowerProjectile"));
        AllProjectiles.Add(new TowerProjectile(2, 7, 10, "ProjectilesSprites/ArcherProjectile"));
        AllProjectiles.Add(new TowerProjectile(2, 9, 20, "ProjectilesSprites/Archer2Projectile"));
        AllProjectiles.Add(new TowerProjectile(2, 7, 5, "ProjectilesSprites/ArcherProjectile"));

        selfTower = AllTowers[(int)selfType];

        GetComponent<SpriteRenderer>().sprite = selfTower.Spr;
        if (transform.position.x < 0)
            GetComponent<SpriteRenderer>().transform.localScale = new Vector3(-1, 1, 1);
    }

    private void Update()
    {
        if (!isDying)
        {
            if (CanShoot())
                SearchTarget();

            CheckIsAlive();

            if (selfTower.CurrCooldown > 0)
            {
                selfTower.CurrCooldown -= Time.deltaTime;
            }
        }
    }

    bool CanShoot()
    {
        if (selfTower.CurrCooldown <= 0)
            return true;
        return false;
    }

    void CheckIsAlive()
    {
        if (selfTower.health <= 0)
            StartCoroutine(Die());
    }

    void SearchTarget()
    {
        if (selfTower.towerType == 1)
        {
            Transform nearestEnemy = null;
            float nearestEnemyDistance = Mathf.Infinity;

            foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy"))
            {
                float currDistance = Vector2.Distance(transform.position, enemy.transform.position);

                if (currDistance <= selfTower.range && currDistance < nearestEnemyDistance
                    && enemy.GetComponent<EnemyScript>().IsAlive
                    && enemy.transform.position.x > 0 == transform.position.x > 0)
                {
                    nearestEnemy = enemy.transform;
                    nearestEnemyDistance = currDistance;
                }
            }

            if (nearestEnemy != null)
                StartCoroutine(Shoot1(nearestEnemy));
        }
        else
        {
            foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy"))
                if (Vector2.Distance(transform.position, enemy.transform.position) <= selfTower.range
                    && enemy.GetComponent<EnemyScript>().IsAlive
                    && Mathf.Abs(enemy.transform.position.y - transform.position.y) < 0.5
                    && enemy.transform.position.x > 0 == transform.position.x > 0)
                    StartCoroutine(Shoot2());
        }
    }

    //Выпуск снаряда для 1 типа башен
    IEnumerator Shoot1(Transform enemy)
    {
        selfTower.CurrCooldown = selfTower.Cooldown;
        anim.SetTrigger("attack");
        yield return new WaitForSeconds(0f);

        GameObject proj = Instantiate(projectile);
        proj.GetComponent<TowerProjectileScript>().selfProjectile = AllProjectiles[(int)selfType];

        proj.transform.position = transform.position;
        proj.GetComponent<TowerProjectileScript>().SetTarget(enemy);
    }

    //Выпуск снаряда для 2 типа башен
    IEnumerator Shoot2()
    {
        selfTower.CurrCooldown = selfTower.Cooldown;
        anim.SetTrigger("attack");
        yield return new WaitForSeconds(0.5f);

        GameObject proj = Instantiate(projectile);
        proj.GetComponent<TowerProjectileScript>().selfProjectile = AllProjectiles[(int)selfType];
        proj.GetComponent<TowerProjectileScript>().projectileRange = (int)AllTowers[(int)selfType].range;
        if (transform.position.x >= 0)
            proj.GetComponent<TowerProjectileScript>().orientation = 1;
        else
            proj.GetComponent<TowerProjectileScript>().orientation = -1;

        if (selfTower.type == 5)
            proj.transform.position = new Vector3(transform.position.x + proj.GetComponent<TowerProjectileScript>().orientation * (-0.2f), transform.position.y + 0.43f, transform.position.z);
        else if (selfTower.type == 4)
            proj.transform.position = new Vector3(transform.position.x + proj.GetComponent<TowerProjectileScript>().orientation * (-0.2f), transform.position.y + 0.3f, transform.position.z);
        else if (selfTower.type == 3)
            proj.transform.position = new Vector3(transform.position.x + proj.GetComponent<TowerProjectileScript>().orientation * (-0.2f), transform.position.y + 0.43f, transform.position.z);
        else if (selfTower.type == 2)
            proj.transform.position = new Vector3(transform.position.x + proj.GetComponent<TowerProjectileScript>().orientation * (-0.2f), transform.position.y + 0.43f, transform.position.z);
    }

    public void TakeDamage(int damage)
    {
        selfTower.health -= damage;
    }

    IEnumerator Die()
    {
        isDying = true;
        anim.SetBool("isAlive", false);
        yield return new WaitForSeconds(1f);
        gameObject.GetComponentInParent<CellScript>().hasTower = false;
        Destroy(gameObject);
    }
}
