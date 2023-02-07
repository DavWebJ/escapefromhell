using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using BlackPearl;
public class VisualMessage : MonoBehaviour
{


    private Text visualMessage = null;
    public Image icon = null;

    private void Awake() {
        
  
       
       visualMessage = transform.Find("message").GetComponent<Text>();
    
        icon = transform.Find("icon").GetComponent<Image>();
        

    
        visualMessage.text = "";
    }

    private void Start() {
        Destroy(gameObject, 7f);
    }

    public void SendVisualMessage(bool add, Item item,int amount)
    {
        if(item == null)
        {
            Destroy(gameObject);
        }else
        {
            visualMessage.text = "+ " + amount.ToString() +" "+ item.ItemName;
            visualMessage.color = (add) ? Color.green  : Color.red;
            icon.sprite = item.ItemIcon;
            icon.preserveAspect = true;
     
                



        }
        
    }



    public void SendVisualMessage(string message,Color color)
    {
        if(message == string.Empty)
        {
            Destroy(gameObject);
        }else
        {

  
            //icon.gameObject.SetActive(false);
            //icon.sprite = null;
            visualMessage.text = message;
            visualMessage.color = color;
   
                
        }
    }

    public void SendVisualMessage(string message)
    {
        if (message == string.Empty)
        {
            Destroy(gameObject);
        }
        else
        {


            icon.gameObject.SetActive(true);
            visualMessage.text = message;

         
        }
    }

    public void SendInventoryVisualMessage(string message, Color color)
    {

        if (message == string.Empty)
        {
            Destroy(gameObject);
        }
        else
        {
            Animation anim;
            anim = GetComponent<Animation>();
            visualMessage.text = message;
            visualMessage.color = color;
            icon.gameObject.SetActive(true);
            if(anim != null)
            {
                anim.Play();
            }
        
             
        }
    }

    public void SendObjectif(string objectif)
    {
        if (objectif == string.Empty)
        {
            Destroy(gameObject);
        }
        else
        {

       
            icon.gameObject.SetActive(false);
            icon.sprite = null;
            icon.enabled = false;
            visualMessage.text = objectif;
            

        }
    }

  
    void Update()
    {
        
    }
}
