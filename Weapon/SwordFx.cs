using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordFx : MonoBehaviour
{
    [Header("FX")]
    public ParticleSystem normalWeaponTrail;
    void Start()
    {
        
    }

    
    public void PlayWeaponFx()
    {
        normalWeaponTrail.Stop();
        if (normalWeaponTrail.isStopped)
        {
            normalWeaponTrail.Play();
        }
    }
    void Update()
    {
        
    }
}
