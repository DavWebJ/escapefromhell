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
    public Transform playerT;
    public GameObject point;
    public SUPERCharacterAIO player;
    public bool isActive;
    public float duration;
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        anim = monster.GetComponent<Animator>();
        audioSource.playOnAwake = false;
        monster.SetActive(false);
        isActive = true;
        point.SetActive(false);
    
    }


    private void OnTriggerEnter(Collider other)
    {
        
        if (other.tag == "Player")
        {
            if (!isActive) { return; }
            player = other.GetComponent<SUPERCharacterAIO>();
            player.PausePlayer(PauseModes.FreezeInPlace);
            playerT = other.transform;
            StartCoroutine(PlayScreamer());
            

        }
    }

    public IEnumerator PlayScreamer()
    {

        AudioClip clip = screamerClip[Random.Range(0, screamerClip.Length)];

        monster.SetActive(true);
        point.SetActive(true);
        monster.GetComponent<Animator>().enabled = true;
        monster.transform.LookAt(playerT);
        audioSource.PlayOneShot(clip);
        yield return new WaitForSeconds(duration);
        player.UnpausePlayer();
        Destroy(monster.gameObject);
        isActive = false;
        yield return new WaitForSeconds(clip.length);
        
        
        Destroy(gameObject);
    }

}
