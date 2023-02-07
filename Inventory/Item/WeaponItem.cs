using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BlackPearl
{
     [CreateAssetMenu(menuName = "Inventory/Item/Create Weapon")]
    public class WeaponItem : Item
    {
        [Header("Gun References")]
        public string pathToMuzzleFx;
        public string pathToMuzzleTransform;
        public string pathToCartridgeFx;
        [Header("Gun sound")]
        public AudioClip gun_equiped_sound,gun_unequiped_sound,cartridge_clip,fire_clip,gun_reload_clip,empty_clip;

        [Header("FlashLight audio")]
        public AudioClip fl_equiped_sound = null;
        public AudioClip fl_unequiped_sound = null;
        public AudioClip fl_reload_clip;
        public AudioClip on_off_clip = null;
        public AudioClip batery_out_clip = null;
        public AudioClip zippo_open_clip = null;
        [Header("flashlight properties")]
        public float batery;
        public float batery_max = 1;
        public float batery_reduce_value;
        public float batery_decrease = 1;
        [Header("flashlight animation")]
        public string fl_equipedTools = string.Empty;
        public string fl_unEquipedTools = string.Empty;
        public string fl_walk_animation = string.Empty;
        public string fl_sprint_animation = string.Empty;
        public string fl_reload_animation = string.Empty;


        [Header("Weapon Properties")]
        public float cooldown_auto = .1f;
        public float cooldown_semi = .5f;
        public float cooldown = 0;
        public float recoil_force =0;
        public int weaponDamage;
        public int bonusDamageMultiplier;
        


        [Header("Ammo")]
        public int ammo = 20;
        public int max_ammo = 20;

        [Header("weapon animation")]
        public string equipedWeapon =string.Empty;
        public string unEquipedWeapon =string.Empty;
        public string walk_animation =string.Empty;
        public string sprint_animation =string.Empty;
        public string shot_animation =string.Empty;
        public string walk_aim_animation =string.Empty;
        public string shot_aim_animation =string.Empty;
        public string reload_animation =string.Empty;
        public string aim_animation = string.Empty;

        [Header("Aiming Position")]
        public Vector3 aimingPos = new Vector3();

        public WeaponItem()
        {
            //this.itemType = ItemType.Weapon;
        }
    
    }
}