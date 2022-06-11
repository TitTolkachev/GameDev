using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelsMenuScript : MonoBehaviour
{

    public void StartLevel1()
    {
        SceneManager.LoadScene(2);
    }

    public void StartLevel2()
    {
        SceneManager.LoadScene(3);
    }

    public void ExitButton()
    {
        SceneManager.LoadScene(0);
    }
}
