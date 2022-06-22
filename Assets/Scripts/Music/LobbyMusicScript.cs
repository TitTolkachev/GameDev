using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; 

public class LobbyMusicScript : MonoBehaviour
{
    void Awake()
    {
        if(FindObjectsOfType<AudioSource>().Length == 1)
        {
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);

        FindObjectOfType<AudioSource>().volume = PlayerPrefs.GetFloat("MusicVolume");
    }
}
