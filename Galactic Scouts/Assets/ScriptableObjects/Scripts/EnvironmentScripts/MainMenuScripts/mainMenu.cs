using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class mainMenu : MonoBehaviour
{
    private GameObject _MAIN_MENU;
    private GameObject _SETTINGS;

    void Start()
    {
        _MAIN_MENU = GameObject.Find("Main Menu");
        _SETTINGS = GameObject.Find("Settings");
        ToggleSettings(false);
        ToggleMainMenu(true);
        //add audio manager call
    }

    public void PlayGame()
    {
        SceneManager.LoadScene("Level Gameplay");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
    public void SettingsButton()
    {
        ToggleSettings(true);
        ToggleMainMenu(false);
    }
    private void ToggleSettings(bool state)
    {
        _SETTINGS.SetActive(state);
    }
    private void ToggleMainMenu(bool state)
    {
        _MAIN_MENU.SetActive(state);
    }
}
