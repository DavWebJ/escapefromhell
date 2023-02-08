using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleInterract : MonoBehaviour
{
    public bool isAnimated = false;
    public AudioSource audios;
    public AudioClip clip;
    public bool isOpen = false;
    public string events = string.Empty;



    public void PlaySound()
    {
        if (!audios.isPlaying)
            audios.PlayOneShot(clip);
    }





}
