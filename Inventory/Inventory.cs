using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using SUPERCharacter;
using UnityEngine.InputSystem;
using System.Linq;
using UnityEngine;
using System;

namespace BlackPearl
{   

 

    public class Inventory : MonoBehaviour
    {
        #region variables
        public static Inventory instance = null;
        [HideInInspector] public SUPERCharacterAIO player = null;
        [HideInInspector] public HUDVitals vitals;
        public bool isInventoryOpen = false;
        private PlayerInputAction inputAction;
        [Header("reference panel")]
        public PanelBackPack panelBackPack = null;

        public Sprite emptySlot;
        
        public PanelStorage panel_storage = null;

        public bool canSelect = false;
        private float timer = 0;
 
        //public HUDObjectif panel_objectif = null;

        [SerializeField] public GameObject slotPref;


        [Header("Color Fx")]
       [SerializeField] public Color colorInterractItem;
        [SerializeField] public Color colorToolsItem;
        [SerializeField] public Color colorWeaponItem;
        [SerializeField] public Color colorConsumableItem;
        [SerializeField] public Color colorKeyItem;
        [SerializeField] public Color colorBackPackItem;

        [Header("Drag & Drop")]
        public bool isInDrag = false;
        private DragImages dragImages = null;
        private Slot startSlot = null;

        [HideInInspector] public Slot endSlot = null;

       [SerializeField] private List<ItemInInventory> inventoryDatabase = new List<ItemInInventory>();
       [SerializeField] private List<ItemInInventoryHotbar> inventoryHotbar = new List<ItemInInventoryHotbar>();





        #endregion

        private void Awake() {
            if(instance == null)
            {
                instance = this;
            }
            if(GetComponent<Canvas>().isActiveAndEnabled)
                GetComponent<Canvas>().enabled = false;

            inputAction = new PlayerInputAction();

        }

        private void Start() {
            player = FindObjectOfType<SUPERCharacterAIO>();
            vitals = FindObjectOfType<HUDVitals>();
            panelBackPack = GetComponentInChildren<PanelBackPack>();
            panelBackPack.Init();
            UiNavigationManager.instance.Init();
           
            // panel_storage = GetComponentInChildren<PanelStorage>();
            // panel_storage.Init();
            // panel_craft = GetComponentInChildren<PanelCraft>();
            // panel_craft.Init();
            dragImages = transform.Find("DragImage").GetComponent<DragImages>();

            //GameBegin();
        }



        public void Init(SUPERCharacterAIO _player)
        {


            player =_player;
            HotBar.instance.Init(player);
            UpdateInventoryDataBase();
            UpdateInventoryHotBar();
            UiNavigationManager.instance.EnableNavigationHotbar();
            
        }

        public void GameBegin()
        {

            Instantiate(inventoryHotbar[0].itemHotbar.ArmPrefab, player.fpscam.armsHolder);

            UpdateInventoryHotBar();
            AudioM.instance.PlayEquiped();
            ScreenEventsManager.instance.SetVisualMessage(true, inventoryHotbar[0].itemHotbar, 1);
        }

        private void Update() {
            if(isInDrag)
            {
               // dragImages.transform.localPosition = (Input.mousePosition) - GetComponent<Canvas>().transform.localPosition;
            }

            if (timer <= 0)
                canSelect = true;

            if (timer > 0)
            {
                canSelect = false;
                timer -= Time.deltaTime;
            }


        }

        

       

        

        #region show hide inventory
        public void OpenCloseInventory()
        {
            isInventoryOpen = !isInventoryOpen;

            
            if(AudioM.instance != null)
            {
                if (panelBackPack.isBackPackEquiped)
                {
                    AudioM.instance.PlayOneShotClip(AudioM.instance.InventoryAudioSource, AudioM.instance.openHudClip);
                }
                else
                {
                    AudioM.instance.PlayOneShotClip(AudioM.instance.InventoryAudioSource, AudioM.instance.hover_clip);
                }

            }
           
            GetComponent<Canvas>().enabled = isInventoryOpen;
            
            if (isInventoryOpen)
            {

                UiNavigationManager.instance.EnableNavigationInventory();
                HUD.instance.ChangeCrossHair(HUD.crosshair_type.None);
                InputManager.instance.EnableInputsInventoryActions();
                InputManager.instance.inputs_ui.enabled = true;

                InputManager.instance.inputs.PlayerMovement.Disable();
                player.SetController(false);


            }
            
            if (!isInventoryOpen)
            {
                InputManager.instance.DisableInputsInventoryActions();
                UiNavigationManager.instance.EnableNavigationHotbar();
           
                HUD.instance.ChangeCrossHair(HUD.crosshair_type.normal);
                HotBar.instance.SelectButton(HotBar.instance.LastButtonSelected());
                InputManager.instance.inputs.PlayerMovement.Enable();
                player.SetController(true);
                InputManager.instance.inputs_ui.enabled = false;

            }
           

        }
        #endregion
        
        

        public void AddItemToInventory(Item item,int amount)
        {
            if (item.itemCategory == Category.BackPack)
            {
                panelBackPack.isBackPackEquiped = true;
                panelBackPack.InitBackPack(item.backpackSize);
                UpdateInventoryDataBase();
                return;
            }
            ItemInInventory[] itemInInventory = inventoryDatabase.Where(elem => elem.itemData == item).ToArray();
            bool itemAdded = false;
            if(itemInInventory.Length > 0 && item.stackable)
            {
                for (int i = 0; i < itemInInventory.Length; i++)
                {
                    if(itemInInventory[i].amount < item.maxStack)
                    {
                        itemAdded = true;
                        itemInInventory[i].amount += amount;
                        break;
                    }
                }
                if (!itemAdded)
                {
                    inventoryDatabase.Add(
                        new ItemInInventory
                        {
                            itemData = item,
                            amount = amount
                        });
                }

            }
            else
            {
                inventoryDatabase.Add(
                    new ItemInInventory
                    {
                        itemData = item,
                        amount = amount
                    });
            }
            UpdateInventoryDataBase();
        }

        public void DestroyItemFromInventory(Item item,bool destroyAll,int amountTodestroy = 0)
        {
            ItemInInventory itemInInventory = inventoryDatabase.Where(elem => elem.itemData == item).FirstOrDefault();
            if(itemInInventory != null && itemInInventory.amount > 1 && !destroyAll)
            {
                itemInInventory.amount--;
            }
            else
            {
                inventoryDatabase.Remove(itemInInventory);
            }
            UpdateInventoryDataBase();
        }

        public void DestroyItemFromInventoryWithAmount(Item item, int amountTodestroy)
        {
            ItemInInventory itemInInventory = inventoryDatabase.Where(elem => elem.itemData == item).FirstOrDefault();
            if (itemInInventory != null && itemInInventory.amount >= amountTodestroy)
            {
                itemInInventory.amount -= amountTodestroy;
            }
            else
            {
                itemInInventory.amount = 0;
                
            }
            UpdateInventoryDataBase();
        }



        public void UpdateInventoryDataBase()
        {
            if (panelBackPack.grid.childCount <= 0)
                return;

            for (int i = 0; i < panelBackPack.grid.childCount; i++)
            {
                Slot currentSlot = panelBackPack.grid.GetChild(i).GetComponent<Slot>();
                currentSlot.currentItem = null;
                currentSlot.itemIcon.sprite = currentSlot.GetComponent<Image>().sprite;
                currentSlot.itemAmount.text = string.Empty;

            }

            for (int i = 0; i < inventoryDatabase.Count; i++)
            {
                Slot currentSlot = panelBackPack.grid.GetChild(i).GetComponent<Slot>();
                currentSlot.currentItem = inventoryDatabase[i].itemData;
                
                if(inventoryDatabase[i].amount > 0)
                {
                    currentSlot.itemIcon.sprite = inventoryDatabase[i].itemData.ItemIcon;
                    currentSlot.itemAmount.text = "X " + inventoryDatabase[i].amount.ToString();
                }
                else if(inventoryDatabase[i].amount <= 0)
                {
                    currentSlot.currentItem = null;
                    currentSlot.itemIcon.sprite = emptySlot;
                    currentSlot.itemAmount.text = string.Empty;
                    
                }
                

            }
       
        }

        public int GetAmountItem(Item item)
        {
            ItemInInventory itemInInventory = inventoryDatabase.Where(elem => elem.itemData == item).FirstOrDefault();
            if(itemInInventory == null)
            {
                return 0;
            }
            return itemInInventory.amount;
        }

        public int GetBateryRemain(Item item)
        {
            ItemInInventory itemInInventory = inventoryDatabase.Where(elem => elem.itemData == item).FirstOrDefault();

            return itemInInventory.itemData.maxBatery;
        }

        public Item getItemByname(string item_name)
        {
            ItemInInventory itemInInventory = inventoryDatabase.Where(elem => elem.itemData.ItemName == item_name).FirstOrDefault();
            return itemInInventory.itemData;
        }

        public Item getItemByID(int id)
        {
            ItemInInventory itemInInventory = inventoryDatabase.Where(elem => elem.itemData.ID == id).FirstOrDefault();
            return itemInInventory.itemData;
        }


        public void AddItemToInventoryHotBar(Item item)
        {
            ItemInInventoryHotbar[] itemInInventoryHotbar = inventoryHotbar.Where(elem => elem.itemHotbar == item).ToArray();
            bool itemAdded = false;
            if (itemInInventoryHotbar.Length > 0 && item.stackable)
            {
                for (int i = 0; i < itemInInventoryHotbar.Length; i++)
                {
                    if (itemInInventoryHotbar[i].amount < item.maxStack)
                    {
                        itemAdded = true;
                        itemInInventoryHotbar[i].amount ++;

                        break;
                    }
                }
                if (!itemAdded)
                {
                    inventoryHotbar.Add(
                        new ItemInInventoryHotbar
                        {
                            itemHotbar = item,
                            amount = 1
                        });
                }

            }
            else
            {
                inventoryHotbar.Add(
                    new ItemInInventoryHotbar
                    {
                        itemHotbar = item,
                        amount =1
                    });


            }

            UpdateInventoryHotBar();
            
        }


        public void MoveItemFromInventoryToInventoryHotBar(Item item)
        {
            ItemInInventoryHotbar[] itemInInventoryHotbar = inventoryHotbar.Where(elem => elem.itemHotbar == item).ToArray();
            ItemInInventory itemInInventory = inventoryDatabase.Where(elem => elem.itemData == item).FirstOrDefault();
            bool itemAdded = false;
            if (itemInInventoryHotbar.Length > 0 && item.stackable)
            {
                for (int i = 0; i < itemInInventoryHotbar.Length; i++)
                {
                    if (itemInInventoryHotbar[i].amount < item.maxStack)
                    {
                        itemAdded = true;
                        itemInInventoryHotbar[i].amount += itemInInventory.amount;
                        
                        break;
                    }
                }
                if (!itemAdded)
                {
                    inventoryHotbar.Add(
                        new ItemInInventoryHotbar
                        {
                            itemHotbar = item,
                            amount = 1
                        });
                }

            }
            else
            {
                inventoryHotbar.Add(
                    new ItemInInventoryHotbar
                    {
                        itemHotbar = item,
                        amount = itemInInventory.amount
                    });
                
                
            }
          
            DestroyItemFromInventory(itemInInventory.itemData,true);
            // instantiate arm
            if (player.fpscam.armsHolder.childCount > 0)
            {

                if (item != null)
                {


                    StartCoroutine(TransitionReplaceHoldEquipment(item));
                }
            }
            else
            {

                StartCoroutine(TransitionNewEquipment(item));
            }
            AudioM.instance.PlayEquiped();
            UpdateInventoryHotBar();
            UpdateInventoryDataBase();
        }

        public IEnumerator TransitionNewEquipment(Item newItem)
        {
            player.fpscam.AddNewArms(newItem);
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

            yield break;
        }
        public void DestroyItemFromHotBarInventory(Item item)
        {
            ItemInInventoryHotbar itemInHotbarInventory = inventoryHotbar.Where(elem => elem.itemHotbar == item).FirstOrDefault();
            if (itemInHotbarInventory != null && itemInHotbarInventory.amount > 1)
            {
                itemInHotbarInventory.amount--;
            }
            else
            {
                inventoryHotbar.Remove(itemInHotbarInventory);
            }

               UpdateInventoryHotBar();
  
        }

        public int GetAmountItemHotBarInventory(Item item)
        {
            ItemInInventoryHotbar itemInHotbarInventory = inventoryHotbar.Where(elem => elem.itemHotbar == item).FirstOrDefault();

            return itemInHotbarInventory.amount;
        }

        public void UpdateInventoryHotBar()
        {
            if (HotBar.instance.gridSlots.childCount <= 0)
                return;

            for (int i = 0; i < HotBar.instance.gridSlots.childCount; i++)
            {
                SlotHotBar currentHotBarSlot = HotBar.instance.gridSlots.GetChild(i).GetComponent<SlotHotBar>();
                currentHotBarSlot.currentItem = null;
                currentHotBarSlot.itemIcon.sprite = currentHotBarSlot.GetComponent<Image>().sprite;


            }
            
            for (int i = 0; i < inventoryHotbar.Count; i++)
            {
                SlotHotBar currentHotBarSlot = HotBar.instance.gridSlots.GetChild(i).GetComponent<SlotHotBar>();
                currentHotBarSlot.currentItem = inventoryHotbar[i].itemHotbar;
                currentHotBarSlot.itemIcon.sprite = inventoryHotbar[i].itemHotbar.ItemIcon;
              
            }

        }




        public int AmountItemInInventory(int _id)
        {
            ItemInInventory itemInInventory = inventoryDatabase.Where(elem => elem.itemData.ID == _id).FirstOrDefault();
            if (itemInInventory != null)
            {

                return itemInInventory.amount;
            }

            return 0;


        }


        public bool InventoryIsFull()
        {
            return panelBackPack.currentInventorySize == inventoryDatabase.Count;
            
        }

        public bool isBackPackEquiped()
        {
            return panelBackPack.isBackPackEquiped;
            
        }

        public bool InventoryHotBarIsFull()
        {
            return HotBar.instance.maxItemInHotBar == inventoryHotbar.Count;
        }

        public List<ItemInInventory> GetAllInventoryItem()
        {
            return inventoryDatabase;
        }

        public bool AddItemStorage(Item item,int amount)
        {
            //return AddItems(item,panel_storage.gridSlot,amount);
            return true;
        }

        
       

        #region Slots
        
        public void DestroyAllSlots(Transform grid)
        {
            if(grid == null || grid.childCount <= 0)
            return;
            for (int i = 0; i < grid.childCount; i++)
            {
                Destroy(grid.GetChild(i).gameObject);
            }
        }
        public void CreateSlots(Transform grid,int amount)
        {
            if(grid == null || amount <= 0)
            {
                return;
            }

            for (int i = 0; i < amount; i++)
            {
                Instantiate(slotPref,grid);
            }
        }

        public Slot CreateCraftSlot(Transform grid)
        {
            if(grid == null)
            {
                return null;
            }

            return Instantiate(slotPref,grid).GetComponent<Slot>();

        }

        
        #endregion

        #region Drag & Drop
        public void StartDrag(Slot slot)
        {
            if(slot == null || slot.currentItem == null)
                return;
            
            isInDrag = true;
            dragImages.Refresh(slot.currentItem);
            startSlot = slot;

            
        }

        // public void selectStartSlot(Slot slot)
        // {
        //     if(slot == null || slot.currentItem == null)
        //         return;
        //     startSlot = slot;
        //     endSlot = HotBar.instance.GetCurrentSlot();
        //     if(endSlot != null){
        //         changeItemSlot();
                
        //         if(GetComponent<Canvas>().isActiveAndEnabled)
        //             GetComponent<Canvas>().enabled = false;
                
        //         player.SetController(true);
        //         isInventoryOpen = false;
        //         HotBar.instance.Selection();
        //     }
        // }
        public void EndDrag()
        {
            if(startSlot != null)
            {
                startSlot.itemIcon.sprite = null;
            }
            isInDrag = false;
            dragImages.Refresh(null);
            if(endSlot != null){
                
                changeItemSlot();
            }
        }

        private void changeItemSlot()
        {
            if(startSlot == endSlot || startSlot.currentItem == null)
            {
                return;
            }

            ItemType startItemType = startSlot.currentItem.itemType;
            ItemType endtSlotType = endSlot.itemAccepted;

            if(endtSlotType == ItemType.None || (endtSlotType != ItemType.None && startItemType == endtSlotType))
            {
                Item itemEndSlot = endSlot.currentItem;
                if(CheckItemSlot(endSlot,startSlot.currentItem) && CheckItemSlot(startSlot,endSlot.currentItem))
                {
                    //same item
                    if(itemEndSlot != null && (itemEndSlot.ItemName == startSlot.currentItem.ItemName) && itemEndSlot.stackable)
                    {
                        //while (endSlot.currentItem.amount < endSlot.currentItem.maxStack)
                        //{
                        //    if(startSlot.currentItem.amount > 0)
                        //    {
                        //        endSlot.currentItem.amount ++;
                        //        startSlot.currentItem.amount --;
                        //    }
                        //    else 
                        //        break;
                        //}

                        //HotBar.instance.Selection();
                        startSlot = null;
                        endSlot = null;
                        
                        return;
                    }



                }
            }
     
            startSlot = null;
            
            //HotBar.instance.Selection();
            endSlot = null; 

        }

        private bool CheckItemSlot(Slot slot, Item item)
        {
            if(item == null) return true;
            if(slot.itemAccepted == ItemType.None) return true;

            return(slot.itemAccepted == item.itemType);
        }
        #endregion


        public float GetPercentage(float value, float max)
        {
            return (((value * 100) / max) / 100);
        }


    }

    [System.Serializable]
    public class ItemInInventory
    {
        public Item itemData;
        public int amount;
    }

    [System.Serializable]
    public class ItemInInventoryHotbar
    {
        public Item itemHotbar;
        public int amount;
        
    }






}