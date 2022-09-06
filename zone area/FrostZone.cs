using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrostZone : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
           HUD.instance.ScreenEffect("frost");

           
           
        }
    }

    private void OnTriggerExit(Collider other) {

           HUD.instance.ScreenEffect("defrost");
           
           
        
    }

    
}
