using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ObjectifManager : MonoBehaviour
{
    public static ObjectifManager instance;
    private ObjectifItem[] objectifDatabase = null;
    public List<ObjectifInDatabase> objectifItems = new List<ObjectifInDatabase>();
    public GameObject pref_objectif_panel;
    public Transform grid;
    public GameObject objectif_empty;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    void Start()
    {
        grid = transform.Find("grid");
        LoadObjectif();
    }
    public void LoadObjectif()
    {
        objectifDatabase = Resources.LoadAll<ObjectifItem>("Inventory/Objectif");

        for (int i = 0; i < objectifDatabase.Length; i++)
        {
            objectifItems.Add(new ObjectifInDatabase
            {
                itemData = objectifDatabase[i],
                id = objectifDatabase[i].Id
            });
        }

    }
    public ObjectifInDatabase GetObjectifById(ObjectifItem obj)
    {
        ObjectifInDatabase objectif = objectifItems.Where(elem => elem.id == obj.Id).FirstOrDefault();
        return objectif;
    }

    public void SetObjectif(ObjectifItem obj)
    {
        ObjectifInDatabase newobjectif = GetObjectifById(obj);
        if(newobjectif != null)
        {
           
            showObjectif(newobjectif.itemData);
        }
        
    }
    public void ValidateObjectif(ObjectifItem obj)
    {
        ObjectifInDatabase objectif = GetObjectifById(obj);
        
            StartCoroutine(DestroyObjectif(objectif));
        
    }

    public IEnumerator DestroyObjectif(ObjectifInDatabase obj)
    {
        AudioM.instance.PlayOneShotClip(AudioM.instance.objectif_audios, AudioM.instance.ValideObjectif);
        yield return new WaitForSeconds(1);
        for (int i = 0; i < grid.childCount; i++)
        {
            ObjectifVisual item = grid.GetChild(i).GetComponent<ObjectifVisual>();
            if (item.currentObjectif == obj.id)
            {
                item.valide.enabled = true;
                item.unValide.enabled = false;
                Destroy(item.gameObject,2);
            }
        }
        ScreenEventsManager.instance.SetVisualMessage("Vous avez rempli un objectif !", ScreenEventsManager.instance.prf_objectif_message, ScreenEventsManager.instance.gridObjectifMessage);
        objectifItems.Remove(obj);
        UpdateObjectifPanel();
        yield break;

    }

    public void showObjectif(ObjectifItem _objectif)
    {
        if(_objectif != null)
        {

            ScreenEventsManager.instance.SetVisualMessage("Vous avez un nouvel objectif !", ScreenEventsManager.instance.prf_objectif_message, ScreenEventsManager.instance.gridObjectifMessage);

            GameObject objectifPrefabs = Instantiate(pref_objectif_panel, grid);
           ObjectifVisual obj =  objectifPrefabs.GetComponent<ObjectifVisual>();
            obj.currentObjectif = _objectif.Id;
            obj.valide.enabled = false;
            obj.unValide.enabled = true;
            obj.visualMessage.text = _objectif.objectif;
        }

        UpdateObjectifPanel();
        
        
    }

    public void UpdateObjectifPanel()
    {
        if (grid.childCount > 0)
        {
            objectif_empty.SetActive(false);
        }
        else
        {
            objectif_empty.SetActive(true);
        }
    }
}

[System.Serializable]
public class ObjectifInDatabase
{
    public ObjectifItem itemData;
    public int id;
}
