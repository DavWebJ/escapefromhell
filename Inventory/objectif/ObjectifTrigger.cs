using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectifTrigger : MonoBehaviour
{
    public ObjectifItem objectif;
    public bool isActive = false;
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {

            if (!isActive)
            {
                isActive = true;
                ObjectifManager.instance.SetObjectif(ObjectifManager.instance.objectifItems[objectif.Id].itemData);
                AudioM.instance.PlayNewObjectif();
                Destroy(gameObject, 3);
            }
            return;

        }
    }
}
