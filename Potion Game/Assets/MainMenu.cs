using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadScene("Latest_Build_V4");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
