using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManAnimationController : MonoBehaviour
{

    public Animator anim;

    public AudioSource logsAudios;
    public AudioSource axeAudios;
    public AudioSource logsBaseAudio;
    public GameObject logsfull;
    public GameObject logshalf;
    public GameObject logshalfPrefabs;
    public AudioClip logsBreak, swing,hit;

    void Start()
    {
       
        anim = GetComponent<Animator>();
        logsAudios.loop = false;
        logsAudios.playOnAwake = false;
        axeAudios.loop = false;
        axeAudios.playOnAwake = false;
        axeAudios.clip = swing;
        logsAudios.clip = logsBreak;
        logsBaseAudio.loop = false;
        logsBaseAudio.playOnAwake = false;
        logsBaseAudio.clip = hit;

  
    }

    public void activatePhone()
    {
    
        anim.applyRootMotion = true;
    }


    public void PlayBeakLogs()
    {
        //logsAudios.Play();
    }

    public void PlaySwingAxe()
    {
        axeAudios.Play();
    }

    public void PlayHitLogs()
    {
        logsBaseAudio.Play();
        logsfull.SetActive(false);
        Instantiate(logshalfPrefabs, logshalf.transform);
        
    }

    public void enableLogsFull()
    {
        logsfull.SetActive(true);
    }


  
    void Update()
    {
        
    }
}
