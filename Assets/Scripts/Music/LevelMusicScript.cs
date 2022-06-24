using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelMusicScript : MonoBehaviour
{
    private void Awake()
    {
        if(GameObject.FindGameObjectsWithTag("LevelMusic").Length == 1)
        {
            if(gameObject.CompareTag("LevelMusic"))
                DontDestroyOnLoad(gameObject);
            GetComponentInChildren<AudioSource>().volume = PlayerPrefs.GetFloat("MusicVolume", 1f);
        }
        else
            Destroy(gameObject);
    }
}
