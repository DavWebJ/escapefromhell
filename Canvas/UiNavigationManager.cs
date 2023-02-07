using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using BlackPearl;
using UnityEngine;

public class UiNavigationManager : MonoBehaviour
{
    public Transform gridInventory;

    public Transform gridHotbar;
 
    public static UiNavigationManager instance;
    public List<Button> btn = new List<Button>();
    private void Awake()
    {

            instance = this;
        
    }

    public void Init()
    {
        gridInventory = Inventory.instance.panelBackPack.grid;

        gridHotbar = HotBar.instance.gridSlots;
    

        
    }

    public void EnableNavigationInventory()
    {
        DisableButtons(gridHotbar);
        InputManager.instance.DisbaleInputHotbar();
        if(gridInventory != null && gridInventory.childCount > 0)
        {
            for (int i = 0; i < gridInventory.childCount; i++)
            {
                Button currentBtn = gridInventory.GetChild(i).GetComponent<Button>();
                Slot currentSlot = gridInventory.GetChild(i).GetComponent<Slot>();
                if (currentBtn.interactable == false)
                {
                    currentBtn.interactable = true;
                    
                }

            }
            gridInventory.GetChild(0).GetComponent<Button>().Select();
        }
        
    }





    

    public void EnableNavigationHotbar()
    {
        DisableButtons(gridInventory);
        InputManager.instance.EnableInputHotbar();
        if (gridHotbar != null && gridHotbar.childCount > 0)
        {
            for (int i = 0; i < gridHotbar.childCount; i++)
            {
                Button currentBtn = gridHotbar.GetChild(i).GetComponent<Button>();
                if (currentBtn.interactable == false)
                {
                    currentBtn.interactable = true;
                }
            }
            
        }
    }

    public void DisableButtons(Transform grid1 = null, Transform grid2 = null,Transform grid3 = null)
    {
        if(grid1 != null && grid1.childCount > 0)
        {
            for (int i = 0; i < grid1.childCount; i++)
            {
                Button currentBtn = grid1.GetChild(i).GetComponent<Button>();
                currentBtn.interactable = false;
            }
        }

        if (grid2 != null && grid2.childCount > 0)
        {
            for (int j = 0; j < grid2.childCount; j++)
            {
                Button currentBtn = grid2.GetChild(j).GetComponent<Button>();
                currentBtn.interactable = false;
            }
        }

        if (grid3 != null && grid3.childCount > 0)
        {
            for (int k = 0; k < grid3.childCount; k++)
            {
                Button currentBtn = grid3.GetChild(k).GetComponent<Button>();
                currentBtn.interactable = false;
            }
        }
    }

}
