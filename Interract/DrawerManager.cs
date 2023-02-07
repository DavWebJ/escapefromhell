using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawerManager : MonoBehaviour
{
 
    public bool isOpen;

    public string nameGameObject = "";
    public bool isInterracting;
    public string CurrentInterractionname = "";
    public AnimationClip open;
    public AnimationClip close;
    public Animation anim;
    void Start()
    {
     
        nameGameObject = name;
        anim = GetComponent<Animation>();

    }

    
    void Update()
    {

    }


    public void PlayOpenClose()
    {
        if (CurrentInterractionname == nameGameObject)
        {
            isOpen = !isOpen;

            if (anim != null)
            {
                if (isOpen)
                {
                    anim.clip = close;
                    anim.Play();
                }
                else
                {
                    anim.clip = open;
                    anim.Play();
                }
            }
        }
    }
   



    public void CloseDraw()
    {
        
        AudioM.instance.PlayCloseDraw();


 
  
    }
    public void OpenDraw()
    {
        
        AudioM.instance.PlayOpenDraw();

        

       
        
    }
}
