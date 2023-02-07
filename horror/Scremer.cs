using System.Collections;
using System.Collections.Generic;
using BlackPearl;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Scremer : MonoBehaviour
{
    public AudioSource audioSource = null;
    public AudioClip[] screamerClip;
    public GameObject monster;
    public Animator anim;
    //public FirstPersonAIO player;
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        anim = monster.GetComponent<Animator>();
        audioSource.playOnAwake = false;
        monster.SetActive(false);
    
    }


    private void OnTriggerEnter(Collider other)
    {
        
        if (other.tag == "Player")
        {
           // other.GetComponent<FirstPersonAIO>().SetController(false);
            
            StartCoroutine(PlayScreamer());
            
        }
    }

    public IEnumerator PlayScreamer()
    {

        AudioClip clip = screamerClip[Random.Range(0, screamerClip.Length)];

       // AudioM.instance.screamer_audios.PlayOneShot(clip);
       
       //player.StartCoroutine(player.CameraShake(0.5f, 0.5f));
        
        
        
        monster.SetActive(true);
        monster.GetComponent<Animator>().enabled = true;
     
        yield return new WaitForSeconds(1);
        //player.SetController(true);
        Destroy(monster.gameObject);   
        Destroy(gameObject);
    }

}
