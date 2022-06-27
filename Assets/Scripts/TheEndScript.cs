using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TheEndScript : MonoBehaviour
{
    public GameObject ContinueText;

    private bool TextAppeared = false;

    public GameObject aud;

    private void Awake()
    {
        AudioSource[] obj = FindObjectsOfType<AudioSource>();
        for (int i = 0; i < obj.Length; i++)
            Destroy(obj[i].gameObject);

        GameObject au = Instantiate(aud);
        au.GetComponent<AudioSource>().volume = PlayerPrefs.GetFloat("MusicVolume");
    }

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
