using System.Collections;
using System.Collections.Generic;
using BlackPearl;
using UnityEngine;

public class AlienEvent : MonoBehaviour
{
    public GameObject monster;
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            monster.SetActive(true);

            Destroy(gameObject);
            
        }
    }
}
