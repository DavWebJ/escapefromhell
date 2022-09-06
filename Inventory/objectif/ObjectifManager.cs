using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectifManager : MonoBehaviour
{
    public static ObjectifManager instance;
    private ObjectifItem[] objectifDatabase = null;
    //public ObjectifItem objectif;



    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }
    void Start()
    {
        LoadObjectif();
    }
    public void LoadObjectif()
    {
        objectifDatabase = Resources.LoadAll<ObjectifItem>("Inventory/Objectif");



    }
    public ObjectifItem GetObjectifById(int id)
    {
        if (objectifDatabase.Length <= 0)
        {
            return null;
        }

        for (int i = 0; i < objectifDatabase.Length; i++)
        {
            if (objectifDatabase[i].Id == id)
            {
                return Instantiate(objectifDatabase[i]);
            }
        }
        return null;
    }

    public void SetObjectif(int id)
    {

    }
    public void ValidateObjectif(int number)
    {
        ObjectifItem currentObjectif = GetObjectifById(number);
        if (currentObjectif.Id == number && !currentObjectif.objectifIsValidate)
        {

            HUD.instance.SetVisualMessage("Vous avez remplie un objectif", Color.white, HUD.instance.prf_objectif_validate, HUD.instance.gridObjectif);
            currentObjectif.objectifIsValidate = true;
            return;


        }
        else
        {
            return;
        }
    }

    public void showtObjectif(ObjectifItem objectif)
    {
        HUD.instance.SetVisualMessage(null, Color.white, HUD.instance.prf_objectif_message, HUD.instance.gridObjectif);
        SetObjectif(objectif.Id);
        
    }

}
