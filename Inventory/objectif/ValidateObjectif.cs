using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ValidateObjectif : MonoBehaviour
{
    [SerializeField] public int objectifNumber;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            AudioM.instance.PlayOneShotClip(AudioM.instance.objectif_audios, AudioM.instance.ValideObjectif);
            ObjectifManager.instance.ValidateObjectif(objectifNumber);
            HUDObjectif.instance.ValidateObjectif(objectifNumber);
            Destroy(gameObject);
        }
    }
}
