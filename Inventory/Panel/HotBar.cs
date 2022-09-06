using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine;

namespace BlackPearl
{
    public class HotBar : MonoBehaviour
    {
        public static HotBar instance = null;
        public Transform gridSlots = null;
        public int currSlot = 0;
        FirstPersonAIO player = null;
        private int NumberSlot = 6;
        private float timer = 0;
        private float delaySelection = 0.2f;
  
        public bool canSelect = false;
        
        private void Awake() {
            
            if(instance == null)
            {
                instance = this;
            }
            
            gridSlots = transform.Find("Grid");


        }

        private void Start()
        {

        }


        private void Update() {
            
            if(!GameManager.instance.CheckHUD)
            {
                if (timer <= 0)
                    canSelect = true;
                
                if(timer > 0)
                {
                    canSelect = false;
                    timer -= Time.deltaTime;
                }
            }
        }

        public void Init(FirstPersonAIO _player)
        {
            player =_player;

        }

        public void SelectNext()
        {
         
            if (!canSelect)
                return;
            if (currSlot < gridSlots.childCount - 1)
            {
                currSlot++;
            }
            else
            {
                currSlot = 0;
            }
        
            
            Selection();
        }

        public void SelectPrev()
        {
            if (!canSelect)
                return;
          
            if (currSlot == 0)
            {
                currSlot = gridSlots.childCount - 1;
            }
            else
            {
                currSlot--;
            }
        
            Selection();
        }






        public void Selection()
        {
        
            timer = delaySelection;
            if(gridSlots.childCount <= 0)
            return;
            for (int i = 0; i < gridSlots.childCount; i++)
            {
             
                if(i == currSlot)
                {
                    SlotHotBar slot = gridSlots.GetChild(i).GetComponent<SlotHotBar>();
                    gridSlots.GetChild(i).GetComponent<Button>().Select();
                    
                    if (slot.currentItem != null)
                    {
                        OnSelectSlot(slot);
                    }
                    else
                    {
                        //HUDWeapon.instance.gameObject.SetActive(false);
                        SlotHotBar newslot = GetCurrentSlot();
                        if(newslot.currentItem == null)
                        {
                           
                            
                            HUDWeapon.instance.GetWeaponInfos(null);
                            HUDWeapon.instance.isFlashlight = false;
                            HUDWeapon.instance.ShowReload(false, "");
                            HUDInfos.instance.ReloadInput(false);
                            

                            DestroyWeaponHands();
                        }
  
                    }
                   
   
                }
            }
        }

        public void OnSelectSlot(SlotHotBar slot)
        {
            
            if (slot.currentItem == null || slot == null || slot.currentItem.WeaponArmPrefab == null)
                return;

            if (slot.currentItem.objectType == ObjectType.Equipable)
            {
            
                Transform holder = Inventory.instance.player.fpscam.armsHolder;
                
                
                StopCoroutine(PlayAnimationWithDelay(holder, slot.currentItem));
                if (holder.childCount > 0)
                {



           
                        StartCoroutine(PlayAnimationWithDelay(holder,slot.currentItem));
            
                }
                else
                {
                    
                    GameObject weapon = Instantiate(slot.currentItem.WeaponArmPrefab, Inventory.instance.player.fpscam.armsHolder);
                    if(weapon.GetComponent<WeaponController>() != null)
                    {
                       
                        weapon.GetComponent<WeaponController>().SetItem(slot.currentItem);
                        HUDWeapon.instance.gameObject.SetActive(true);
                        HUDWeapon.instance.GetWeaponInfos(slot.currentItem);
                    }

                    if (weapon.GetComponent<FlashLightController>() != null)
                    {
                       
                        weapon.GetComponent<FlashLightController>().SetItem(slot.currentItem);
                        HUDWeapon.instance.gameObject.SetActive(true);
                        HUDWeapon.instance.GetWeaponInfos(slot.currentItem);
                    }

                    if (weapon.GetComponent<LighterController>() != null)
                    {

                        weapon.GetComponent<LighterController>().SetItem(slot.currentItem);
                        HUDWeapon.instance.GetWeaponInfos(null);
                    }

                }

                
            }


        }




        public IEnumerator PlayAnimationWithDelay(Transform arms,Item item)
        {
            
            arms.GetChild(0).GetComponent<Animator>().SetTrigger("hide");
            yield return new WaitForSeconds(0.3f);
            Destroy(arms.GetChild(0).gameObject);
            yield return new WaitForSeconds(0.3f);

            GameObject weapon = Instantiate(item.WeaponArmPrefab, Inventory.instance.player.fpscam.armsHolder);
            if (weapon.GetComponent<FlashLightController>() != null)
            {

                weapon.GetComponent<FlashLightController>().SetItem(item);
                HUDWeapon.instance.GetWeaponInfos(item);
            }
            if (weapon.GetComponent<WeaponController>() != null)
            {

                weapon.GetComponent<WeaponController>().SetItem(item);
                HUDWeapon.instance.GetWeaponInfos(item);
            }
            if (weapon.GetComponent<LighterController>() != null)
            {

                weapon.GetComponent<LighterController>().SetItem(item);
                HUDWeapon.instance.GetWeaponInfos(null);
            }

            
            yield break;

        }


        


        public void DestroyWeaponHands()
        {
            Transform holder = player.fpscam.armsHolder;
            if(holder.childCount > 0)
            {
                
                for (int i = 0; i < holder.childCount; i++)
                {
                    
                    holder.GetChild(i).GetComponent<Animator>().SetTrigger("hide");
                    Destroy(holder.GetChild(i).gameObject,0.3f);
                     
                }
                
            }
           
           
            HUDWeapon.instance.GetWeaponInfos(null);
       
        }

       

        public bool AddItemToSlot(Item item)
        {
            if(item == null)
                return false;

            SlotHotBar slotSelected = GetCurrentSlot();

    
            if (slotSelected == null)
                return false;
            
            if (slotSelected.currentItem != null)
            {

                slotSelected = GetNextSlot();

            }




            ChangeItem(item,slotSelected);
            Selection();
            HUD.instance.SetVisualMessage(true, item);
            return true;
        }

        public SlotHotBar GetCurrentSlot()
        {
            
            for (int i = 0; i < gridSlots.childCount; i++)
            {
                if(currSlot == gridSlots.GetChild(i).GetSiblingIndex())
                {
                   
                    return gridSlots.GetChild(i).GetComponent<SlotHotBar>();
                }
            }

            return null;
        }

        public SlotHotBar GetNextSlot()
        {
        
            for (int i = 0; i < gridSlots.childCount; i++)
            {
                if (currSlot == gridSlots.GetChild(i).GetSiblingIndex())
                {
                    currSlot += 1;
                    
                    return gridSlots.GetChild(i +1).GetComponent<SlotHotBar>();
                }
            }

            return null;
        }

        public bool SetItemToSlot(Item item)
        {
           
            if(item == null || item.amount <= 0)
            {
                return false;
            }
            
            for (int i = 0; i < gridSlots.childCount; i++)
            {
                if(i == currSlot)
                {
                    
                    SlotHotBar slot = gridSlots.GetChild(i).GetComponent<SlotHotBar>();
                    if(slot.currentItem != null)
                    {
                        print("busy");
                    }
                    else
                    {
                        print("no busy");
                    }
                    ChangeItem(item,slot);
                    Selection();
                    return true;
                }
                else
                {
                    print(".....");
                }
            }
            return false;
        }

        public void ChangeItem(Item item,SlotHotBar slot)
        {
        
            slot.currentItem = item;
            slot.itemIcon.sprite = item.ItemIcon;
            slot.itemIcon.enabled = true;
            
        }

    }
}