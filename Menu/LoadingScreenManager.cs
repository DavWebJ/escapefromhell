using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine;

public enum LoadingScreenType
{
    newGameScreen, continueGameScreen, MainMenu,Transition,CutScene
}
public class LoadingScreenManager : MonoBehaviour
{
    public static LoadingScreenManager instance;

    public LoadingScreenType loadingScreenType;
    public string gameScene = "game";
    public string menuScene = "intro";
    public float speed = 0.3f;
    public GameObject title;
    public string[] tips;
    public Image settingsImage, fill;
    public Text infoTxtGame;
    public bool isRotating = false;

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }
    void Start()
    {
        
        //fill = loadinScreenGame.transform.Find("Slider/Image/fill").GetComponent<Image>();
        infoTxtGame = transform.Find("txtInfoGame").GetComponent<Text>();
        settingsImage = transform.Find("animsettings").GetComponentInChildren<Image>();
        
        title = transform.Find("Title").gameObject;
        title.SetActive(false);
        gameObject.SetActive(false);
        Scene currScene = SceneManager.GetActiveScene();
    }


    public void LoadScene(string scene)
    {


        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        gameObject.SetActive(true);
        StartCoroutine(Loading(scene));
    }


    IEnumerator Loading(string scene)
    {

        fill.fillAmount = 0;

        
        infoTxtGame.text = "Chargement...";

        
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
                    title.SetActive(true);
                    if (progressValueGame > 0.2f && progressValueGame  <= 0.3f)
                    {

                        
                        yield return new WaitForSeconds(0.5f);

                    }

                    if (progressValueGame >= 0.9f)
                    {

                        isRotating = false;
                        fill.fillAmount = 1;
                        operation.allowSceneActivation = true;
                  
                    }
                    break;
                case LoadingScreenType.continueGameScreen:
                    infoTxtGame.text = "chargement...";

                    if (progressValueGame >= 0.4f && progressValueGame <= 0.5f)
                    {

                        
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
                       
                    }
                    break;

                case LoadingScreenType.MainMenu:
                    infoTxtGame.text = "chargement...";
                    if (progressValueGame >= 0.8f && progressValueGame <= 0.9f)
                    {
                        infoTxtGame.text = "Chargement du menu principal...";
                        yield return new WaitForSeconds(0.5f);

                    }
                   
                    break;

                default:
                    infoTxtGame.text = "chargement...";
                    break;
            }
            if (progressValueGame >= 0.9f)
            {
                isRotating = false;
                fill.fillAmount = 1;
                if(loadingScreenType == LoadingScreenType.newGameScreen)
                {

                    MenuAudioManager.instance.enableScreamer = false;
                    ThunderManager.instance.thunderType = ThunderType.game;
                    ThunderManager.instance.EnableGameThunder();
                    AudioM.instance.PlayDefaultAmbientBackground();
                }
                operation.allowSceneActivation = true;

            }


            yield return null;

        }
    }



    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        if (isRotating)
        {
            settingsImage.transform.Rotate(new Vector3(0, 0, 360) * Time.deltaTime * speed);
        }
    }
}
