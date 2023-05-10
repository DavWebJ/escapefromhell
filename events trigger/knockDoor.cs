using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class knockDoor : MonoBehaviour
{

    public float timer = 0;
    public float eventsBetween;

    public AudioSource audios;
    public AudioClip[] clips;

    public int numberTimeToPlayClip;
    public int index;

    public bool canPlayClip = false;
    void Start()
    {
        audios = GetComponent<AudioSource>();
    }


    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            canPlayClip = true;
        }
    }

    

    void Update()
    {

        if(index > numberTimeToPlayClip)
        {
            canPlayClip = false;
            Destroy(gameObject);
            
        }

        if (canPlayClip && index <= numberTimeToPlayClip)
        {
            timer += Time.deltaTime;

            if(timer >= eventsBetween)
            {
               int clipChoose = Random.Range(0, clips.Length);

                if (!audios.isPlaying)
                {
                    audios.PlayOneShot(clips[clipChoose]);
                    index ++;
                }
                timer = 0;

            }
        }
    }
}
