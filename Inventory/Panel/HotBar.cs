using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using SUPERCharacter;
using UnityEngine;

namespace BlackPearl
{
    public class HotBar : MonoBehaviour
    {
        public static HotBar instance = null;
        public Transform gridSlots = null;
        public int maxItemInHotBar = 6;
        public int index = 0;
        public SUPERCharacterAIO player;
        public SlotHotBar currentSlotSelected;
        public Item currentItem;
        private float timer = 0;
        private float delaySelection = 0.2f;
        private PlayerInputAction inputs;
        PlayerInputAction.HotbarActionInputActions hotbarActions;
        public bool canSelect = false;
        public bool isInTransitionArm = false;
        public GameObject slotHotbarPref;


        private void Awake() {
            
            if(instance == null)
            {
                instance = this;
            }
            
            gridSlots = transform.Find("Grid");
            inputs = new PlayerInputAction();
            hotbarActions = inputs.HotbarActionInput;

        }

        private void Start()
        {
            currentSlotSelected = null;
            hotbarActions.UseItem.performed += CheckCurrentItem;
            hotbarActions.DropItem.performed += OnDropItem;

            if (gridSlots.childCount == maxItemInHotBar) return;

            CreateHotbarSlots();
            UiNavigationManager.instance.EnableNavigationHotbar();
            SelectButton(getFirstButton());

        }
        private void OnEnable()
        {
            hotbarActions.DropItem.Enable();
            hotbarActions.UseItem.Enable();
        }

        private void Update() {
            

            if (timer <= 0 && !isInTransitionArm)
                canSelect = true;
                
            if(timer > 0 || isInTransitionArm)
            {
                canSelect = false;
                timer -= Time.deltaTime;
            }
            
        }

        public void Init(SUPERCharacterAIO _player)
        {
            player =_player;

        }

        public void SelectNext()
        {
         
            if (!canSelect)
                return;
            if (index < gridSlots.childCount - 1)
            {
                index++;
            }
            else
            {
                index = 0;
            }
        
            
            Selection();
        }

        public void SelectPrev()
        {
            if (!canSelect)
                return;
          
            if (index == 0)
            {
                index = gridSlots.childCount - 1;
            }
            else
            {
                index--;
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
             
                if(i == index)
                {

                    SlotHotBar slot = gridSlots.GetChild(i).GetComponent<SlotHotBar>();
                    gridSlots.GetChild(i).GetComponent<Button>().Select();
                    currentSlotSelected = slot;
                    currentItem = slot.currentItem;
                    ArmSelection(slot);
                }
            }
        }

        public void SelectButton(Button btn)
        {
            
           if(btn != null)
           {
                
                currentSlotSelected = btn.GetComponent<SlotHotBar>();
                currentItem = currentSlotSelected.currentItem;

                btn.Select();
                
                int tempIndex = currentSlotSelected.transform.GetSiblingIndex();
                if(index != tempIndex)
                {
                    index = tempIndex;
                }

                    
            }
            
        }



        public void SelectButtonByHotbar(Button btn)
        {

            if (btn != null)
            {

                currentSlotSelected = btn.GetComponent<SlotHotBar>();
                currentItem = currentSlotSelected.currentItem;

                btn.Select();
                ArmSelection(currentSlotSelected);
                int tempIndex = currentSlotSelected.transform.GetSiblingIndex();
                if (index != tempIndex)
                {
                    index = tempIndex;
                }


            }

        }

        public void RespawnArm(SlotHotBar slot)
        {
            if (slot != null && currentItem != null)
            {
                player.fpscam.AddNewArms(slot.currentItem);
                SelectButton(slot.GetComponent<Button>());
            }
        }

        public void ArmSelection(SlotHotBar slot)
        {
            if (isInTransitionArm)
                return;

            if(slot != null && currentItem != null)
            {
                
                ArmsController arm = player.fpscam.armsHolder.GetChild(0).GetComponent<ArmsController>();
                if (arm != null)
                {
                    if(arm.armName == slot.currentItem.ArmPrefab.name)
                    {
                        
                        return;
                    }
                    
                    StartCoroutine(TransitionNewArm(arm,slot.currentItem));
                    SelectButton(slot.GetComponent<Button>());
                }

            }
            else
            {
               
                return;
            }
        }

        public IEnumerator TransitionNewArm(ArmsController _arm,Item newItem)
        {

            
            if(_arm != null)
            {
                isInTransitionArm = true;
                _arm.PlayHideAnimation();
                yield return new WaitForSeconds(_arm.anim.GetCurrentAnimatorClipInfo(0).Length);
                player.fpscam.DestroyCurrentArms();
                yield return new WaitForSeconds(0.1f);
                player.fpscam.AddNewArms(newItem);
                isInTransitionArm = false;
            }


            yield break;
            
        }

        public Button LastButtonSelected()
        {

            return gridSlots.GetChild(index).GetComponent<Button>();
        }

        public Button GetCurrentSlotButton(Item item)
        {
            Button current;
            for (int i = 0; i < gridSlots.childCount; i++)
            {
                if(gridSlots.GetChild(i).GetComponent<SlotHotBar>().currentItem == item)
                {
                    current = gridSlots.GetChild(i).GetComponent<Button>();
                    return current;
                }
            }

            return null;
        }


        public void CheckCurrentItem(InputAction.CallbackContext context)
        {
            
            if (currentSlotSelected == null || currentItem == null)
            {
            
                return;
            }

            if (currentItem.itemCategory == Category.Consumable)
            {
                if (currentItem.attributes.name != string.Empty)
                { 
                    

                }
            }

        }


        public SlotHotBar GetCurrentSlot()
        {
            
            for (int i = 0; i < gridSlots.childCount; i++)
            {
                if(index == gridSlots.GetChild(i).GetSiblingIndex())
                {
                   
                    return gridSlots.GetChild(i).GetComponent<SlotHotBar>();
                }
            }

            return null;
        }

        public Button getFirstButton()
        {
      
           return gridSlots.GetChild(0).GetComponent<Button>();
                
        }

        public void OnDropItem(InputAction.CallbackContext context)
        {
            if (Inventory.instance.isInventoryOpen) return;

            if (currentItem != null)
            {
                GameObject groundItem = Instantiate(currentItem.ItemGroundPrefabs);
                groundItem.GetComponent<ItemToPickUp>().amount = Inventory.instance.GetAmountItemHotBarInventory(groundItem.GetComponent<ItemToPickUp>().item);
                groundItem.transform.position = player.fpscam.targetEject.position;
                Rigidbody rb = groundItem.GetComponent<Rigidbody>();
                rb.useGravity = true;
                groundItem.GetComponent<MeshCollider>().enabled = true;
                groundItem.GetComponent<MeshCollider>().convex = true;
                groundItem.GetComponent<ItemGroundManager>().isDropped = true;
                rb.AddForce(player.fpscam.targetEject.transform.forward *
                    player.fpscam.dropForce * rb.mass, ForceMode.Impulse);
                Inventory.instance.DestroyItemFromHotBarInventory(currentItem);
                player.fpscam.DestroyCurrentArms();
                currentItem = null;
                currentSlotSelected = null;

                SelectButton(LastButtonSelected());
                return;
            }
        }


        public void CreateHotbarSlots()
        {
            if (gridSlots == null || maxItemInHotBar <= 0)
            {
                return;
            }

            for (int i = 0; i < maxItemInHotBar; i++)
            {
               GameObject slot =  Instantiate(slotHotbarPref, gridSlots);
                slot.GetComponent<SlotHotBar>().itemIcon.enabled = true;
            }
        }

    }
}