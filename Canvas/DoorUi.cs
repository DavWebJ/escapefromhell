using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class DoorUi : MonoBehaviour
{
    public static DoorUi instance;
    public GameObject ui;

    public Text input;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }
    void Start()
    {
        ui = transform.GetChild(0).gameObject;

        ui.SetActive(false);
    }

    public void ShowUi(bool open)
    {
        if (!ui.activeInHierarchy)
        {
            ui.SetActive(true);
            if (open)
            {
                input.text = "Open Door";
            }
            else
            {
                input.text = "Close Door";
            }
        }
    }

    public void HideUi()
    {
        ui.SetActive(false);
    }

 
    void Update()
    {
        
    }
}
