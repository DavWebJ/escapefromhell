using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenDoor : MonoBehaviour
{
    public Animation animator;
    public AnimationClip open, close;
    public AudioSource audioS;
    public AudioClip openDoor,scree,closeClip;

    public bool isClosed = true;
    void Start()
    {
        animator = GetComponent<Animation>();
     
        audioS = GetComponent<AudioSource>();
    }

    public void PlayOpenDoor()
    {
        if (isClosed)
        {
            animator.clip = open;
        }
        else
        {
            animator.clip = close;
        } 
        
        animator.Play();

        
        isClosed = !isClosed;


    }


    public void PlayOpenSound()
    {
        audioS.PlayOneShot(openDoor);
    }
    public void PlayClose()
    {
        audioS.PlayOneShot(closeClip);
    }

    public void ScreeSound()
    {
       
        audioS.PlayOneShot(scree);
    }

    void Update()
    {
        
    }
}
