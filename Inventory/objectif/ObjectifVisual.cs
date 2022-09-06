using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using BlackPearl;

public class ObjectifVisual : MonoBehaviour
{

    public Text visualMessage = null;
    public Image unValide = null;
    public Image valide = null;
    public int currentObjectif;

    private void Awake()
    {


        visualMessage.text = "";
        
    }

    public void SetVisualObjectif(string objectif,bool validate,int id)
    {
        if (objectif == string.Empty)
        {
            Destroy(gameObject);
        }
        else
        {
            if (validate)
            {
                valide.gameObject.SetActive(true);
                unValide.gameObject.SetActive(false);
            }
            else
            {
                valide.gameObject.SetActive(false);
                unValide.gameObject.SetActive(true);
            }

            visualMessage.text = objectif;
            currentObjectif = id;


        }
    }

    public void SetValideObjectif()
    {

      valide.gameObject.SetActive(true);
      unValide.gameObject.SetActive(false);
        return;
  
    }

}
