using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using BlackPearl;
public class HUDInfos : MonoBehaviour
{
    public static HUDInfos instance = null;
    [Header("Object Scene Infos: ")]
    [SerializeField] private GameObject sceneObjectInfos = null;
    [SerializeField] private Text sceneObjectNameText = null;
    [SerializeField] private Image sceneObjectInfosIcon = null;
    [SerializeField] private Text sceneObjectInfosText = null;
    public Text timer;
    [Header("Object Scene Actions: ")]
    [SerializeField] private GameObject action_pickup = null;
    [SerializeField] private GameObject action_interract = null;
    [SerializeField] private GameObject action_equip = null;
    private int endTime = 0;
    private ItemToPickUp _item;
    private void Awake() {
        if(instance == null)
            instance = this;
    }
    private void Start() {
        SceneObjectInfos(null,ActionType.None);
        SetUpActionsInputsText(action_pickup, "Pickup (y)");
        SetUpActionsInputsText(action_interract, "Open (y)");
        SetUpActionsInputsText(action_equip, "Equip (y)");
        timer.text = string.Empty;
    }

    public void SceneObjectInfos(ItemToPickUp item, ActionType action)
    {
        
        if(item == null)
        {

            if (sceneObjectInfos.activeSelf)
            {
                sceneObjectInfosText.text = string.Empty;
                timer.text = string.Empty;

                sceneObjectInfos.SetActive(false);
                
            }
            _item = null;
        }
        else
        {
            string quality = string.Empty;

            sceneObjectInfos.SetActive(true);
            sceneObjectNameText.text = item.item.ItemName;
            sceneObjectInfosIcon.sprite = item.item.ItemIcon;
            sceneObjectInfosText.text =  item.item.ItemDescription;
            _item = item;
            if(item.GetComponent<ItemGroundManager>() != null)
            {
                if (!item.GetComponent<ItemGroundManager>().startTimer)
                {
                    timer.text = string.Empty;
                }
            }

            
        }
        SceneObjectInputs(action);

    }

    private void SceneObjectInputs(ActionType interract)
    {
        if (gameObject.activeInHierarchy)
        {
            action_pickup.SetActive(interract == ActionType.PickUp || interract == ActionType.Equip);
            action_equip.SetActive(interract == ActionType.Equip);
            action_interract.SetActive(interract == ActionType.Interract);

            InputManager.instance.EnableEquipInput(action_equip.activeInHierarchy);
            
        }


    }

    public void ClosePickupInfos()
    {
        SceneObjectInfos(null, ActionType.None);
        sceneObjectInfos.SetActive(false);
    }

    


    private void SetUpActionsInputsText(GameObject action, string text_actions)
    {
        // action.transform.Find("TextInputtInfos").GetComponent<Text>().text = text_actions;
    }

    void Update()
    {
        if (sceneObjectInfos.activeInHierarchy && _item.item.isPerimable && _item.GetComponent<ItemGroundManager>().startTimer)
        {

            int remainTime = (int)_item.gameObject.GetComponent<ItemGroundManager>().timer;
            timer.text = "Time before destroy: " + remainTime.ToString() + " seconds";
        }
    }
}
