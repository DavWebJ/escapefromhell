using System.Collections;
using System.Collections.Generic;
using BlackPearl;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;
using UnityEditor;
using TMPro;
using System.Linq;

public class HUD : MonoBehaviour
{
    public enum crosshair_type
    {
        normal,gun,pickup,None,Interract
    }
    public static HUD instance = null;

    [Header("UI visual effect")]
    [SerializeField] private Animation fallAnimationEffect = null;
    [SerializeField] private Animation gameoverAnimation;
    [SerializeField] private Animation healthAnimation;


    public FPSCamera cam;
    public Canvas healthUicanvas;

    public Gradient bateryBarColor = new Gradient();
    [Header("sound fx")]
    public AudioSource audioSource;
    [SerializeField] private AudioClip hitclip,hexalClip,saveClip;

    [Header("Crosshair")]
    [SerializeField] private GameObject crosshair_normal;
    [SerializeField] private GameObject crosshair_gun;
    [SerializeField] private GameObject crosshair_pickup;
    [SerializeField] private GameObject crosshair_interract;
    public crosshair_type crosshair_Type;
    public UniversalRendererData rendererData;
    public bool isInFrostZone = false;
    public bool isDefrosting = true;
    public float timer = 0;
    public float exhalTransitionTime = 2.5f;

    private void Awake() {
    if(instance == null)
        instance = this;
        
    }

    private void Start() {

        audioSource = GetComponent<AudioSource>();
        crosshair_normal = transform.Find("screen effect/Canvas/crosshairnormal").gameObject;
        crosshair_gun = transform.Find("screen effect/Canvas/crosshairgun").gameObject;
        crosshair_pickup = transform.Find("screen effect/Canvas/crosshairpickup").gameObject;
        crosshair_interract = transform.Find("screen effect/Canvas/crosshairinterract").gameObject;

        ChangeCrossHair(crosshair_type.normal);
        cam = FindObjectOfType<FPSCamera>();
        

    }

    public void showHealth()
    {
        healthAnimation.Play();
    }

    public void HealthIsInDangerousValue()
    {
        healthUicanvas.GetComponent<CanvasGroup>().alpha = 1;
    }
    
    public void ScreenEffect(string nameEffect)
    {
        //var frost = rendererData.rendererFeatures.OfType<MobileFrostUrp>().FirstOrDefault();
        switch (nameEffect)
        {
            case"BloodFallDamage":
            fallAnimationEffect.Play();
                
                //if(!audioSource.isPlaying)
                //{
                //    audioSource.PlayOneShot(hitclip);
                //}
                break;
            case "frost":
                isInFrostZone = true;

                if (!audioSource.isPlaying)
                {
                    audioSource.clip = hexalClip;
                    audioSource.loop = true;
                    audioSource.Play();

                }
                break;
            case "defrost":
                isInFrostZone = false;
                isDefrosting = false;

                audioSource.Stop();
                audioSource.loop = false;
                break;
            case "GameOver":
                gameoverAnimation.Play();
                break;
            default:
            break;
        }
    }




    public void ChangeCrossHair(crosshair_type crosshairtype)
    {
        switch (crosshairtype)
        {
            case crosshair_type.normal:
                crosshair_gun.SetActive(false);
                crosshair_pickup.SetActive(false);
                crosshair_normal.SetActive(true && !Inventory.instance.isInventoryOpen);
                crosshair_interract.SetActive(false);
                break;
            case crosshair_type.gun:
                crosshair_gun.SetActive(true && !Inventory.instance.isInventoryOpen);
                crosshair_pickup.SetActive(false);
                crosshair_normal.SetActive(false);
                crosshair_interract.SetActive(false);
                break;
            case crosshair_type.pickup:
                crosshair_gun.SetActive(false);
                crosshair_pickup.SetActive(true);
                crosshair_normal.SetActive(false);
                crosshair_interract.SetActive(false);
                break;
            case crosshair_type.None:
                crosshair_gun.SetActive(false);
                crosshair_pickup.SetActive(false);
                crosshair_normal.SetActive(false);
                crosshair_interract.SetActive(false);
                break;
            case crosshair_type.Interract:
                crosshair_gun.SetActive(false);
                crosshair_pickup.SetActive(false);
                crosshair_normal.SetActive(false);
                crosshair_interract.SetActive(true && !Inventory.instance.isInventoryOpen);
                break;
            default:
            break;
        }
    }

    public void PlaySaveClip()
    {
        audioSource.PlayOneShot(saveClip);
    }


    private void Update()
    {
        
        if (isInFrostZone)
        {
            timer += Time.deltaTime;
            if (isDefrosting)
            {
                Frost();
            }

            if (timer >= exhalTransitionTime && isInFrostZone)
            {
                Invoke("ActivateSmoke", 0.1f);
                timer = 0.0f;

            }


        }
        else
        {
            if (!isDefrosting)
            {
                Defrost();
            }
            else
            {
                return;
            }
        }
    }

    public void ActivateSmoke()
    {
        cam.smoke.SetActive(true);
        Invoke("DeactivateSmoke", 2f);
    }

    public void DeactivateSmoke()
    {
       cam.smoke.SetActive(false);
    }

    public void Defrost()
    {
        var frost = rendererData.rendererFeatures.OfType<MobileFrostUrp>().FirstOrDefault();

        frost.settings.Vignette = Mathf.Lerp(frost.settings.Vignette, 0, Time.deltaTime / 2f);

        if (frost.settings.Vignette <= 0.2f)
        {
            frost.settings.Vignette = 0;
            isDefrosting = true;


        }

        rendererData.SetDirty();



    }

    public void Frost()
    {
        var frost = rendererData.rendererFeatures.OfType<MobileFrostUrp>().FirstOrDefault();

        frost.settings.Vignette = Mathf.Lerp(frost.settings.Vignette, 0.35f, Time.deltaTime / 2f);

        if (frost.settings.Vignette >= 0.34f)
        {
            frost.settings.Vignette = 0.35f;


        }

        rendererData.SetDirty();

    }

}
