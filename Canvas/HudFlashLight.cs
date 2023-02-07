using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using BlackPearl;
using UnityEngine;

public class HudFlashLight : MonoBehaviour
{

    public static HudFlashLight instance;
    public Transform grid;
    public int amountBateryIninventory = 0;
    public int maxBatery = 100;
    public int currentBatery;
    public GameObject percentageHolder;
    public Text percentageText;
    public bool isLightOn = false;
    public bool flashlightEquiped = false;
    public GameObject flashlightInputreload;
    public GameObject switchLightInput;
    public Image[] icon;
    public int index = 0;
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }
    void Start()
    {
        grid = transform;
        InitRendererBatery();
        flashlightInputreload.SetActive(false);
        switchLightInput.SetActive(false);
    }


    public void InitRendererBatery()
    {
        for (int i = 0; i < icon.Length; i++)
        {
            icon[i].gameObject.SetActive(false);
        }
        percentageText.gameObject.SetActive(false);
        maxBatery = 100;
        currentBatery = maxBatery;
        gameObject.SetActive(false);
    }

    public int CheckRemainingBateryInInventory()
    {
        return Inventory.instance.AmountItemInInventory(5);
    }

    public void ShowInputReload()
    {
        flashlightInputreload.SetActive(true);
        switchLightInput.SetActive(false);
    }

    public void HideInputReload()
    {
        flashlightInputreload.SetActive(false);
        switchLightInput.SetActive(true);
    }
    public float GetBateryPercentage()
    {

        return ((float)currentBatery / maxBatery) * 100;
    }

    public void UpdateIcon()
    {
        switch (currentBatery)
        {
            case 100:
                index = 20;
            break;
            case 95:
                index = 19;
                break;
            case 90:
                index = 18;
                break;
            case 85:
                index = 17;
                break;
            case 80:
                index = 16;
                break;
            case 75:
                index = 15;
                break;
            case 70:
                index = 14;
                break;
            case 65:
                index = 13;
                break;
            case 60:
                index = 12;
                break;
            case 55:
                index = 11;
                break;
            case 50:
                index = 10;
                break;
            case 45:
                index = 9;
                break;
            case 40:
                index = 8;
                break;
            case 35:
                index = 7;
                break;
            case 30:
                index = 6;
                break;
            case 25:
                index = 5;
                break;
            case 20:
                index = 4;
                break;
            case 15:
                index = 3;
                break;
            case 10:
                index = 2;
                break;
            case 5:
                index = 1;
                break;
            case 0:
                index = 0;
                break;
            default:
                break;
        }

        float val = (float)currentBatery / (float)maxBatery;
        for (int i = 0; i < icon.Length; i++)
        {
            icon[i].gameObject.SetActive(false);
            icon[index].gameObject.SetActive(true);
            icon[index].color = HUD.instance.bateryBarColor.Evaluate(val);
            percentageText.color = Color.white;
            
        }
        
    }

    public void ShowHud()
    {
        gameObject.SetActive(true);
        percentageHolder.SetActive(true);
    }

    public void HideHud()
    {
        percentageHolder.SetActive(false);
        for (int i = 0; i < icon.Length; i++)
        {
            icon[i].gameObject.SetActive(false);
        }
        flashlightInputreload.SetActive(false);
        switchLightInput.SetActive(false);
        gameObject.SetActive(false);

    }


    public void SwitchHudLight()
    {

        isLightOn = !isLightOn;
  
    }

    void Update()
    {
        
        if (currentBatery <= 0)
        {
            index = 0;

        }

        if (flashlightEquiped)
        {
            UpdateIcon();
            percentageText.text = GetBateryPercentage() + "%";
        }

    }
}
