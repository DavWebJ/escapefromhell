using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutScene : MonoBehaviour
{
    //public Animator anim;
    public GameObject player;
    //public GameObject phone;
    public Animator playerAnim;
    public AudioSource phoneAudios;
    public AudioSource ambientAudios;
    public AudioClip phoneRing;
    public AudioClip coming, departure;

    public bool stopSmouthVolume = false;
    public bool incomingCall = false;
    private void Awake()
    {
        //phone.SetActive(false);
        ambientAudios.clip = coming;
        ambientAudios.loop = true;
        ambientAudios.Play();
    }
    void Start()
    {
        //anim = GetComponent<Animator>();
        playerAnim = player.GetComponent<Animator>();
        phoneAudios.playOnAwake = false;
    }

    public void IncomingCall()
    {
        phoneAudios.clip = phoneRing;
        phoneAudios.loop = false;
        phoneAudios.Play();
    }

    public void DisplayThePhone()
    {
        //phone.SetActive(true);
    }

    public void AnswerPhone()
    {
        
    }


    public void stopSoundVolume()
    {
        stopSmouthVolume = true;
    }


    
    void Update()
    {
        if (stopSmouthVolume)
        {
            if(ambientAudios.volume >= 0.1f)
            {
                ambientAudios.volume = Mathf.Lerp(ambientAudios.volume, 0, Time.deltaTime * 0.7f);
            }
            else
            {
                stopSmouthVolume = false;
                incomingCall = true;
                
            }

        }

        if (incomingCall)
        {
            PlayIncomingCallSound();
        }
    }

    private void PlayIncomingCallSound()
    {
        if (incomingCall)
        {
            incomingCall = false;
            StartCoroutine(PlayStandUpAnimation());

            
           
        }
    }

    public IEnumerator PlayStandUpAnimation()
    {
        ambientAudios.Stop();
        phoneAudios.clip = phoneRing;
        phoneAudios.Play();
        
        playerAnim.SetBool("stand", true);
        yield break;
    }
}
