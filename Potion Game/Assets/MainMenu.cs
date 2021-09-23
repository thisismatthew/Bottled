using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public Sprite Tick, Cross;
    public Image ControllerSettings, FullscreenSettings;
    public bool ControllerActive, FullscreenActive = false;
    public void PlayGame()
    {
        SceneManager.LoadScene("Latest_Build_V4");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void SetControllerSettings()
    {
        if (ControllerActive)
        {
            ControllerActive = false;
            ControllerSettings.sprite = Cross;
        }
        else
        {
            ControllerActive = true;
            ControllerSettings.sprite = Tick;
        }
    }

    public void SetFullscreen()
    {
        if (FullscreenActive)
        {
            FullscreenActive = false;
            FullscreenSettings.sprite = Cross;
        }
        else
        {
            FullscreenActive = true;
            FullscreenSettings.sprite = Tick;
        }
    }
}
