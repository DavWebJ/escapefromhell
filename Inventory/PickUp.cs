using System.Collections;
using System.Collections.Generic;
using SUPERCharacter;
using BlackPearl;
using UnityEngine;
using UnityEngine.InputSystem;

public class PickUp : MonoBehaviour
{


    public Item item = null;
    private SUPERCharacterAIO player;
    public GameObject interactableObject = null;
    private bool isInterracting = false;
    public int firstTime = 0;
    public InterractItem interract = null;
    private void Start()
    {


        player = GetComponent<SUPERCharacterAIO>();


    }


    public void OnInterract()
    {
        if (interactableObject)
        {
            ItemToPickUp InterractItem = interactableObject.GetComponent<ItemToPickUp>();
            SimpleInterract simpleInterract = interactableObject.GetComponent<SimpleInterract>();
            if (InterractItem != null)
            {
                if (isInterracting)
                {
                    return;
                }

                isInterracting = true;
                if (InterractItem.item.itemCategory == Category.Interract)
                {
                    InputManager.instance.DisableInventoryInput();
                    if(simpleInterract != null)
                    {
                        if (simpleInterract.isOpen) { DisableInterraction(); return; }
                        SimpleInterraction(simpleInterract);
                    }
                    else
                    {
                        InterractAction(InterractItem);
                    }
                    
                    InputManager.instance.EnableInputInventory();

                }
            }
        }
    }



    public void OnPickUpItem()
    {

        if (interactableObject)
        {
            ItemToPickUp groundItem = interactableObject.GetComponent<ItemToPickUp>();
            if (groundItem)
            {
                if (isInterracting)
                {
                    return;
                }

                isInterracting = true;
                if (groundItem.item.itemCategory == Category.BackPack)
                {
                    InputManager.instance.DisableInventoryInput();

                    if (groundItem.GetComponent<ItemGroundManager>() != null)
                    {
                        if (groundItem.GetComponent<ItemGroundManager>().startTimer)
                        {
                            groundItem.GetComponent<ItemGroundManager>().startTimer = false;
                        }
                    }
                    
                    Inventory.instance.AddItemToInventory(groundItem.item,0);
                    PickUpAction();
                    InputManager.instance.EnableInputInventory();
                    
                    return;
                }
                if (!Inventory.instance.isBackPackEquiped())
                {
                    if (firstTime <= 0)
                    {
                        ObjectifManager.instance.SetObjectif(ObjectifManager.instance.objectifItems[0].itemData);
                        AudioM.instance.PlayNewObjectif();
                        Inventory.instance.helperinput.SetActive(true);
                    }
                    else
                    {
                        if (ScreenEventsManager.instance.gridObjectifMessage.childCount > 0)
                        {
                            isInterracting = false;
                            return;
                        }
                            

                        ScreenEventsManager.instance.SetVisualMessage("Trouvez un sac pour stocker vos objets", ScreenEventsManager.instance.prf_inventory_message, ScreenEventsManager.instance.gridInventoryMessage);
                    }

                    firstTime = 1;
                    isInterracting = false;
                    HUDInfos.instance.ClosePickupInfos();
                    return;
                }

                if (Inventory.instance.InventoryIsFull())
                {
                    ScreenEventsManager.instance.SetVisualMessage("Votre inventaire est plein!", Color.red, ScreenEventsManager.instance.prf_Message, ScreenEventsManager.instance.gridInventoryMessage,false);
                    AudioM.instance.InventoryFull();
                    isInterracting = false;
                    HUDInfos.instance.ClosePickupInfos();
                    return;
                }


                InputManager.instance.DisableInventoryInput();
     
                if(groundItem.GetComponent<ItemGroundManager>() != null)
                {
                    if (groundItem.GetComponent<ItemGroundManager>().startTimer)
                    {
                        groundItem.GetComponent<ItemGroundManager>().startTimer = false;
                    }
                }

                Inventory.instance.AddItemToInventory(groundItem.item, groundItem.amount);
                PickUpAction();
                InputManager.instance.EnableInputInventory();

            }


        }

    }

    public void SimpleInterraction(SimpleInterract simpleinterract)
    {
       
        if(simpleinterract == null) { return; }
        if (simpleinterract.isAnimated)
        {

            interactableObject.GetComponent<Animation>().Play();
            simpleinterract.PlaySound();
            simpleinterract.isOpen = true;

        }
        HUDInfos.instance.ClosePickupInfos();
        if (interactableObject.GetComponent<Objectif>() != null)
        {
            ObjectifItem obj = interactableObject.GetComponent<Objectif>().objectif;
            ObjectifManager.instance.ValidateObjectif(obj);
        }
        if(simpleinterract.events != string.Empty)
        {
           Invoke(simpleinterract.events,1);
        }
        DisableInterraction();
    }

    public void InterractAction(ItemToPickUp _interractItem)
    {

        interract = _interractItem.item as InterractItem;
        if(interract == null) { return; }

        switch (interract.interractType)
        {
            case InterractType.Object:
                if (interract.isAnimated)
                {

                    interactableObject.GetComponent<Animation>().Play();


                }
                HUDInfos.instance.ClosePickupInfos();
                if (interactableObject.GetComponent<Objectif>() != null)
                {
                    ObjectifItem obj = interactableObject.GetComponent<Objectif>().objectif;
                    ObjectifManager.instance.ValidateObjectif(obj);
                }

                

                DisableInterraction();
                break;
            case InterractType.Door:
                if (interract.needKey && interract.isLocked)
                {
                    if (interract.isAnimated)
                    {
                        interactableObject.GetComponent<Animation>().Play();
                    }
                    AudioM.instance.PlayLockedDoor();
                    ScreenEventsManager.instance.SetVisualMessage("Cette porte est verrouiller", Color.red, ScreenEventsManager.instance.prf_objectif_message, ScreenEventsManager.instance.gridInventoryMessage, false);
                    HUDInfos.instance.ClosePickupInfos();
                    if (interactableObject.GetComponent<Objectif>() != null)
                    {
                        ObjectifItem obj = interactableObject.GetComponent<Objectif>().objectif;
                        ObjectifManager.instance.ValidateObjectif(obj);
                    }
                    DisableInterraction();

                }
                break;
            case InterractType.Drawer:
                if (interract.isAnimated)
                {
                    interactableObject.GetComponent<DrawerManager>().CurrentInterractionname = interactableObject.name;
                    interactableObject.GetComponent<DrawerManager>().PlayOpenClose();


                }
                HUDInfos.instance.ClosePickupInfos();
                if (interactableObject.GetComponent<Objectif>() != null)
                {
                    ObjectifItem obj = interactableObject.GetComponent<Objectif>().objectif;
                    ObjectifManager.instance.ValidateObjectif(obj);
                }
      
                DisableInterraction();
                break;
            case InterractType.Box:
                if (interract.isAnimated)
                {

                    interactableObject.GetComponent<Animation>().Play();

                    
                }
                HUDInfos.instance.ClosePickupInfos();
                if (interactableObject.GetComponent<Objectif>() != null)
                {
                    ObjectifItem obj = interactableObject.GetComponent<Objectif>().objectif;
                    ObjectifManager.instance.ValidateObjectif(obj);
                }

                DisableInterraction();
                break;
            default:
                break;
        }

    }

    public void OnEquipItem()
    {

        if (interactableObject)
        {
            ItemToPickUp groundItem = interactableObject.GetComponent<ItemToPickUp>();
            if (groundItem)
            {
                if (isInterracting)
                {
                    return;
                }

                isInterracting = true;
                if (groundItem.item.itemCategory == Category.BackPack)
                {
                    InputManager.instance.DisableInventoryInput();

                    if (groundItem.GetComponent<ItemGroundManager>() != null)
                    {
                        if (groundItem.GetComponent<ItemGroundManager>().startTimer)
                        {
                            groundItem.GetComponent<ItemGroundManager>().startTimer = false;
                        }
                    }

                    Inventory.instance.AddItemToInventory(groundItem.item, 0);
                    PickUpAction();
                    InputManager.instance.EnableInputInventory();

                    return;
                }
                if (!Inventory.instance.isBackPackEquiped())
                {
                    if (firstTime <= 0)
                    {
                        ObjectifManager.instance.SetObjectif(ObjectifManager.instance.objectifItems[0].itemData);
                        AudioM.instance.PlayNewObjectif();
                    }
                    else
                    {
                        if (ScreenEventsManager.instance.gridObjectifMessage.childCount > 0)
                        {
                            isInterracting = false;
                            return;
                        }


                        ScreenEventsManager.instance.SetVisualMessage("Trouvez un sac pour stocker vos objets", ScreenEventsManager.instance.prf_inventory_message, ScreenEventsManager.instance.gridInventoryMessage);
                    }

                    firstTime = 1;
                    isInterracting = false;
                    HUDInfos.instance.ClosePickupInfos();
                    return;
                }

                if (Inventory.instance.InventoryHotBarIsFull())
                {
                    ScreenEventsManager.instance.SetVisualMessage("Votre inventaire est plein!", Color.red, ScreenEventsManager.instance.prf_Message, ScreenEventsManager.instance.gridInventoryMessage, false);
                    AudioM.instance.InventoryFull();
                    isInterracting = false;
                    HUDInfos.instance.ClosePickupInfos();
                    return;
                }


                InputManager.instance.DisableInventoryInput();

                if (groundItem.GetComponent<ItemGroundManager>() != null)
                {
                    if (groundItem.GetComponent<ItemGroundManager>().startTimer)
                    {
                        groundItem.GetComponent<ItemGroundManager>().startTimer = false;
                    }
                }


                Inventory.instance.AddItemToInventoryHotBar(groundItem.item);
                EquipAction();
                InputManager.instance.EnableInputInventory();

            }


        }

    }




    public void PickUpAction()
    {
        
       
        HUDInfos.instance.ClosePickupInfos();


        AudioM.instance.PlayPickUp();
        ScreenEventsManager.instance.SetVisualMessage(true, item, interactableObject.GetComponent<ItemToPickUp>().amount);
        
        if(interactableObject.GetComponent<Objectif>() != null)
        {
            ObjectifItem obj = interactableObject.GetComponent<Objectif>().objectif;
            ObjectifManager.instance.ValidateObjectif(obj);
        }
        Destroy(interactableObject.gameObject, 0.2f);
        DisableInterraction();
        
    }


    public void EquipAction()
    {
        HUDInfos.instance.ClosePickupInfos();

        AudioM.instance.PlayEquiped();
        ScreenEventsManager.instance.SetVisualMessage(true, item, interactableObject.GetComponent<ItemToPickUp>().amount);

        if (interactableObject.GetComponent<Objectif>() != null)
        {
            ObjectifItem obj = interactableObject.GetComponent<Objectif>().objectif;
            ObjectifManager.instance.ValidateObjectif(obj);
        }

        Destroy(interactableObject.gameObject, 0.2f);
        // equip the player
        Item currentItem = item;
  
        if (player.fpscam.armsHolder.childCount > 0)
        {
          
            if (currentItem != null)
            {
                
                
                StartCoroutine(TransitionReplaceHoldEquipment(currentItem));
            }
        }
        else
        {
            
            StartCoroutine(TransitionNewEquipment(currentItem));
        }
        HotBar.instance.SelectButton(HotBar.instance.GetCurrentSlotButton(currentItem));
        isInterracting = false;
        HUD.instance.ChangeCrossHair(HUD.crosshair_type.normal);
        InputManager.instance.EnableEquipInput(false);

    }

    public IEnumerator TransitionNewEquipment(Item newItem)
    {

        player.fpscam.AddNewArms(newItem);
     
        yield return new WaitForSeconds(0.2f);
        interactableObject = null;
        item = null;
        yield break;
    }

    public IEnumerator TransitionReplaceHoldEquipment(Item newItem)
    {
        ArmsController arms = player.fpscam.armsHolder.GetChild(0).GetComponent<ArmsController>();
        arms.PlayHideAnimation();
        yield return new WaitForSeconds(arms.anim.GetCurrentAnimatorClipInfo(0).Length);
       
        player.fpscam.DestroyCurrentArms();
        yield return new WaitForSeconds(0.2f);
         player.fpscam.AddNewArms(newItem);
     
        yield return new WaitForSeconds(0.2f);
        interactableObject = null;
        item = null;
        yield break;
    }





    public void OnInterract(ItemToPickUp itemToPickup)
    {
        if (itemToPickup == null)
        {
           
            DisableInterraction();
            
            return;
        }
            


        interactableObject = itemToPickup.gameObject;
        if(interactableObject != null)
        {
            HUDInfos.instance.SceneObjectInfos(itemToPickup, itemToPickup.ActionType);
            
            if (interract == null) 
            {
                interract = itemToPickup.item as InterractItem;
            }

            item = itemToPickup.item;
            

        }
        else
        {
          
            DisableInterraction();
            return;
        }
        
        if (item != null)
        {

            switch (itemToPickup.ActionType)
            {

                case ActionType.Interract:
                    HUD.instance.ChangeCrossHair(HUD.crosshair_type.Interract);
                    InputManager.instance.EnabledInterractpinput(true);
                    InputManager.instance.DisablePickupInput();
                    
                    break;
                case ActionType.PickUp:
                    InputManager.instance.EnabledInterractpinput(false);
                    InputManager.instance.EnabledPickUpinput();
                   
                    HUD.instance.ChangeCrossHair(HUD.crosshair_type.pickup);
                    break;
                case ActionType.Equip:
                    InputManager.instance.EnabledInterractpinput(false);
                    InputManager.instance.EnabledPickUpinput();
                    InputManager.instance.EnableEquipInput(true);
                    HUD.instance.ChangeCrossHair(HUD.crosshair_type.pickup);
            
                    break;
                default:
                   
                    break;
            }
        }
        else
        {
            
            DisableInterraction();
        }

    }
   

    public void DisableInterraction()
    {
        HUDInfos.instance.ClosePickupInfos();
        HUD.instance.ChangeCrossHair(HUD.crosshair_type.normal);
        isInterracting = false;
        interactableObject = null;
        item = null;
        interract = null;
        InputManager.instance.EnableEquipInput(false);
        InputManager.instance.DisablePickupInput();
        InputManager.instance.EnabledInterractpinput(false);
        InputManager.instance.EnableInputFL();
    }


    public void SwichTheLight()
    {
        LightManager.instance.SwichOn();
    }



}
