using System.Collections;
using System.Collections.Generic;
using TMPro;
using BlackPearl;
using UnityEngine.UI;
using UnityEngine;

public class ToolTip : MonoBehaviour
{


    public Text title;
    public Text desc;
    public Text cat;
    public GameObject boxInput;
    public GameObject useInput;
    public GameObject inspecInput;
    public GameObject dropInput;
    public GameObject equipInput;
    void Start()
    {
        //boxInput.SetActive(false);
    }
    public void SetText(string itemName,string itemDesc,Item item)
    {
        if(itemName != null)
        {
            title.text = itemName;
        }
        if(item != null)
        {
            cat.text = item.itemCategory.ToString();
            switch (item.categoryObjectType)
            {
                case CategoryObjectType.Interractable:

                    cat.color = Inventory.instance.colorInterractItem;
                    break;
                case CategoryObjectType.Tools:

                    cat.color = Inventory.instance.colorToolsItem;
                    break;
                case CategoryObjectType.Weapon:

                    cat.color = Inventory.instance.colorWeaponItem;
                    break;
                case CategoryObjectType.Consumable:

                    cat.color = Inventory.instance.colorConsumableItem;
                    break;
                case CategoryObjectType.Key:

                    cat.color = Inventory.instance.colorKeyItem;
                    break;
                default:
                    break;
            }
        }
        else
        {
            cat.text = "None";
        }
        if(desc != null)
        {
            desc.text = itemDesc;
        }
        
    }

    public void SetInput(bool activeUse,bool activeInspect,bool activeDrop,bool activeEquip)
    {
        if (!boxInput.activeInHierarchy)
        {
            boxInput.SetActive(true);
        }

        useInput.SetActive(activeUse);
        inspecInput.SetActive(activeInspect);
        dropInput.SetActive(activeDrop);
        equipInput.SetActive(activeEquip);
    }


}
