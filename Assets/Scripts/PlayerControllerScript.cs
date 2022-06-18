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

    public float left, right, up, down;

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
                Vector3 vect = new(transform.position.x + speed * Time.deltaTime, transform.position.y, transform.position.z);
                vect.x = Mathf.Min(vect.x, right);
                vect.x = Mathf.Max(vect.x, left);
                vect.y = Mathf.Min(vect.y, up);
                vect.y = Mathf.Max(vect.y, down);
                transform.position = vect;
                if(!isAttacking)
                    animator.Play("Run");
                transform.localScale = new Vector3(1, 1, 1);
            }
            if (Input.GetKey(KeyCode.A))
            {
                Vector3 vect = new(transform.position.x - speed * Time.deltaTime, transform.position.y, transform.position.z);
                vect.x = Mathf.Min(vect.x, right);
                vect.x = Mathf.Max(vect.x, left);
                vect.y = Mathf.Min(vect.y, up);
                vect.y = Mathf.Max(vect.y, down);
                transform.position = vect;

                if (!isAttacking)
                    animator.Play("Run");
                transform.localScale = new Vector3(-1, 1, 1);
            }
            if (Input.GetKey(KeyCode.W))
            {
                Vector3 vect = new(transform.position.x, transform.position.y + speed * Time.deltaTime, transform.position.z);
                vect.x = Mathf.Min(vect.x, right);
                vect.x = Mathf.Max(vect.x, left);
                vect.y = Mathf.Min(vect.y, up);
                vect.y = Mathf.Max(vect.y, down);
                transform.position = vect;

                if (!isAttacking)
                    animator.Play("Run");
            }
            if (Input.GetKey(KeyCode.S))
            {
                Vector3 vect = new(transform.position.x, transform.position.y - speed * Time.deltaTime, transform.position.z);
                vect.x = Mathf.Min(vect.x, right);
                vect.x = Mathf.Max(vect.x, left);
                vect.y = Mathf.Min(vect.y, up);
                vect.y = Mathf.Max(vect.y, down);
                transform.position = vect;

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
