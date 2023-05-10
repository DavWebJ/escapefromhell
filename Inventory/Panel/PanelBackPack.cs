using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using SUPERCharacter;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using UnityEngine;

namespace BlackPearl
{
    

    public class PanelBackPack : MonoBehaviour
    {

        [HideInInspector] public Transform grid = null;
       // public int MaxAmount = 10;
        public int currentInventorySize = 0;
        public int startInventorySize = 10;
        public int maxInventorySize = 30;
        public bool upgradeBackPack;
        public bool isBackPackEquiped = false;
        public bool isInventoryEmpty = true;
       // public int currentAmount;
       // public Text currAmountText;
        public GridLayoutGroup gridLayoutGroup;
        public Slot currentSlotSelected;
        public Item currentItem;
        private SUPERCharacterAIO player;
        public bool inInterract = false;
        [SerializeField] public GameObject inventoryEmtyText;

        private void Awake()
        {
   
           
            grid = transform.Find("Grid");

        }
        private void Start()
        {
            currentSlotSelected = null;
            player = Inventory.instance.player;
        }

        public void Init()
        {
            isBackPackEquiped = false;
            InitBackPack(10);

            

            
        }

        public void InitBackPack(int places)
        {
            if (!isBackPackEquiped)
                return;

            for (int i = 0; i < places; i++)
            {
                Instantiate(Inventory.instance.slotPref,grid);
            }

            currentInventorySize = places;
        }

        public void Selection(Item _currentItem, Slot _currentSlot)
        {

          
            if (grid.childCount <= 0)
                return;
            for (int i = 0; i < grid.childCount; i++)
            {
                    currentSlotSelected = _currentSlot;
                    currentItem = _currentItem;

            }

            //currentSlotSelected.GetComponent<Button>().Select();

        }

        public void OnEquipItem(InputAction.CallbackContext context)
        {
            if (!Inventory.instance.isInventoryOpen || inInterract) return;

            if(currentItem != null)
            {
                inInterract = true;
                Inventory.instance.MoveItemFromInventoryToInventoryHotBar(currentItem);
                ToolTypeManager.instance.Hide();
                Inventory.instance.OpenCloseInventory();
                HotBar.instance.SelectButton(HotBar.instance.GetCurrentSlotButton(currentItem));
                currentItem = null;
                currentSlotSelected = null;
                inInterract = false;
            }
            

        }

        public void OnUseItem(InputAction.CallbackContext context)
        {
            if (!Inventory.instance.isInventoryOpen || Inventory.instance.GetAmountItem(currentItem) <= 0 || inInterract) return;

            if(currentItem != null)
            {

                if(currentItem.itemCategory == Category.Consumable && currentItem.canUse)
                {

                     StartCoroutine(InterractActionUse());

                }
            }
           
        }

        public IEnumerator InterractActionUse()
        {
            inInterract = true;
            if (currentItem.canEat)
            {
                if(!player.playerVitals.CheckIfStatsIsFull(player.playerVitals.hunger, player.playerVitals.hungerMax))
                {
                    AudioM.instance.PlayOneShotClip(AudioM.instance.FxAudioSource, AudioM.instance.eat);
                    player.playerVitals.AddHunger(currentItem.hungerAdd);
                    Inventory.instance.DestroyItemFromInventory(currentItem, false);
                    currentSlotSelected.UpdateSlot();
                    yield return new WaitForSeconds(0.5f);
                    inInterract = false;
                    yield break;
                }
                else
                {
                    player.playerVitals.ShowStatsFullMessage(false, true, false);
                    inInterract = false;
                    yield break;
                }

            }


            if(currentItem.canDrink)
            {
                if(!player.playerVitals.CheckIfStatsIsFull(player.playerVitals.thirst, player.playerVitals.thirstMax))
                {
                    AudioM.instance.PlayOneShotClip(AudioM.instance.FxAudioSource, AudioM.instance.drink);
                    player.playerVitals.AddThirsty(currentItem.thirstyAdd);
                    Inventory.instance.DestroyItemFromInventory(currentItem, false);
                    currentSlotSelected.UpdateSlot();
                    yield return new WaitForSeconds(0.5f);
                    inInterract = false;
                    yield break;
                }
                else
                {
                    player.playerVitals.ShowStatsFullMessage(false, false, true);
                    inInterract = false;
                    yield break;
                }

            }


            if (currentItem.canHeal)
            {
                if(!player.playerVitals.CheckIfStatsIsFull(player.playerVitals.health, player.playerVitals.healthMax))
                {
                    if (currentItem.attributes.name == "seringue")
                    {
       
                        Inventory.instance.OpenCloseInventory();
            
                        SlotHotBar tempslot = HotBar.instance.GetCurrentSlot();
             
                        player.fpscam.DestroyCurrentArms();
           
                        GameObject go = Instantiate(player.playerVitals.seringueArm, player.fpscam.armsHolder);
                        yield return new WaitForSeconds(2.5f);
                        HotBar.instance.RespawnArm(tempslot);
                    }

                    if (currentItem.attributes.name == "paracetamol")
                    {
                        AudioM.instance.PlayOneShotClip(AudioM.instance.FxAudioSource, AudioM.instance.pills);
                    }
                    player.playerVitals.AddHealth(currentItem.healthAdd);
                    Inventory.instance.DestroyItemFromInventory(currentItem, false);
                    currentSlotSelected.UpdateSlot();
                    yield return new WaitForSeconds(0.5f);
                    inInterract = false;
                    yield break;
                }
                else
                {
                    player.playerVitals.ShowStatsFullMessage(true, false, false);
                    inInterract = false;
                    yield break;
                }

            }
            yield break;
        }
        public void OnDropItem(InputAction.CallbackContext context)
        {
            if (!Inventory.instance.isInventoryOpen) return;

            if (currentItem != null)
            {
                GameObject groundItem = Instantiate(currentItem.ItemGroundPrefabs);
                groundItem.GetComponent<ItemToPickUp>().amount = Inventory.instance.GetAmountItem(groundItem.GetComponent<ItemToPickUp>().item);
                groundItem.transform.position = player.fpscam.targetEject.position;
                Rigidbody rb = groundItem.GetComponent<Rigidbody>();
                rb.useGravity = true;
                groundItem.GetComponent<MeshCollider>().enabled = true;
                groundItem.GetComponent<MeshCollider>().convex = true;
                groundItem.GetComponent<ItemGroundManager>().isDropped = true;
                if (groundItem.GetComponent<ItemToPickUp>().item.isPerimable)
                {
                    groundItem.GetComponent<ItemGroundManager>().timer = groundItem.GetComponent<ItemToPickUp>().item.timeToDestroy;
                    if(groundItem.GetComponent<ItemToPickUp>().item.timeToDestroy > 0)
                    {
                        groundItem.GetComponent<ItemGroundManager>().startTimer = true;
                    }
                    
                }
                Inventory.instance.OpenCloseInventory();
                rb.AddForce(player.fpscam.targetEject.transform.forward *
                    player.fpscam.dropForce * rb.mass, ForceMode.Impulse);
                Inventory.instance.DestroyItemFromInventory(currentItem, true);
                Inventory.instance.UpdateInventoryDataBase();
                currentItem = null;
                currentSlotSelected = null;
                ToolTypeManager.instance.Hide();

                //Button currBtn = currentSlotSelected.GetComponent<Button>();
                //Button next = currBtn.FindSelectable(currBtn.transform.position).GetComponent<Button>();
                //next.Select();
                SelectNextButton();
                return;
            }
        }

        public void OnInspectItem(InputAction.CallbackContext context)
        {
            if (!Inventory.instance.isInventoryOpen) return;
           
        }

        public void SelectNextButton()
        {
            if(currentSlotSelected == null)
            {
                for (int i = 0; i < grid.childCount; i++)
                {
                    Slot slot = grid.GetChild(i).GetComponent<Slot>();
                    if(slot.currentItem != null)
                    {
                        Button btn = slot.GetComponent<Button>();
                        btn.Select();
                    }
                }
            }
        }




        private void Update() {


            inventoryEmtyText.SetActive(grid.childCount <= 0);
        }

        public void UpgradedBackPack(int newSize)
        {
            currentInventorySize += newSize;
            
        }


        public void DownGradeBackPack(int newSize)
        {
            currentInventorySize -= newSize;

        }
    }


}
