using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Platformer;

public class MenuController : MonoBehaviour
{
    public static MenuController instance;

    //Methods
    private void Awake()
    {
        instance = this;
        Hide();
    }
    public void Show() {
        ShowMainMenu();
        gameObject.SetActive(true);
        Time.timeScale = 0;
        PlayerController.instance.isPaused = true;
    }
    public void Hide() {
        gameObject.SetActive(false);
        Time.timeScale = 1;
        if (PlayerController.instance != null) {
            PlayerController.instance.isPaused = false;
        } 

    }
    public void LoadLevel() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    // Outlets
    public GameObject mainMenu;
    public GameObject OptionsMenu;
    public GameObject levelMenu;

    void SwitchMenu(GameObject someMenu)
    {
        //Clean-up Menus
        mainMenu.SetActive(false);
        OptionsMenu.SetActive(false);
        levelMenu.SetActive(false);

        //Turn on reqiested menu
        someMenu.SetActive(true);
    }
    public void ShowMainMenu() {
        SwitchMenu(mainMenu);
    }
    public void ShowOptionsMenu()
    {
        SwitchMenu(OptionsMenu);
    }
    public void ShowLevelMenu()
    {
        SwitchMenu(levelMenu);
    }
}
