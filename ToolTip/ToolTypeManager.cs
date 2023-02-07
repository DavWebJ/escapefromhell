using System.Collections;
using System.Collections.Generic;
using BlackPearl;
using UnityEngine;

public class ToolTypeManager : MonoBehaviour
{
    public static ToolTypeManager instance;
    public ToolTip tooltip;

    private void Awake()
    {
        instance = this;
    }

    public void Show(string title,string itemDesc,Item item)
    {
        tooltip.SetText(title,itemDesc,item);   
        tooltip.gameObject.SetActive(true);
    }
    public void SetInput(bool activeUse,bool activeInspect,bool activeDrop,bool activeEquip)
    {
        
        tooltip.SetInput(activeUse, activeInspect, activeDrop, activeEquip);
    }

    public void Hide()
    {
        
        tooltip.gameObject.SetActive(false);
    }
}
