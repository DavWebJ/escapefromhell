using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using BlackPearl;
using SUPERCharacter;
using UnityEngine.UI;
using UnityEngine.Rendering.Universal;
using System.Linq;

[RequireComponent(typeof(M_Resources))]

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;
    public Transform playerStart;
    public SUPERCharacterAIO player;
    [SerializeField] public Text timer;
    [HideInInspector] public M_Resources resources = null;
    [HideInInspector] public AudioM audioManager = null;
    public bool IsGameStarted = false;
    private string gameScene = "Game";
    private string menuScene = "Menu";
    public bool startTimer = false;
    public bool enabledmouse = false;
    public bool enabledGamepad = false;
    public bool enabledKeyboard = false;
    public bool isDebug = false;
    public float initialTime = 72000;
    public float timeToEnd = 14400;
    public bool GameOver = false;
    public float multiplyTime;

    public bool CheckHUD
    {
        get{ 
            if(Inventory.instance.isInventoryOpen)
            {
                return true;
            }else{
                return false;
            }
        }
    }
    void Awake()
    {
        
        if (instance != null && instance != this)
        {

            Destroy(this.gameObject);
        }else
        {
            
            instance = this;


            DontDestroyOnLoad(this);

        }

        
        resources = GetComponent<M_Resources>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<SUPERCharacterAIO>();
        player.transform.position = playerStart.position;
        player.transform.rotation = Quaternion.identity;
        audioManager = GetComponent<AudioM>();
        resources.InitItems();
        
    }
    private void Start() {

        // SetGamePad(options.enabledPad);

        if(timer != null)
        {
            if (IsGameStarted && !GameOver)
            {
                startTimer = true;
            }
        }

        initialTime = 72000;
        timeToEnd = 86400;

    }


    public void ShowTimer(float timeToDisplay)
    {
        if(initialTime >= timeToEnd)
        {
            initialTime = timeToEnd;
            GameOver = true;
            startTimer = false;
        }
        int seconds = (int)(timeToDisplay % 60);
        int minutes = (int)(timeToDisplay / 60) % 60;
        
        int totalHourbeforeEnd = (int)(timeToEnd / 3600) % 24;
        
        int hour = (int)(timeToDisplay / 3600) % 24;

        timer.text = string.Format("{0:00}:{1:00}", hour,minutes);
    }



    






    public void SetGamePad(bool activate)
    {
        enabledGamepad = activate;
        enabledKeyboard = false;

    }

    public void SetKeyBoard(bool activate)
    {
        enabledKeyboard = activate;
        enabledGamepad = false;
    }

    private void Update() {
        if (startTimer)
        {
            if (initialTime < timeToEnd && !GameOver)
            {
                initialTime += Time.deltaTime;

            }
            else
            {
                GameOver = true;
            }
            ShowTimer(initialTime);
        }
    }
  
}
