using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BlackPearl;
[RequireComponent(typeof(AudioSource))]
public class HealthArmsController : MonoBehaviour
{
    [Header("References")]

    private Item item = null;

    [SerializeField] private AudioSource audios = null;
    private Animator animator = null;

    public bool isInitialized = false;
   public void Initialized()
    {
        audios = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
        audios.playOnAwake = false;
        audios.loop = false;
        audios.volume = 0.5f;
     
       
        // Heal();
        StartCoroutine(PlayHeal());
        isInitialized = true;
    }



    public void SetToolsItem(Item item)
    {
        this.item = item;
        

    }

    public void Heal()
    {
        
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if(!audios.isPlaying)
        {
            
        }
        //player.GetComponent<VitalState>().AddHealthFull(100);

    }
    public IEnumerator PlayHeal()
    {
    
        yield return new WaitForSeconds(3.2f);

        HotBar.instance.Selection();
        yield break;

    }

   
    void Update()
    {
        
    }
}
