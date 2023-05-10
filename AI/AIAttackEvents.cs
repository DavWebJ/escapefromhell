using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIAttackEvents : MonoBehaviour
{

    public AudioSource audios;
    public AIDamageDealer damageDealer;
    public AudioClip clipWeaponFx,scream;

    
    void Update()
    {
        
    }

    public void StartDealDamage()
    {
        if(damageDealer != null)
        {
            damageDealer.StartDealDamage();
        }
    }
    public void EndDealDamage()
    {
        damageDealer.EndDealDamage();
    }

    public void PlayWeaponFx()
    {
        if(audios != null && !audios.isPlaying)
        {
            audios.PlayOneShot(clipWeaponFx);
        }
    }

    public void PlayScream()
    {
        if (audios != null && !audios.isPlaying)
        {
            audios.PlayOneShot(scream);
        }
    }
}
