using System.Collections;
using System.Collections.Generic;
using BlackPearl;
using UnityEngine;
[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(Outline))]

public class PickUp : MonoBehaviour
{
    private Outline[] outlines = null;
    public Outline outline = null;
    public enum ActionType { pickable, equipable, interractable }

    [SerializeField] public ActionType actionType;
    public Item item;
    public bool isOpen = false;
    [Header("Audio/FX")]
    private AudioSource audios;
    public AudioClip pickup_clip,open,close;
    

    private void Start()
    {
        outlines = GetComponents<Outline>();
        outline = GetComponent<Outline>();
        ActivateOutlines(false);
        ActivateCurrentOutlines(false);
        audios = GetComponent<AudioSource>();


        audios.volume = 0.5f;
        audios.playOnAwake = false;
    }

    #region Outline system
    public void ActivateOutlines(bool activate)
    {
       
        if (outlines.Length > 0 && outlines != null)
        {
            for (int i = 0; i < outlines.Length; i++)
            {
                outlines[i].enabled = activate;
            }
        }

    }

    public void ActivateCurrentOutlines(bool active)
    {
        if(outline != null)
        {
            outline.enabled = active;
        }
        
    }
    #endregion

    public void PickUpItem()
    {
        
        audios.PlayOneShot(item.equiped_clip);
        Inventory.instance.AddItemBackPack(item);
        Destroy(gameObject.transform.parent.gameObject,0.2f);
    }

    public void EquipItem()
    {
        WeaponItem weapon = item as WeaponItem;
        audios.PlayOneShot(item.equiped_clip);
        HotBar.instance.AddItemToSlot(weapon);
        Destroy(gameObject.transform.parent.gameObject, 0.2f);
    }

    public void InterractAction()
    {

        if (gameObject.GetComponent<Animation>() != null)
        {
            if (isOpen)
            {
                audios.PlayOneShot(close);
                gameObject.GetComponent<Animation>().Play("Close");
                isOpen = false;
            }
            else
            {
                audios.PlayOneShot(open);
                gameObject.GetComponent<Animation>().Play("open");
                isOpen = true;
            }

        }
    }

}
