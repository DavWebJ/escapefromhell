using System.Collections;
using System.Collections.Generic;
using BlackPearl;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine;

public class MainMenuManager : MonoBehaviour
{
    public enum MenuType 
    {
        main,option,game,continueGame
    }

    
    public static MainMenuManager instance;
    public MenuType menuType;
    
    public AudioClip clickStart;
    public Camera main;
    public CameraMenuMove cameraIntro;
    public AudioSource AudioSource,bg;
    public AudioClip start,hover;
    public GameObject panel_start, panel_menu, btn_continueGame, btn_newGame, panel_option;//btn_unpaused;
    
    public Image back_hover,toggle_hover;

    public bool isBackHover = false;
    public bool toggleIsHover = false;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }

        
        
    }
    void Start()
    {
        main = FindObjectOfType<Camera>();
        cameraIntro = main.GetComponent<CameraMenuMove>();
        AudioSource = GetComponent<AudioSource>();
        panel_start.SetActive(true);
        panel_menu.SetActive(false);
        panel_option.SetActive(false);

        back_hover.enabled = false;
        toggle_hover.enabled = false;




    }

    public void OnStartButton()
    {
        
        
        AudioSource.Play();
  
        cameraIntro.MoveToTheMenu();
        
    }

    public void DisableButtonStart()
    {
        panel_start.SetActive(false);
    }

    public void Hover()
    {
        AudioSource.clip = hover;
        AudioSource.PlayOneShot(hover);
    }

    public void BackHover()
    {
        AudioSource.clip = hover;
        AudioSource.PlayOneShot(hover);
        isBackHover = true;
    }
    public void EndBackHover()
    {
        isBackHover = false;
    }
    public void ToggleHover()
    {
       
        
        toggleIsHover = true;
    }
    public void EndToggleHover()
    {
        toggleIsHover = false;
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


    public void OnPlayButton()
    {

        
        AudioSource.PlayOneShot(clickStart);
        panel_menu.SetActive(false);
        cameraIntro.MoveToGame();
        
        AudioM.instance.StopAllSound(bg);

    }

    public void LoadGameScene()
    {
        if (panel_menu != null && panel_menu.activeInHierarchy)
        {
            panel_menu.SetActive(false);
        }
        LoadingScreenManager.instance.LoadScene(LoadingScreenManager.instance.gameScene);
    }



    public void OnContinueButton()
    {
        if (panel_menu != null && panel_menu.activeInHierarchy)
        {
            panel_menu.SetActive(false);
        }
        LoadingScreenManager.instance.LoadScene(LoadingScreenManager.instance.gameScene);
    }



    public void OnQuitButton()
    {
        SaveAndLoadManager.instance.saveGame();
        Application.Quit();
    }


    public void onShowMenu()
    {
        if (SaveAndLoadManager.instance.checkIfGameDataExist())
        {
            btn_continueGame.SetActive(true);
            btn_newGame.SetActive(false);
            //naviguationSystem.checkBtnMenu(btn_continueGame);
        }
        else
        {
            btn_continueGame.SetActive(false);
            btn_newGame.SetActive(true);
            //naviguationSystem.checkBtnMenu(btn_newGame);
        }
    }


    void Update()
    {
            
            back_hover.enabled = isBackHover;
            toggle_hover.enabled = toggleIsHover && M_Options.instance.isFull;




        

    }




}
