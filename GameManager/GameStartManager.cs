using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using BlackPearl;
using UnityEngine;

public class GameStartManager : MonoBehaviour
{
    public static GameStartManager instance;

    public Text hour;
    public Text minutes;
    public Text separator;
    public Canvas canvas;
    public bool screenEffect = false;
    
    

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        //GameManager.instance.gameObject.SetActive(false);
    }

    private void Start()
    {
        //hour.text = "19";
        //minutes.text = "59";
        //StartCoroutine(DisplayIntroScreen());
        //GameObject.FindGameObjectWithTag("Player").GetComponent<SUPERCharacter.SUPERCharacterAIO>().SetController(false);
        canvas.enabled = false;
        GameManager.instance.IsGameStarted = true;
        GameManager.instance.startTimer = true;
        
        //GameObject.FindGameObjectWithTag("Player").GetComponent<SUPERCharacter.SUPERCharacterAIO>().SetController(true);
    }


    public IEnumerator DisplayIntroScreen()
    {
        yield return new WaitForSeconds(5);
        hour.text = "20";
        minutes.text = "00";
        yield return new WaitForSeconds(1);
        screenEffect = true;
        yield break;
    }

    private void Update()
    {
        if (screenEffect)
        {
            float min = 0.0f;
            float current = canvas.GetComponent<CanvasGroup>().alpha;
            if(current > min && canvas.isActiveAndEnabled)
            {
                canvas.GetComponent<CanvasGroup>().alpha = Mathf.Lerp(current, min, Time.deltaTime * 1.7f);
                if(current <= 0.002f && canvas.isActiveAndEnabled)
                {
                    canvas.enabled = false;
                    GameManager.instance.IsGameStarted = true;
                    GameManager.instance.startTimer = true;
                    Inventory.instance.GameBegin();
                    GameObject.FindGameObjectWithTag("Player").GetComponent<SUPERCharacter.SUPERCharacterAIO>().SetController(true);

                }

            }

            
        }
    }
}
