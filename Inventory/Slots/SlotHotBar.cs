using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using TMPro;
using BlackPearl;
using UnityEngine;

public class SlotHotBar : MonoBehaviour,ISelectHandler,IDeselectHandler
{

    public Item currentItem = null;
    public Image itemIcon = null;

 
    public SpellActionType actionType;
    public Transform grid;


    private void Awake()
    {
        itemIcon = transform.Find("icon").GetComponent<Image>();


        
        

    }



    private void Start()
    {
        grid = HotBar.instance.gridSlots;
      
        if (currentItem != null)
        {

            itemIcon.sprite = currentItem.ItemIcon;
            itemIcon.enabled = true;
        }


    }


    public void OnSelect(BaseEventData eventData)
    {

        if (currentItem == null)
        {
            return;
        }
        else
        {
            switch (currentItem.itemCategory)
            {
                case Category.None:
                    break;
                case Category.Weapon:
                    break;
                case Category.Consumable:
                    break;
                case Category.Key:
              
                    break;
                default:
                    break;
            }

        }
    }

    public void OnDeselect(BaseEventData eventData)
    {
        //HotBar.instance.currentItem = null;
        //HotBar.instance.currentSpellitem = currentSpellItem;
    }

    private void Update()
    {

    }
}
