using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseExit : MonoBehaviour
{

    public GameObject door;
    public AudioSource AudioSource;
    public AudioClip close,stringer,tension;
    public string doortype = "";

    private void Start()
    {
        AudioSource = GetComponent<AudioSource>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            
            AudioSource.PlayOneShot(close);
     
        }
    }

    
}
