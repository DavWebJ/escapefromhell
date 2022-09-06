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
        normal,gun,pickup
    }
    public static HUD instance = null;

    [Header("Colors vitals bar")]

    public Gradient bateryBarColor = new Gradient();

    [Header("UI visual effect")]
    [SerializeField] private Animation fallAnimationEffect = null;

    [SerializeField] public Transform gridMessage = null;
    [SerializeField] public Transform gridObjectif = null;

    [SerializeField] public GameObject prf_Message = null;
    [SerializeField] public GameObject prf_objectif_message = null;
    [SerializeField] public GameObject prf_objectif_validate = null;
    [SerializeField] private GameObject scopedOverlay = null;
    [SerializeField] public GameObject smoke;
    public GameObject gameOverScreen;
    [Header("sound fx")]
    public AudioSource audioSource;
    [SerializeField] private AudioClip hitclip,hexalClip,saveClip;

    [Header("Crosshair")]
    [SerializeField] private GameObject crosshair_normal;
    [SerializeField] private GameObject crosshair_gun;
    [SerializeField] private GameObject crosshair_pickup;
    public crosshair_type crosshair_Type;
    public ForwardRendererData rendererData;
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
        gridMessage = transform.Find("HUD_Infos/message").transform;
        gridObjectif = transform.Find("HUD_Objectif/objectif").transform;
        ChangeCrossHair(crosshair_type.normal);
        gameOverScreen = transform.Find("Game Over Screen").gameObject;
        gameOverScreen.SetActive(false);
        

    }
    
    public void ScreenEffect(string nameEffect)
    {
        var frost = rendererData.rendererFeatures.OfType<MobileFrostUrp>().FirstOrDefault();
        switch (nameEffect)
        {
            case"BloodFallDamage":
            fallAnimationEffect.Play();
            if(!audioSource.isPlaying)
            {
                audioSource.PlayOneShot(hitclip);
            }
            
            break;
            case"frost":
               isInFrostZone = true;

                if (!audioSource.isPlaying)
                {
                    audioSource.clip = hexalClip;
                    audioSource.loop = true;
                    audioSource.Play();
                   
                }
                break;
            case"defrost":
                isInFrostZone = false;
                isDefrosting = false;
                
                audioSource.Stop();
                audioSource.loop = false;
                break;
            default:
            break;
        }
    }

    public void setScopedImage(bool activate)
    {

        scopedOverlay.SetActive(activate);
        
    }
    public void SetVisualMessage(bool add,Item item)
    {
        if(item == null)
        return;
        if (gridMessage.childCount > 0)
        {
            for (int i = 0; i < gridMessage.childCount; i++)
            {
                Destroy(gridMessage.GetChild(i).gameObject);
            }
        }
        VisualMessage msg = Instantiate(prf_Message,gridMessage).GetComponent<VisualMessage>();

        if (msg != null)
        {
            msg.SendVisualMessage(add,item);
        }
    }
    public void SetVisualMessage(string message,Color color,GameObject prefabs, Transform grid)
    {

        if (gridMessage.childCount > 0 || gridObjectif.childCount > 0)
        {
            for (int i = 0; i < gridMessage.childCount; i++)
            {
                Destroy(gridMessage.GetChild(i).gameObject);
            }
            for (int i = 0; i < gridObjectif.childCount; i++)
            {
                Destroy(gridObjectif.GetChild(i).gameObject);
            }
        }
        if (prefabs != null)
        {
            
            VisualMessage msg = Instantiate(prefabs, grid).GetComponent<VisualMessage>();
            
            if (msg != null)
            {
                msg.SendVisualMessage(message, color);
            }
        }
    }

    public void ChangeCrossHair(crosshair_type crosshairtype)
    {
        switch (crosshairtype)
        {
            case crosshair_type.normal:
            crosshair_gun.SetActive(false);
            crosshair_pickup.SetActive(false);
            crosshair_normal.SetActive(true);
            break;
            case crosshair_type.gun:
            crosshair_gun.SetActive(true);
            crosshair_pickup.SetActive(false);
            crosshair_normal.SetActive(false);
            break;
            case crosshair_type.pickup:
            crosshair_gun.SetActive(false);
            crosshair_pickup.SetActive(true);
            crosshair_normal.SetActive(false);
            break;
            default:
            break;
        }
    }



    private void Update()
    {
        timer += Time.deltaTime;
        if (isInFrostZone)
        {
            if (isDefrosting)
            {
                Frost();
            }
            
            if (timer >= exhalTransitionTime && isInFrostZone)
            {
                Invoke("ActivateSmoke",0.1f);
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
        smoke.SetActive(true);
        Invoke("DeactivateSmoke", 2f);
    }

    public void DeactivateSmoke()
    {
        smoke.SetActive(false);
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

    public void PlaySaveClip()
    {
        audioSource.PlayOneShot(saveClip);
    }

}
