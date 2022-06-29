using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LobbyButtonsScript : MonoBehaviour
{

    public void StartButton()
    {
        SceneManager.LoadScene(1);
    }

    public void ExitButton()
    {
        Application.Quit();
    }

    public void BackButton()
    {
        SceneManager.LoadScene(0);
    }

    public void AuthorButton()
    {
        SceneManager.LoadScene(14);
    }

}
