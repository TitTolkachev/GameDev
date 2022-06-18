using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextLevelPortalScript : MonoBehaviour
{

    [SerializeField]
    Animator animator;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.gameObject.SetActive(false);
            StartCoroutine(NextLevel());
        }
    }

    private IEnumerator NextLevel()
    {
        Time.timeScale = 1f;
        animator.SetTrigger("Close");
        yield return new WaitForSeconds(1.2f);

        if (SceneManager.GetActiveScene().buildIndex + 1 < 5)
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        else
            SceneManager.LoadScene(1);
    }
}
