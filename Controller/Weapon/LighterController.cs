using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BlackPearl;
[RequireComponent(typeof(AudioSource))]
public class LighterController : MonoBehaviour
{
    [Header("References")]
   
    public WeaponItem weapon = null;
    [SerializeField] private AudioSource audios = null;
    private Animator animator = null;
    private FirstPersonAIO player;

    [Header("FlashLight parameter")]
    [SerializeField] public Light lighter;
    public float DefaultIntensity = 8f;
    public string weaponItemName;
    public bool isInit = false;
    public AudioClip open, lighting;
    public GameObject flame;

    private void Awake()
    {
        HUDWeapon.instance.GetWeaponInfos(null);
        flame.SetActive(false);
    }

    public void InitialiseLightController()
    {
        audios = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();

        audios.playOnAwake = false;
        audios.loop = false;

        player = FindObjectOfType<FirstPersonAIO>();

        

        
        HUD.instance.ChangeCrossHair(HUD.crosshair_type.normal);
        lighter.intensity = DefaultIntensity;
        isInit = true;
    }


    public void SetItem(Item item)
    {

        if (item.itemType != ItemType.FlashLight)
            return;
        weapon = item as WeaponItem;



        InitialiseLightController();

    }

    public void OpenZippo()
    {
        audios.PlayOneShot(open);
    }

    public void StartLighter()
    {
        audios.PlayOneShot(lighting);
        lighter.enabled = true;
        lighter.intensity = 2;
        lighter.range = 15;
        flame.SetActive(true);
    }

    public void updateInputsFlashLight()
    {


        if (!player.IsGrounded)
        {

            animator.SetBool(weapon.fl_walk_animation, false);
            animator.SetBool(weapon.fl_sprint_animation, false);

        }

        if (!player.isWalking && !player.isSprinting)
        {



            animator.SetBool(weapon.fl_walk_animation, false);
            animator.SetBool(weapon.fl_sprint_animation, false);

        }

        if (player.isWalking && player.fps_Rigidbody.velocity != Vector3.zero)
        {


            animator.SetBool(weapon.fl_walk_animation, true);


            animator.SetBool(weapon.fl_sprint_animation, false);

        }

        if (player.isSprinting && player.fps_Rigidbody.velocity != Vector3.zero)
        {

            animator.SetBool(weapon.fl_walk_animation, false);
            animator.SetBool(weapon.fl_sprint_animation, true);


        }

    }
}
