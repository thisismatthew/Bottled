using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public Sprite Tick, Cross;
    public Image ControllerUISprite, FullscreenUISprite;
    public bool ControllerActive, FullscreenActive = false;
    private OptionsHelper options;
    public Slider volumeSlider;

    private void Start()
    {
        options = FindObjectOfType<OptionsHelper>();
        options.Fullscreen = FullscreenActive;
        options.GamepadController = ControllerActive;
    }

    private void Update()
    {
        options.MasterVolume = volumeSlider.value;
    }

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
            options.GamepadController = false;
            ControllerActive = false;
            ControllerUISprite.sprite = Cross;
        }
        else
        {
            options.GamepadController = true;
            ControllerActive = true;
            ControllerUISprite.sprite = Tick;
        }
    }

    public void SetFullscreen()
    {
        Screen.fullScreen = !Screen.fullScreen;
        if (FullscreenActive)
        {
            options.Fullscreen = false;
            FullscreenActive = false;
            FullscreenUISprite.sprite = Cross;
        }
        else
        {
            options.Fullscreen = true;
            FullscreenActive = true;
            FullscreenUISprite.sprite = Tick;
        }
    }
}
