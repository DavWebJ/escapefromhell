using System.Collections;
using System.Collections.Generic;
using BlackPearl;
using UnityEngine;

public class Objectif : MonoBehaviour
{

   public ObjectifItem objectif;
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            AudioM.instance.PlayOneShotClip(AudioM.instance.objectif_audios, AudioM.instance.newObjectifClip);
            if (!objectif.objectifIsValidate)
            {
                ObjectifManager.instance.showtObjectif(objectif);
                HUDObjectif.instance.id = objectif.Id;
                HUDObjectif.instance.SetObjectif(objectif.objectif, objectif.objectifIsValidate);
            }
            
            
            Destroy(gameObject);
        }
    }


}
