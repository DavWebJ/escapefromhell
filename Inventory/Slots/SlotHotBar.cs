using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using BlackPearl;
using UnityEngine;

public class SlotHotBar : MonoBehaviour
{
    
    
        
       
        public Item currentItem = null;
        public Image itemIcon = null;

    

    private void Awake()
    {
        this.itemIcon = transform.Find("icon").GetComponent<Image>();
        this.itemIcon.sprite = null;
        this.itemIcon.enabled = false;
        
    }
}
