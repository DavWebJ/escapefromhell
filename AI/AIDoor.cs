using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIDoor : MonoBehaviour
{
    public Animation animator;
    public AnimationClip open, close;
    public AudioSource audioS;
    public AudioClip openDoor, scree;
    public bool isClosed = true;
    void Start()
    {
        animator = GetComponent<Animation>();

        audioS = GetComponent<AudioSource>();
    }

    public void PlayOpenSound()
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
        audioS.PlayOneShot(openDoor);
        isClosed = !isClosed;

    }



    public void ScreeSound()
    {

        audioS.PlayOneShot(scree);
    }


}
