using System.Collections;
using System.Collections.Generic;
using BlackPearl;
using SUPERCharacter;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Scremer : MonoBehaviour
{
    public AudioSource audioSource = null;
    public AudioClip[] screamerClip;
    public GameObject monster;
    public Animator anim;
    public Camera cam;
    public SUPERCharacterAIO player;
    public bool isActive;
    public float duration;
    
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        anim = monster.GetComponent<Animator>();
        audioSource.playOnAwake = false;

        anim.enabled = true;
        cam = GetComponentInChildren<Camera>();
        //point.SetActive(false);
        cam.enabled = false;
        isActive = true;
        monster.SetActive(false);
    }


    private void OnTriggerEnter(Collider other)
    {
        
        if (other.tag == "Player")
        {
            if (!isActive) { return; }
            player = other.GetComponent<SUPERCharacterAIO>();
            
            player.PausePlayer(PauseModes.FreezeInPlace);
            
          
            StartCoroutine(PlayScreamer());
            

        }
    }

    public IEnumerator PlayScreamer()
    {

        AudioClip clip = screamerClip[Random.Range(0, screamerClip.Length)];

        monster.SetActive(true);
        cam.enabled = true;
        ThunderManager.instance.PlayThunder();
        anim.enabled = true;
        audioSource.PlayOneShot(clip);
       
        yield return new WaitForSeconds(duration);

        AudioM.instance.PlayOneShotClip(AudioM.instance.FxAudioSource, AudioM.instance.afraid);
        Destroy(monster.gameObject);
        cam.enabled = false;
        player.UnpausePlayer();
        isActive = false;
        yield return new WaitForSeconds(clip.length);

        Destroy(gameObject);
    }

}
