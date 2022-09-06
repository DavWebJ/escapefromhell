using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine;

using System.Linq;

namespace BlackPearl
{   
    public enum ItemType
    {
        None = 0,All = 1,Consumables = 2,Zippo = 3,Health = 4,Storage = 5,Market = 6,Resources = 7,Buildable = 8,Liquide = 9,Gun = 10,Ammo = 11,FlashLight = 12,Rifle = 13,Weapon = 14
    }
    public enum ItemRarityEnum
	{
		Common =0 , Rare = 1, Epic = 2, Legendary = 3, Mythic = 4
	}
    public enum ObjectType{None,Equipable,Interractable}
    public enum Firemode{auto,semi,none}
    public enum WeaponType{none,scoped,sight}

    public class Inventory : MonoBehaviour
    {
        #region variables
        public static Inventory instance = null;
        [HideInInspector] public FirstPersonAIO player = null;
        [HideInInspector] public HUDVitals vitals;
        public bool isInventoryOpen = false;
        private PlayerInputAction inputAction;
        [Header("reference panel")]
        [Tooltip("ici sont réferencer tous les panels de l'inventaire, les panels doivent être enfant direct de l'inventaire")]
        public PanelBackPack panelBackPack = null;
        public PanelOptions panel_options = null;
        public PanelStorage panel_storage = null;
        public HUDObjectif panel_objectif = null;
        // public PanelCraft panel_craft = null;
        // public PanelMarket panel_market = null;
        public PanelItemInfos panel_infos = null;
        [SerializeField] public GameObject slotPref;
   

        [Header("Drag & Drop")]
        public bool isInDrag = false;
        private DragImages dragImages = null;
        private Slot startSlot = null;

        [HideInInspector] public Slot endSlot = null;

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
            player = FindObjectOfType<FirstPersonAIO>();
            vitals = FindObjectOfType<HUDVitals>();
            panelBackPack = GetComponentInChildren<PanelBackPack>();
            panelBackPack.Init();
            panel_options = GetComponentInChildren<PanelOptions>();
            panel_options.Init();
            panel_objectif = GetComponent<HUDObjectif>();
            // panel_storage = GetComponentInChildren<PanelStorage>();
            // panel_storage.Init();
            // panel_craft = GetComponentInChildren<PanelCraft>();
            // panel_craft.Init();
            panel_infos = GetComponentInChildren<PanelItemInfos>();
            panel_infos.HideItemInfos();
            dragImages = transform.Find("DragImage").GetComponent<DragImages>();
        }

        public void Init(FirstPersonAIO _player)
        {

            Item lighter = GameManager.instance.resources.GetitemByName("Lighter") as WeaponItem;
           HotBar.instance.AddItemToSlot(lighter);

            player =_player;
            HotBar.instance.Init(player);
        }

        private void Update() {
            if(isInDrag)
            {
               // dragImages.transform.localPosition = (Input.mousePosition) - GetComponent<Canvas>().transform.localPosition;
            }

           
        }

        #region show hide inventory
        public void OpenCloseInventory()
        {
            isInventoryOpen = !isInventoryOpen;
            CheckFirstSlot();
            inputAction.PlayerMovement.Disable();
            AudioM.instance.PlayOneShotClip(AudioM.instance.AudiosHUD, AudioM.instance.openHudClip);

            // isBlurtheUi = !isBlurtheUi;
            GetComponent<Canvas>().enabled = isInventoryOpen;
            //player.StopSmouthMovementPlayer();
            player.SetController(!isInventoryOpen);
            GameManager.instance.SetMouseCursor(isInventoryOpen);
            panel_options.HidePanelOption(isInventoryOpen);
            panel_options.HideOption();
            

            // NaviguationSystemCanvas.instance.checkFirstSelected();

            if (!isInventoryOpen)
            {
                // if(panel_storage.gameObject.activeInHierarchy)
                // {
                //     panel_storage.HidePanel();
                // }
                // if(panel_market.gameObject.activeInHierarchy)
                // {
                //     panel_market.HidePanelMarket();
                // }
                if(panel_infos.gameObject.activeInHierarchy)
                {
                    panel_infos.HideItemInfos();
                }
                StartCoroutine(ReEnabledInputPlayer());

            }
           

        }
        #endregion
        
        public IEnumerator ReEnabledInputPlayer()
        {
            yield return new WaitForSeconds(0.5f);
          
            inputAction.PlayerMovement.Enable();
            yield break;
        }

        #region Items
        public bool AddItemBackPack(Item item)
        {

            return AddItems(item, panelBackPack.grid);
        }

        public bool AddItemStorage(Item item)
        {
            return AddItems(item,panel_storage.gridSlot);
        }
        // public bool AddItemMarket(Item item)
        // {
        //     return AddItems(item,panel_market.gridSlot);
        // }
        private bool AddItems(Item item, Transform grid)
        {
            
            if(item == null || item.amount <= 0 )
            {
                return false;
            }
            
            List<Slot> slotList = GetSlots(grid);
            
            if(slotList.Count <= 0)
            {
                return false;
            }
            
            // search first slot ref item
            Slot slotFound = slotList.FirstOrDefault(p => p.currentItem != null
            && p.currentItem.ItemName == item.ItemName
            && p.currentItem.stackable 
            && p.currentItem.amount + item.amount <= item.maxAmount);
           

            // slot is find then we can stack item
            if(slotFound != null)
            {
                if (slotFound.currentItem.itemType == ItemType.Ammo)
                {
                    slotFound.ammoAmount += item.amount;
                    slotFound.currentItem.amount += item.amount;
                   
                }
                else
                {
                    slotFound.currentItem.amount += item.amount;
                }
                

                //panelBackPack.UpdateWeight(item.amount);
                slotFound.UpdateSlot();
            }else// no item is find then we search first slot empty
            {
                slotFound = slotList.FirstOrDefault(p => p.currentItem == null);
                if(slotFound == null)
                {
                    
                    HUD.instance.SetVisualMessage("Votre inventaire est plein !",Color.red,HUD.instance.prf_Message,HUD.instance.gridMessage);
                    
                    
                    return true;
                }
                //slotFound.currentItem.Weight = item.Weight;
                //panelBackPack.UpdateWeight(item.amount);
                slotFound.ChangeItem(item);
            }

            HUD.instance.SetVisualMessage(true,item);
            return true;
        }
        #endregion
        public void CheckFirstSlot()
        {
            //if(panelBackPack.inventoryEvent != null)
            //{
            //    if(panelBackPack.inventoryEvent.firstSelectedGameObject == null)
            //    {
            //        for (int i = 0; i < panelBackPack.grid.childCount; i++)
            //        {
            //            panelBackPack.inventoryEvent.firstSelectedGameObject = panelBackPack.grid.GetChild(0).gameObject;
            //        }
            //    }
            //    else
            //    {
            //        return;
            //    }
            //}
            //else
            //{
            //    return;
            //}

        }
        #region Slots
        public List<Slot> GetSlots(Transform grid)
        {
           
            if(grid == null || grid.childCount <= 0)
                return null;
                List<Slot> slots = new List<Slot>();

                for (int i = 0; i < grid.childCount; i++)
                {
                    
                    slots.Add(grid.GetChild(i).GetComponent<Slot>());
                }
                
                return slots;

                
        }
        public int GetAmmoSlots(Transform grid)
        {

            List<Slot> slots = new List<Slot>();
            int currAmmo = 0;
            for (int i = 0; i < grid.childCount; i++)
            {
                if (slots[i].currentItem.itemType == ItemType.Ammo)
                {
                    slots[i].currentItem.amount++;
                    currAmmo++;
                }

            }
            print(currAmmo);
            return currAmmo;


        }

        public List<Slot> GetAllSlots()
        {

                List<Slot> slots = new List<Slot>();
                if(panelBackPack.grid.childCount > 0){

                    for (int i = 0; i < panelBackPack.grid.childCount; i++)
                    {
                        slots.Add(panelBackPack.grid.GetChild(i).GetComponent<Slot>());
                    }
                }
               
                return slots;
        } 

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

        public void UpdateStorageSlots(Transform grid,List<Slot> slots)
        {
            if(grid == null || slots.Count <= 0)
            {
                return;
            }

            for (int i = 0; i < slots.Count; i++)
            {
             Slot slot = Instantiate(slotPref,grid).GetComponent<Slot>();
             slot.itemAccepted = slots[i].itemAccepted;
             slot.slotType = slots[i].slotType;
             slot.ChangeItem(slots[i].currentItem);
            }
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
            startSlot.set_SelectedImage(true);
            
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
                startSlot.set_SelectedImage(false);
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

            if(endtSlotType == ItemType.All || (endtSlotType != ItemType.All && startItemType == endtSlotType))
            {
                Item itemEndSlot = endSlot.currentItem;
                if(CheckItemSlot(endSlot,startSlot.currentItem) && CheckItemSlot(startSlot,endSlot.currentItem))
                {
                    //same item
                    if(itemEndSlot != null && (itemEndSlot.ItemName == startSlot.currentItem.ItemName) && itemEndSlot.stackable)
                    {
                        while (endSlot.currentItem.amount < endSlot.currentItem.maxAmount)
                        {
                            if(startSlot.currentItem.amount > 0)
                            {
                                endSlot.currentItem.amount ++;
                                startSlot.currentItem.amount --;
                            }
                            else 
                                break;
                        }
                        startSlot.UpdateSlot();
                        endSlot.UpdateSlot();
                        //HotBar.instance.Selection();
                        startSlot = null;
                        endSlot = null;
                        
                        return;
                    }

                    endSlot.ChangeItem(startSlot.currentItem);
                    startSlot.ChangeItem(itemEndSlot);

                }
            }
     
            startSlot = null;
            
            //HotBar.instance.Selection();
            endSlot = null; 

        }

        private bool CheckItemSlot(Slot slot, Item item)
        {
            if(item == null) return true;
            if(slot.itemAccepted == ItemType.All) return true;

            return(slot.itemAccepted == item.itemType);
        }
        #endregion

        public int ReturnsAmountRequiredForCraft(string item_name)
        {
            if(item_name == "")
            return 0;

            int result = 0;

            List<Slot> slots = GetAllSlots();
            if(slots.Count > 0)
            {
                for (int i = 0; i < slots.Count; i++)
                {
                    if(slots[i].currentItem != null && slots[i].currentItem.ItemName == item_name)
                    {
                        result += slots[i].currentItem.amount;
                    }
                }
            }

            return result;
        }

        public int AmountConsumableInInventory(string item_name)
        {
            if(item_name == "")
            return 0;

            int result = 0;

            List<Slot> slots = GetAllSlots();
            if(slots.Count > 0)
            {
                for (int i = 0; i < slots.Count; i++)
                {
                    if(slots[i].currentItem != null && slots[i].currentItem.ItemName == item_name)
                    {
                        if(slots[i].currentItem.itemType == ItemType.Ammo)
                        {
                            result += slots[i].ammoAmount;
                        }
                        else
                        {
                            result += slots[i].currentItem.amount;
                        }
                        
                    }
                }
            }

            return result;
        }


        public void UpdateConsumableInInventory(string item_name,int consumable)
        {
            if (item_name == "")
                return;
           
            List<Slot> slots = GetSlots(panelBackPack.grid);

            if (slots.Count > 0)
            {
                for (int i = 0; i < slots.Count; i++)
                {
                    if (slots[i].currentItem != null && slots[i].currentItem.ItemName == item_name)
                    {
                        if (slots[i].currentItem.itemType == ItemType.Ammo)
                        {
                            slots[i].ammoAmount -= consumable;
                            slots[i].UpdateSlotAmmo();
                        }
                        else
                        {
                            slots[i].currentItem.amount -= consumable;
                            slots[i].UpdateSlot();
                        }
                            
                        
                    }
                }
            }

        }


        public float GetPercentage(float value, float max)
        {
            return (((value * 100) / max) / 100);
        }


    }




}