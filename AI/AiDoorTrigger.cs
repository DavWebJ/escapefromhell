using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiDoorTrigger : MonoBehaviour
{
    public GameObject door;
    public bool isActive = false;
    private void OnTriggerEnter(Collider other)
    {

        if (other.tag == "NPC" && !isActive)
        {
            isActive = true;
            door.GetComponent<AIDoor>().PlayOpenSound();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        isActive = false;
        door.GetComponent<AIDoor>().PlayOpenSound();
    }

    private void OnTriggerStay(Collider other)
    {
        return;
    }
}
