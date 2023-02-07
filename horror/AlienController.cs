using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BlackPearl;

public class AlienController : MonoBehaviour
{
    public AudioSource AudioSource;
    public AudioClip scream,tension;
    public Animator anim;
    public GameObject triggerFrost;

    private void Awake()
    {
        AudioSource = GetComponent<AudioSource>();
        anim = GetComponent<Animator>();
        
        Scream();
    }



    public void Scream()
    {

        anim.SetBool("scream", true);
        


    }

    public void PlayScreamSound()
    {
        AudioSource.PlayOneShot(scream);
       // StartCoroutine(Inventory.instance.player.CameraShake(0.5f, 0.5f));

        anim.SetBool("scream", false);
        
        HUD.instance.ScreenEffect("defrost");
        Destroy(triggerFrost);
        Destroy(gameObject,1.5f);


    }




    // Update is called once per frame
    void Update()
    {

    }
}
