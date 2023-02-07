using System.Collections;
using System.Collections.Generic;
using SUPERCharacter;
using BlackPearl;
using UnityEngine;
[RequireComponent(typeof(ItemGroundManager))]
[RequireComponent(typeof(Outline))]
public class ItemToPickUp : MonoBehaviour
{
    public ActionType ActionType = ActionType.PickUp;
    public Item item = null;
    public int amount = 1;
    public GameObject fx = null;
    public ParticleSystem particle = null;
    public ParticleSystem.MainModule main;
    public bool hasAlreadyFx = true;
    public Outline outline = null;

    private void Start()
    {
        outline = GetComponent<Outline>();
        if (ActionType == ActionType.PickUp || ActionType == ActionType.Equip || ActionType == ActionType.Interract)
        {

            
            if (hasAlreadyFx)
            {
                fx = transform.Find("fx").gameObject;

                particle = fx.transform.GetChild(0).GetComponent<ParticleSystem>();

                main = particle.main;
            }

            
            if(item != null && hasAlreadyFx)
            {
                InitFxColor();
            }

            InitOutlineColor();
            
        }

        ActivateCurrentOutlines(false);
    }



    public void InitFxColor()
    {
        switch (item.categoryObjectType)
        {
            case CategoryObjectType.Interractable:
                main.startColor = new ParticleSystem.MinMaxGradient(Inventory.instance.colorInterractItem);

                break;
            case CategoryObjectType.Tools:
                main.startColor = new ParticleSystem.MinMaxGradient(Inventory.instance.colorToolsItem);

                break;
            case CategoryObjectType.Weapon:
                main.startColor = new ParticleSystem.MinMaxGradient(Inventory.instance.colorWeaponItem);
   
                break;
            case CategoryObjectType.Consumable:
                main.startColor = new ParticleSystem.MinMaxGradient(Inventory.instance.colorConsumableItem);
     
                break;
            case CategoryObjectType.Key:
                main.startColor = new ParticleSystem.MinMaxGradient(Inventory.instance.colorKeyItem);
       
                break;
            default:
                break;
        }
    }

    public void InitOutlineColor()
    {
        switch (item.categoryObjectType)
        {
            case CategoryObjectType.Interractable:

                outline.OutlineColor = Inventory.instance.colorInterractItem;
                break;
            case CategoryObjectType.Tools:
               
                outline.OutlineColor = Inventory.instance.colorToolsItem;
                break;
            case CategoryObjectType.Weapon:
             
                outline.OutlineColor = Inventory.instance.colorWeaponItem;
                break;
            case CategoryObjectType.Consumable:
               
                outline.OutlineColor = Inventory.instance.colorConsumableItem;
                break;
            case CategoryObjectType.Key:
        
                outline.OutlineColor = Inventory.instance.colorKeyItem;
                break;
            case CategoryObjectType.BackPack:
                outline.OutlineColor = Inventory.instance.colorBackPackItem;
                break;
            default:
                break;
        }
    }

    public void ActivateCurrentOutlines(bool active)
    {
        if (outline != null && GetComponent<MeshRenderer>() != null)
        {
            outline.enabled = active;
        }

    }


}
