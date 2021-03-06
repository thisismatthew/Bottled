using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class PauseMenu : MonoBehaviour
{
    public bool GameIsPaused = false;

    public GameObject PauseMenuUI;
    public GameObject OptionsMenuUI;
    //public GameObject GameUI;
    private TMP_Animated textCoroutine;

    private void Start()
    {
        textCoroutine = FindObjectOfType<TMP_Animated>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameIsPaused)
            {
                Resume();
            } 
            else
            {
                Pause();
            }
        }
    }
    public void Resume()
    {
        textCoroutine.ReadPaused = false;
        OptionsMenuUI.SetActive(false);
        PauseMenuUI.SetActive(false);
        //GameUI.SetActive(true);
        Time.timeScale = 1f;
        GameIsPaused = false;
    }
    public void Pause()
    {
        textCoroutine.ReadPaused = true;
        PauseMenuUI.SetActive(true);
        //GameUI.SetActive(false);
        Time.timeScale = 0f;
        GameIsPaused = true;
        //Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }
    public void LoadMenu()
    {
        Time.timeScale = 1f;
        GameIsPaused = false;
        SceneManager.LoadScene("Main_Menu_V4");
    }
    public void QuitGame()
    {
        Application.Quit();
    }

    public void OptionsToggle(bool val)
    {
        OptionsMenuUI.SetActive(val);
        PauseMenuUI.SetActive(!val);
    }


}
