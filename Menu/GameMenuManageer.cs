using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using BlackPearl;
using UnityEngine;

public class GameMenuManageer : MonoBehaviour
{
    public PausedInput pausedInput;

    public GameObject panel_menu, panel_option ,btn_unpaused;

    public bool isPausedGame = false;

    private void Awake()
    {
        pausedInput = new PausedInput();
    }

    void Start()
    {
        panel_menu.SetActive(false);
        panel_option.SetActive(false);
    }

    public void OnEnable()
    {

        pausedInput.PausedMenu.Enable();

    }

    public void OnDisable()
    {

        pausedInput.PausedMenu.Disable();
    }

    public void inputPaused()
    {
        if (pausedInput.PausedMenu.pausedAction.triggered)
        {
            PauseGame();
        }
    }

    public void OnOptionButton()
    {
        panel_option.SetActive(true);
        panel_menu.SetActive(false);
    }

    public void OnBackButton()
    {
        M_Options.instance.saveAudioSettings();
        panel_option.SetActive(false);
        panel_menu.SetActive(true);
    }

    public void PauseGame()
    {
        isPausedGame = !isPausedGame;
        //Inventory.instance.player.SetController(!isPausedGame);
        panel_menu.SetActive(isPausedGame);


    }

    public void OnQuitButton()
    {
        if (panel_menu != null && panel_menu.activeInHierarchy)
        {
            panel_menu.SetActive(false);
        }
        SaveAndLoadManager.instance.saveGame();
        LoadingScreenManager.instance.LoadScene(LoadingScreenManager.instance.menuScene);
    }



    // Update is called once per frame
    void Update()
    {

        inputPaused();


    }
}
