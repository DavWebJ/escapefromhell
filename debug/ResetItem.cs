using System.Collections;
using System.Collections.Generic;
using BlackPearl;
using UnityEngine;

public class ResetItem : MonoBehaviour
{
    public Item batery;
    public Item ammo;
    public WeaponItem gun;
    public WeaponItem fl;
    void Awake()
    {

        if (batery != null)
        {
            batery.amount = 1;
        }

        ammo.amount = 8;
        gun.ammo = 0;
        fl.batery = 0;

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
