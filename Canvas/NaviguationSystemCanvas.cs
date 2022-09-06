using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;
using BlackPearl;
public class  NaviguationSystemCanvas : MonoBehaviour {

    [Header("inventory canvas events systeme")]
   public GameObject firstInventorySlots,lastInventorySlots;
    public static NaviguationSystemCanvas instance = null;
    public EventSystem inventoryEvent;


    private void Awake() {
        if(instance == null)
        {
            instance = this;
        }
    }
   
  


    private void OnDisable() {
        // EventSystem.current.SetSelectedGameObject(lastInventorySlots == null ? firstInventorySlots : lastInventorySlots);
    }

    public void checkFirstSelected()
    {
        inventoryEvent.SetSelectedGameObject(firstInventorySlots);
 
    }

    // public void checkLastSelected()
    // {
    //     Slot slot;

    //     slot = Inventory.instance.panel_options.slotSelected;
    //    lastInventorySlots = slot.gameObject;
    //    EventSystem.current.SetSelectedGameObject(lastInventorySlots);
    //     Inventory.instance.panel_options.ShowOption(slot);
    // }


}

