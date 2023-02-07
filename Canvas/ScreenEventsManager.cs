using System.Collections;
using System.Collections.Generic;
using BlackPearl;
using UnityEngine;

public class ScreenEventsManager : MonoBehaviour
{
    public static ScreenEventsManager instance;

    [SerializeField] public Transform gridMessage = null;
    [SerializeField] public Transform gridObjectifMessage = null;
    [SerializeField] public Transform gridInventoryMessage = null;

    [SerializeField] public GameObject prf_Message = null;
    [SerializeField] public GameObject prf_objectif_message = null;
    //[SerializeField] public GameObject prf_objectif_validate = null;
    [SerializeField] public GameObject prf_inventory_message = null;
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }
    void Start()
    {
        gridInventoryMessage = transform.Find("grid_inventory_message");
        gridObjectifMessage = transform.Find("grid_objectif_message ");
        gridMessage = transform.Find("grid_pickup_message ");

    }

    public void SetVisualMessage(bool add, Item item, int amount)
    {
        if (item == null)
            return;
        if (gridMessage.childCount > 0)
        {
            for (int i = 0; i < gridMessage.childCount; i++)
            {
                Destroy(gridMessage.GetChild(i).gameObject);
            }
        }
        VisualMessage msg = Instantiate(prf_Message, gridMessage).GetComponent<VisualMessage>();

        if (msg != null)
        {
            msg.SendVisualMessage(add, item, amount);
        }
    }

    public void SetVisualMessage(string message, Color color)
    {
        if (message == string.Empty)
            return;
        if (gridMessage.childCount > 0)
        {
            for (int i = 0; i < gridMessage.childCount; i++)
            {
                Destroy(gridMessage.GetChild(i).gameObject);
            }
        }
        VisualMessage msg = Instantiate(prf_Message, gridMessage).GetComponent<VisualMessage>();

        if (msg != null)
        {
            msg.SendVisualMessage(message, Color.green);
        }
    }

    public void SetVisualMessage(string message,GameObject pref,Transform grid)
    {
        if (message == string.Empty)
            return;
        if (grid.childCount > 0)
        {
            for (int i = 0; i < grid.childCount; i++)
            {
                Destroy(grid.GetChild(i).gameObject);
            }
        }
        VisualMessage msg = Instantiate(pref, grid).GetComponent<VisualMessage>();

        if (msg != null)
        {
            msg.SendVisualMessage(message);
        }
    }

    public void SetInventoryVisualMessage(string message, Color color, GameObject prefabs, Transform grid)
    {
        if (message == string.Empty)
            return;

        if (gridMessage.childCount > 0)
        {
            for (int i = 0; i < gridMessage.childCount; i++)
            {
                Destroy(gridMessage.GetChild(i).gameObject);
            }
        }
        if (prefabs != null)
        {

            VisualMessage msg = Instantiate(prefabs, grid).GetComponent<VisualMessage>();

            if (msg != null)
            {
                msg.SendVisualMessage(message, color);
            }
        }
    }
    public void SetVisualMessage(string message, Color color, GameObject prefabs, Transform grid, bool isObjectif)
    {

        if (grid.childCount > 0)
        {
            for (int i = 0; i < grid.childCount; i++)
            {
                Destroy(grid.GetChild(i).gameObject);
            }
        }
        if (prefabs != null)
        {

            VisualMessage msg = Instantiate(prefabs, grid).GetComponent<VisualMessage>();
            if (isObjectif)
            {

            }
            if (msg != null)
            {
                msg.SendVisualMessage(message, color);
            }
        }
    }
}
