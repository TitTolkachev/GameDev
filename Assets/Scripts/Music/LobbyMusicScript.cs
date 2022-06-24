using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; 

public class LobbyMusicScript : MonoBehaviour
{
    void Awake()
    {

        GameObject[] auds = GameObject.FindGameObjectsWithTag("LevelMusic");
        for(int i = 0; i < auds.Length; i++)
            Destroy(auds[i]);

        if(GameObject.FindGameObjectsWithTag("LevelMusic") != null && FindObjectsOfType<AudioSource>().Length - GameObject.FindGameObjectsWithTag("LevelMusic").Length == 1 
            || GameObject.FindGameObjectsWithTag("LevelMusic") == null && FindObjectsOfType<AudioSource>().Length == 1)
        {
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);

        AudioSource[] aauds = FindObjectsOfType<AudioSource>();
        for(int i = 0; i < aauds.Length; i++)
            aauds[i].volume = PlayerPrefs.GetFloat("MusicVolume", 1f);
    }
}
