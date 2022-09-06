using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySound : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
           
            AudioM.instance.PlayScreamerAmbience();
            Destroy(gameObject, 1);
        }
    }
}
