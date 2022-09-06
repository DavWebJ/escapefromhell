using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenDoorMenu : MonoBehaviour
{
    public Animator animator;
    public AudioSource audioS;
    public AudioClip open;
    void Start()
    {
        animator = GetComponent<Animator>();
        animator.enabled = false;
        audioS = GetComponent<AudioSource>();
    }

    public void PlaySound()
    {
        audioS.PlayOneShot(open);
    }
    
    void Update()
    {
        
    }
}
