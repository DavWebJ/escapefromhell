using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using BlackPearl;
using UnityEngine;

public class HealthUi : MonoBehaviour
{
    public GameObject uiGameObject;
    public Image fill;
    public bool isVisible = true;
    private Transform player;
    void Start()
    {
        

        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    public void ToggleUI()
    {
        isVisible = !isVisible;

        uiGameObject.SetActive(isVisible);
    }

    public void SetHealth(float value, float max)
    {
        float percentage = Inventory.instance.GetPercentage(value, max);
        fill.fillAmount = percentage;
        if(fill.fillAmount <= 0)
        {
            fill.fillAmount = 0;
        }
        if(fill.fillAmount >= max)
        {
            fill.fillAmount = max;
        }
    }
    void Update()
    {
        if (isVisible)
        {
            transform.LookAt(player);
        }
    }
}
