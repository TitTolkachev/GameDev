using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelsMenuScript : MonoBehaviour
{

    public Button[] levelButtons;

    int levelComplete;

    private void Start()
    {
        levelComplete = PlayerPrefs.GetInt("LevelComplete");

        for(int i = levelComplete + 1; i < levelButtons.Length; i++)
        {
            levelButtons[i].interactable = false;
        }
    }

    public void LoadTo(int level)
    {
        SceneManager.LoadScene(level);
    }

    public void ExitButton()
    {
        SceneManager.LoadScene(0);
    }

    public void ResetLevels()
    {
        PlayerPrefs.DeleteKey("LevelComplete");

        levelComplete = PlayerPrefs.GetInt("LevelComplete");
        for (int i = levelComplete + 1; i < levelButtons.Length; i++)
        {
            levelButtons[i].interactable = false;
        }
    }
}
