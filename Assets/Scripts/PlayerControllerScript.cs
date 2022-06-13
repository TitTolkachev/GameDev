using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerScript : MonoBehaviour
{

    Animator animator;
    Rigidbody2D rb;
    SpriteRenderer sprite;

    [SerializeField]
    float speed;

    public int power;

    [SerializeField]
    GameObject attackHitBox;

    bool isAttacking = false;

    private void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();

        attackHitBox.SetActive(false);
    }

    private void Update()
    {
    }

    IEnumerator attack1()
    {
        isAttacking = true;
        animator.SetTrigger("isAttacking");
        yield return new WaitForSeconds(0.25f);
        attackHitBox.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        attackHitBox.SetActive(false);
        isAttacking = false;
    }

    private void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.Space) && !isAttacking)
        {
            StartCoroutine(attack1());
        }

        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S))
        {
            if (Input.GetKey(KeyCode.D))
            {
                transform.position = new Vector3(transform.position.x + speed * Time.deltaTime, transform.position.y, transform.position.z);
                if(!isAttacking)
                    animator.Play("Run");
                transform.localScale = new Vector3(1, 1, 1);
            }
            if (Input.GetKey(KeyCode.A))
            {
                transform.position = new Vector3(transform.position.x - speed * Time.deltaTime, transform.position.y, transform.position.z);
                if (!isAttacking)
                    animator.Play("Run");
                transform.localScale = new Vector3(-1, 1, 1);
            }
            if (Input.GetKey(KeyCode.W))
            {
                transform.position = new Vector3(transform.position.x, transform.position.y + speed * Time.deltaTime, transform.position.z);
                if (!isAttacking)
                    animator.Play("Run");
            }
            if (Input.GetKey(KeyCode.S))
            {
                transform.position = new Vector3(transform.position.x, transform.position.y - speed * Time.deltaTime, transform.position.z);
                if (!isAttacking)
                    animator.Play("Run");
            }
        }
        else
        {
            if (!isAttacking)
                animator.Play("Idle");
        }
    }
}
