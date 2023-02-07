using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem;
using UnityEngine;
using UnityEngine.EventSystems;
namespace BlackPearl
{

    public class Slot : MonoBehaviour,ISelectHandler, IDeselectHandler
    {
        public Item currentItem = null;
        public ItemType itemAccepted = ItemType.None;
        public Image itemIcon = null;
        public TMP_Text itemAmount = null;
        public Text txt_id = null;
        public Text CancelCraftText = null;
        private Image fill_craft = null;
        public SlotType slotType = SlotType.None;
        public bool isDraggable = true;
        public InventoryManagerInput inventoryInputs;
        InventoryManagerInput.InventoryActionActions inventoryAction;
        public Slot slotselected = null;
        // public float weight;
        // [SerializeField] private GameObject BarHorizontal = null;
        // [SerializeField] private Image fill_HorizontalBar = null;
        // [SerializeField] private GameObject BarVertical = null;
        // [SerializeField] private Image fill_ItemVerticalBar = null;
        [HideInInspector] public PanelBackPack BackPack = null;
        [HideInInspector] public RecipeCraftItem[] recipeCraftItems = null;

   


        private void Awake() {
            itemIcon = transform.Find("icon").GetComponent<Image>();

            
            

            // fill_craft = transform.Find("fill_craft").GetComponent<Image>();
            // CancelCraftText = transform.Find("X").GetComponent<Text>();
            itemAmount = transform.Find("qty").GetComponent<TMP_Text>();
            itemAmount.gameObject.SetActive(true);

            // fill_craft.fillAmount = 0;
            // CancelCraftText.enabled =false;
            BackPack = GameObject.FindGameObjectWithTag("BagPack").GetComponent<PanelBackPack>();

            inventoryInputs = new InventoryManagerInput();
            inventoryAction = inventoryInputs.InventoryAction;


            //     BarHorizontal = transform.Find("BarHorizontal").gameObject;
            //     BarVertical = transform.Find("BarVertical").gameObject;
            //     fill_HorizontalBar = BarHorizontal.transform.Find("Fill_HorizontalBar").GetComponent<Image>();
            //     fill_ItemVerticalBar = BarVertical.transform.Find("Fill_VerticalBar").GetComponent<Image>();



        }



        public void OnDeselect(BaseEventData eventData)
        {

            ToolTypeManager.instance.Hide();

        }

        public void InitFirstSlotSelect()
        {
            slotselected = this;
            currentItem = slotselected.currentItem;
            if (currentItem != null)
            {
                currentItem = GetComponent<Slot>().currentItem;
                Slot slot = GetComponent<Slot>();
                ToolTypeManager.instance.Show(currentItem.ItemName, currentItem.ItemDescription, currentItem);
                ToolTypeManager.instance.SetInput(currentItem.canUse, currentItem.isInspectable, true, currentItem.canEquiped);
                AudioM.instance.PlayHUDHoverClip();

                Inventory.instance.panelBackPack.Selection(currentItem, slotselected);


            }

        }

        public void OnSelect(BaseEventData eventData)
        {
            slotselected = this;
            currentItem = slotselected.currentItem;
            if (currentItem != null)
            {
                currentItem = GetComponent<Slot>().currentItem;
                Slot slot = GetComponent<Slot>();
                ToolTypeManager.instance.Show(currentItem.ItemName, currentItem.ItemDescription,currentItem);
                ToolTypeManager.instance.SetInput(currentItem.canUse, currentItem.isInspectable, true, currentItem.canEquiped);
                AudioM.instance.PlayHUDHoverClip();

                Inventory.instance.panelBackPack.Selection(currentItem, slotselected);
                

            }
            else
            {
                ToolTypeManager.instance.Show("", null, null);
                ToolTypeManager.instance.SetInput(false, false, false, false);
                Inventory.instance.panelBackPack.Selection(null, slotselected);
            }



          
            
        }

        public void UpdateSlot()
        {
            slotselected = this;
            currentItem = slotselected.currentItem;
            if (currentItem != null)
            {
                currentItem = GetComponent<Slot>().currentItem;
                Slot slot = GetComponent<Slot>();
                ToolTypeManager.instance.Show(currentItem.ItemName, currentItem.ItemDescription, currentItem);
                ToolTypeManager.instance.SetInput(currentItem.canUse, currentItem.isInspectable, true, currentItem.canEquiped);
                AudioM.instance.PlayHUDHoverClip();

                Inventory.instance.panelBackPack.Selection(currentItem, slotselected);


            }
            else
            {
                ToolTypeManager.instance.Show("", null, null);
                ToolTypeManager.instance.SetInput(false, false, false, false);
                Inventory.instance.panelBackPack.Selection(null, slotselected);
            }
        }

        

        #region update Slot & icon    
        
        

       


       

        

       

        

        

        


        #endregion



        #region mouse event
        //public void MouseEventHover()
        //{

        //    if(slotType == SlotType.CraftQ)
        //    {
        //        CancelCraftText.enabled = true;
        //    }
        //    if(isDraggable && Inventory.instance.isInDrag)
        //    {
        //        Inventory.instance.endSlot = this;
        //    }
 
        //    AudioM.instance.PlayHUDHoverClip();
         
        //    Inventory.instance.panel_infos.ShowItemInfos(this);
         
        //}
        

        //public void MouseEventSelect(BaseEventData data)
        //{
        //    if(currentItem == null)
        //        return;
 
        //    PointerEventData pointer = (PointerEventData)data;
        //    if(pointer.button == PointerEventData.InputButton.Left && slotType != SlotType.Required)
        //    {
        //        if(slotType == SlotType.CraftQ)
        //        {
        //            CancelCraftInQueue();
        //        }else
        //        {
                    
        //            Inventory.instance.panel_infos.ShowItemInfos(this);
        //        }
                

        //    }else if(pointer.button == PointerEventData.InputButton.Right)
        //    {
        //        if(slotType == SlotType.Craft || slotType == SlotType.CraftQ || slotType == SlotType.Required)
        //        return;
    
        //        Inventory.instance.panel_options.ShowOption(this);
        //        Inventory.instance.panel_infos.ShowItemInfos(this);

        //    }
            


        //}

        //public void ButtonInventoryEventSelect()
        //{
            
        //    if(currentItem == null)
        //        return;
                
        //    if(slotType == SlotType.Craft || slotType == SlotType.CraftQ || slotType == SlotType.Required)
        //    return;
    
        //    Inventory.instance.panel_options.ShowOption(this);

        //}

        

       








        //public void MouseEventExit()
        //{
            
        //    if(slotType != SlotType.Craft || slotType != SlotType.Required)
        //    {
        //        Inventory.instance.panel_infos.HideItemInfos();
        //    }
        //    if(slotType == SlotType.CraftQ)
        //    {
        //        CancelCraftText.enabled = false;
        //    }
            
        //    if(isDraggable)
        //        Inventory.instance.endSlot = null;
        //}
        #endregion
        
        

        public void OnBeginDrag_Event()
        {
            if(Inventory.instance.isInDrag)
                return;

            //if(Input.GetKey(KeyCode.Mouse0) && currentItem != null)
            //{
            //    if(isDraggable)
            //        Inventory.instance.StartDrag(this);
         
            //}
        }

        public void OnEndDrag_event()
        {
            if(isDraggable)
                Inventory.instance.EndDrag();
          
        }

        
    }



}