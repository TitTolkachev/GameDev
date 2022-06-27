using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TheEndScript : MonoBehaviour
{
    public GameObject ContinueText;

    private bool TextAppeared = false;

    void Start()
    {
        StartCoroutine(ContinueTextAppearence());
    }

    IEnumerator ContinueTextAppearence()
    {
        yield return new WaitForSeconds(5);
        ContinueText.SetActive(true);
        yield return new WaitForSeconds(2);
        TextAppeared = true;
    }

    void Update()
    {
        if (TextAppeared && Input.anyKeyDown)
            SceneManager.LoadScene(1);
    }
}
