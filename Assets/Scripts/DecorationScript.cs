using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecorationScript : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("HitBox"))
        {
            FindObjectOfType<LevelManagerScript>().GameMoney += Random.Range(0, 10);
            Destroy(gameObject);
        }
    }
}
