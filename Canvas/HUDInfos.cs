using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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

    [Header("Object Scene Actions: ")]
    [SerializeField] private GameObject action_pickup = null;
    [SerializeField] private GameObject action_equip = null;
    [SerializeField] private GameObject action_interract = null;

    [SerializeField] private GameObject action_reload = null;
    
    private void Awake() {
        if(instance == null)
            instance = this;
    }
    private void Start() {
        SceneObjectInfos(null);
        SetUpActionsInputsText(action_pickup, GameManager.instance.input.input_pickup.ToString());
        SetUpActionsInputsText(action_equip, GameManager.instance.input.input_equip.ToString());
        SetUpActionsInputsText(action_interract, GameManager.instance.input.input_actionPrimary.ToString());
        ReloadInput(false);
    }

    public void SceneObjectInfos(PickUp interract)
    {
        SceneObjectInputs(interract);
        if(interract == null || interract.item == null || interract.item.objectType == ObjectType.Interractable)
        {

            if(sceneObjectInfos.activeSelf)
            sceneObjectInfosText.text = string.Empty;
            sceneObjectInfos.SetActive(false);
        }
        else
        {
            string quality = string.Empty;
            if(interract.item.attributes.name == "Liquide")
            {
                quality = " (" + interract.item.attributes.GetPercentage() * 100 + " % - état = " + interract.item.attributes.quality + " )";
            }
            sceneObjectInfos.SetActive(true);
            sceneObjectNameText.text = interract.item.ItemName.ToUpper();
            sceneObjectInfosIcon.sprite = interract.item.ItemIcon;
            sceneObjectInfosText.text = (interract.item.amount > 1) ? "x" + interract.item.amount + " " + interract.item.ItemDescription : interract.item.ItemDescription;
        }

    }

    private void SceneObjectInputs(PickUp interract)
    {
        action_pickup.SetActive(interract != null && interract.actionType == PickUp.ActionType.pickable);
        action_equip.SetActive(interract != null && interract.actionType == PickUp.ActionType.equipable);
        action_interract.SetActive(interract != null && interract.actionType == PickUp.ActionType.interractable);

    }


    public void ReloadInput(bool activate)
    {
        action_reload.SetActive(activate);
    }

    private void SetUpActionsInputsText(GameObject action, string text_actions)
    {
        // action.transform.Find("TextInputtInfos").GetComponent<Text>().text = text_actions;
    }

    void Update()
    {
        
    }
}
