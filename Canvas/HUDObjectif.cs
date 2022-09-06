using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BlackPearl;

public class HUDObjectif : MonoBehaviour
{
    public static HUDObjectif instance = null;
    public GameObject pref_objectif;
    public Transform grid;
    public int id;

    private void Awake() {
    if(instance == null)
        instance = this;
        
    }

    public void SetObjectif(string text,bool validate)
    {
        GameObject obj = Instantiate(pref_objectif, grid);

        if(obj.GetComponent<ObjectifVisual>() != null)
        {
            obj.GetComponent<ObjectifVisual>().SetVisualObjectif(text,validate,id);
        }
        
    }

    public void ValidateObjectif(int number)
    {
        for (int i = 0; i < grid.childCount; i++)
        {
            if (grid.GetChild(i).GetComponent<ObjectifVisual>().currentObjectif == number)
            {
                grid.GetChild(i).GetComponent<ObjectifVisual>().SetValideObjectif();
            }
            else
            {
                return;
            }
        }
    }
  
    void Update()
    {
        
    }
}
