using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void StartClicked()
    {
        SceneManager.LoadScene("TestingScene");
    }

    public void ExitClicked()
    {
       Application.Quit();
    }
}
