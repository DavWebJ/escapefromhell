using System.Collections;
using System.Collections.Generic;
using BlackPearl;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine;

public class MainMenuManager : MonoBehaviour
{
    public enum MenuType 
    {
        main,option,game,continueGame
    }
    public enum LoadingScreenType 
    { 
        newGameScreen, continueGameScreen,main 
    }
    public PausedInput pausedInput;
    public static MainMenuManager instance;
    public MenuType menuType;
    public LoadingScreenType loadingScreenType;
    public AudioClip clickStart;
    public Camera main;
    public AudioSource AudioSource,bg;
    public AudioSource buttonStartAudio;
    public AudioClip start,hover;
    public GameObject panel_start, panel_menu, btn_continueGame, btn_newGame, loadinScreenGame,panel_option,btn_unpaused;
    AsyncOperation async;
    public Image fill,back_hover,toggle_hover;
    private string gameScene = "game";
    private string menuScene = "intro";
    private string testScene = "test";
    private string cutScene = "cutintro";
    public float speed = 0.3f;
    public string[] tips;
    public Image settingsImage;
    public Text infoTxtGame;
    public bool isRotating = false;
    public bool isBackHover = false;
    public bool toggleIsHover = false;
    public bool isPausedGame = false;
    public bool isInGame = false;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }

        pausedInput = new PausedInput();
        
    }
    void Start()
    {
        main = FindObjectOfType<Camera>();
        AudioSource = GetComponent<AudioSource>();
        if (isInGame)
        {
            panel_start.SetActive(false);
            AudioSource.loop = false;
            AudioSource.playOnAwake = false;
            AudioSource.Stop();
        }
        else
        {
            panel_start.SetActive(true);

            
        }
        panel_menu.SetActive(false);
        panel_option.SetActive(false);
        loadinScreenGame = GameObject.FindGameObjectWithTag("loadingscreen");
        //fill = loadinScreenGame.transform.Find("Slider/Image/fill").GetComponent<Image>();
        infoTxtGame = loadinScreenGame.transform.Find("txtInfoGame").GetComponent<Text>();
        settingsImage = loadinScreenGame.transform.Find("animsettings").GetComponentInChildren<Image>();
        loadinScreenGame.SetActive(false);
        Scene currScene = SceneManager.GetActiveScene();
        back_hover.enabled = false;
        toggle_hover.enabled = false;
        if (!isInGame)
        {
            btn_unpaused.SetActive(false);
        }



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


    public void OnStartButton()
    {
        //AudioSource.clip = start;
        
        AudioSource.Play();
        main.GetComponent<CameraMenuMove>().animator.enabled = false;
        main.GetComponent<CameraMenuMove>().backToOrigin = true;
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

    public void PauseGame()
    {
        isPausedGame = !isPausedGame;
        Inventory.instance.player.SetController(!isPausedGame);
        panel_menu.SetActive(isPausedGame);
        btn_unpaused.SetActive(isPausedGame && isInGame);
        btn_newGame.SetActive(!isPausedGame);
        btn_continueGame.SetActive(!isPausedGame);
        
    }
    public void OnPlayButton()
    {

        
        AudioSource.PlayOneShot(clickStart);
        panel_menu.SetActive(false);
        main.GetComponent<CameraMenuMove>().GoToGameLauncher();
        
        AudioM.instance.StopAllSound(bg);

    }

    public void LoadGameScene()
    {
        loadingScreenType = LoadingScreenType.newGameScreen;
        LoadScene(gameScene);
    }

    public void LoadCutSceneIntro()
    {
        loadingScreenType = LoadingScreenType.newGameScreen;
        LoadScene(gameScene);
        //LoadScene(cutScene);
    }

    public void OnContinueButton()
    {
        loadingScreenType = LoadingScreenType.continueGameScreen;
        LoadScene(gameScene);
    }



    public void OnQuitButton()
    {
        SaveAndLoadManager.instance.saveGame();
        Application.Quit();
    }


    public void backToMainMenu()
    {


        loadingScreenType = LoadingScreenType.main;

        LoadScene(menuScene);

    }

    public void loadingMainMenu()
    {


        loadingScreenType = LoadingScreenType.main;

        SaveAndLoadManager.instance.saveGame();
        LoadScene(menuScene);

    }

    public void LoadScene(string scene)
    {

            if (panel_menu != null && panel_menu.activeInHierarchy)
            {
                panel_menu.SetActive(false);
            }
        

        //if (pauseMenuscreen != null)
        //{
        //    if (pauseMenuscreen.activeInHierarchy)
        //    {
        //        pauseMenuscreen.SetActive(false);
        //    }
        //}
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        switch (scene)
        {
            case "main":

                menuType = MenuType.main;

                break;
            case "game":

                menuType = MenuType.game;

                break;
            case "option":
                menuType = MenuType.option;
                break;
            case "cutscene":
                menuType = MenuType.game;
                break;
            case "continueGame":
                menuType = MenuType.continueGame;

                break;

        }
        StartCoroutine(Loading(scene, menuType));
    }


    IEnumerator Loading(string scene, MenuType type)
    {

        fill.fillAmount = 0;

        loadinScreenGame.SetActive(true);
        infoTxtGame.text = "Chargement...";

        AudioM.instance.enableThunder = false;
        AsyncOperation operation = SceneManager.LoadSceneAsync(scene);
        operation.allowSceneActivation = false;
        float progressValueGame = 0;
        isRotating = true;
        while (!operation.isDone)
        {
            progressValueGame = Mathf.MoveTowards(progressValueGame, operation.progress, Time.deltaTime);
            fill.fillAmount = progressValueGame;

            switch (loadingScreenType)
            {
                case LoadingScreenType.newGameScreen:
                    infoTxtGame.text = "chargement...";
                    if (progressValueGame <= 0.1f)
                    {

                        infoTxtGame.text = "Chargement...";
                        yield return new WaitForSeconds(0.5f);

                    }

                    if (progressValueGame >= 0.9f)
                    {

                        isRotating = false;
                        fill.fillAmount = 1;
                        operation.allowSceneActivation = true;
                        isInGame = true;
                    }
                    break;
                case LoadingScreenType.continueGameScreen:
                    infoTxtGame.text = "chargement...";

                    if (progressValueGame >= 0.4f && progressValueGame <= 0.5f)
                    {

                        infoTxtGame.text = "Chargement...";
                        yield return new WaitForSeconds(0.5f);

                    }

                    if (progressValueGame >= 0.75f && progressValueGame <= 0.8f)
                    {
                        string tipsRandom = tips[Random.Range(0, tips.Length)];
                        infoTxtGame.text = tipsRandom;
                        yield return new WaitForSeconds(3f);
                    }
                    if (progressValueGame >= 0.9f)
                    {

                        isRotating = false;
                        fill.fillAmount = 1;

                        operation.allowSceneActivation = true;
                        isInGame = true;
                    }
                    break;
               
                case LoadingScreenType.main:
                    infoTxtGame.text = "chargement...";
                    if (progressValueGame >= 0.8f && progressValueGame <= 0.9f)
                    {
                        infoTxtGame.text = "Chargement du menu principal...";
                        yield return new WaitForSeconds(0.5f);

                    }
                    isInGame = false;
                    break;
                
                default:
                    infoTxtGame.text = "chargement...";
                    break;
            }
            if (progressValueGame >= 0.9f)
            {
                isRotating = false;
                fill.fillAmount = 1;
                operation.allowSceneActivation = true;

            }


            yield return null;

        }
    }

    private void FixedUpdate()
    {
        if (isRotating)
        {
            settingsImage.transform.Rotate(new Vector3(0, 0, 360) * Time.deltaTime * speed);
        }
    }
    void Update()
    {
            
            back_hover.enabled = isBackHover;
            toggle_hover.enabled = toggleIsHover && M_Options.instance.isFull;

        if (isInGame)
        {
            inputPaused();
        }
        if(isInGame && isPausedGame)
        {
            btn_continueGame.SetActive(false);
            btn_newGame.SetActive(false);
        }
        if (!isInGame)
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

    }




}
