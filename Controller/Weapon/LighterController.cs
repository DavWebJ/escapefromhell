using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BlackPearl;
[RequireComponent(typeof(AudioSource))]
public class LighterController : MonoBehaviour
{
    [Header("References")]

    ArmsController arm;
    [SerializeField] private AudioSource audios = null;
    


    [Header("FlashLight parameter")]
    [SerializeField] public Light lighter;
    public float DefaultIntensity = 8f;
    public bool isInit = false;
    public AudioClip open, lighting;
    public GameObject flame;

    private void Awake()
    {
        //HUDWeapon.instance.GetWeaponInfos(null);
        flame.SetActive(false);
    }

    private void Start()
    {
        InitialiseLightController();
    }

    public void InitialiseLightController()
    {
        audios = GetComponent<AudioSource>();
        //animator = GetComponent<Animator>();
        arm = GetComponent<ArmsController>();

        audios.playOnAwake = false;
        audios.loop = false;

   

        

        
       // HUD.instance.ChangeCrossHair(HUD.crosshair_type.normal);
        lighter.intensity = DefaultIntensity;
        isInit = true;
    }




    public void OpenZippo()
    {
        audios.PlayOneShot(open);
    }

    public void StartLighter()
    {
        audios.PlayOneShot(lighting);
        lighter.enabled = true;
        lighter.intensity = 1;
        lighter.range = 3;
        flame.SetActive(true);
    }


}
