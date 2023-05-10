using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootStep : MonoBehaviour
{
    public AudioSource audios;
    public AudioClip[] clips;



    public void PlayFootSound()
    {
        if(clips.Length <= 0)
        {
            return;
        }

        int clip = Random.Range(0, clips.Length);

        if(!audios.isPlaying)
            audios.PlayOneShot(clips[clip]);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
