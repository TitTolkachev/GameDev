using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TutorialScript : MonoBehaviour
{

    int state = 0; // номер слайда

    [SerializeField]
    GameObject NextBtn;

    [SerializeField]
    GameObject PrevBtn;

    [SerializeField]
    GameObject ExitBtn;

    [SerializeField]
    GameObject[] Panels;

    public void NextBtnFun()
    {
        if (state == Panels.Length - 1)
        {
            SceneManager.LoadScene(1);
            return;
        }
        else if (state == Panels.Length - 2)
        {
            NextBtn.GetComponentInChildren<Text>().text = "finish";
        }
        else if (state == 0)
            PrevBtn.SetActive(true);

        Panels[state].SetActive(false);
        Panels[state + 1].SetActive(true);

        state++;
    }
    public void PrevBtnFun()
    {
        Panels[state].SetActive(false);
        Panels[state - 1].SetActive(true);

        state--;

        if (state == 0)
        {
            PrevBtn.SetActive(false);
        }
        else if (state == Panels.Length - 2)
        {
            NextBtn.GetComponentInChildren<Text>().text = "next";
        }
    }
    public void ExitBtnFun()
    {
        SceneManager.LoadScene(1);
    }
}
