using BlackPearl;
using SUPERCharacter;
using System.Collections;
using UnityEngine;
using VLB;

[RequireComponent(typeof(AudioSource))]
public class FlashLightController : MonoBehaviour
{
    [Header("References")]

    public WeaponItem weapon = null;
    [SerializeField] private AudioSource audios = null;
    public AudioClip batery_out_clip, fl_reload_clip;
    public Gradient lightColor;
    private SUPERCharacterAIO player;

    [Header("FlashLight parameter")]
    [SerializeField] public Light flashlight;
    public float DefaultIntensity = 8f;
    public bool isFlashlightEquiped = false;
    public bool isReloading = false;
    public bool isInit = false;
    public bool switch_on = false;

    public float timer = 0;
    public float maxTimeToDecrease;
    public int bateryValueDecrease = 5;
    
    public ArmsController arm;

    [Header("Volumetric Light")]
    public bool initFx = false;
    public bool stopflicker = false;
    public VolumetricLightBeam volumetricLight;
    public float volumetricLightDefaultIntensity = 0.12f;
    public float volumetricDecreaseTime = 2;

    public FlashLightInput flInput;
    FlashLightInput.FlashLightInputControllerActions FLActions;

    private void Awake()
    {

        flInput = new FlashLightInput();
        FLActions = flInput.FlashLightInputController;

        FLActions.openclose.performed += ctx => SwitchFlashLight();
        FLActions.reload.performed += ctx => CheckForReloadingBattery();
    }

    private void OnEnable()
    {
        FLActions.Enable();
    }
    private void OnDisable()
    {
        FLActions.Disable();
    }

    private void Start()
    {

        player = FindObjectOfType<SUPERCharacterAIO>();
        flashlight.enabled = false;
        flashlight.intensity = DefaultIntensity;
        maxTimeToDecrease = 3;
        arm = GetComponent<ArmsController>();
        volumetricLight = flashlight.GetComponent<VolumetricLightBeam>();
        volumetricLight.intensityGlobal = 0;
        InitialiseFlashLightController();

    }



    private void OnDestroy()
    {
        
        this.gameObject.SetActive(false);
    }


    public void InitialiseFlashLightController()
    {
        audios = GetComponent<AudioSource>();


        audios.playOnAwake = false;
        audios.loop = false;

        player = FindObjectOfType<SUPERCharacterAIO>();
        isFlashlightEquiped = false;
        flashlight.enabled = false;
        
        HUD.instance.ChangeCrossHair(HUD.crosshair_type.normal);
        FlashLightEquiped();
        
        ManageTheLight();
        
        isInit = true;
    }


    public void CheckForReloadingBattery()
    {
        if (Inventory.instance.isInventoryOpen) { return; }
        if (player.fpscam.currentItem != null) { return; }

        if (HudFlashLight.instance.currentBatery <= 0)
        {
            if (HudFlashLight.instance.CheckRemainingBateryInInventory() > 0 )
            {
             
                StartCoroutine(ReloadingBatery());
            }
            else
            {
                ScreenEventsManager.instance.SetVisualMessage("Pas de piles dans votre inventaire", ScreenEventsManager.instance.prf_inventory_message, ScreenEventsManager.instance.gridInventoryMessage);
            }

        }
    }

    public void FlashLightEquiped()
    {

        isFlashlightEquiped = !isFlashlightEquiped;
        
        if (!isFlashlightEquiped)
        {
            HudFlashLight.instance.flashlightEquiped = false;
            HudFlashLight.instance.HideHud();
            
        }
        else
        {
            HudFlashLight.instance.flashlightEquiped = true;
            HudFlashLight.instance.ShowHud();
            HudFlashLight.instance.HideInputReload();
        }

    }


    public void StopFx()
    {
        if (stopflicker)
        {
            StopCoroutine("playWithBeam");
            initFx = false;

        }
    }
    public void SwitchFlashLight()
    {
        switch_on = !switch_on;
        AudioM.instance.PlayOneShotClip(AudioM.instance.AudiosHUD, AudioM.instance.hover_clip);

        ManageTheLight();

    }

    public void ResetAll()
    {
        

        if (switch_on)
        {
            SwitchFlashLight();
            FlashLightEquiped();
        }
        else
        {
            FlashLightEquiped();
        }
    }

    public IEnumerator ReloadingBatery()
    {
        isReloading = true;
        StopFx();
        
        if (!audios.isPlaying)
        {
            audios.PlayOneShot(batery_out_clip);
        }
        yield return new WaitForSeconds(batery_out_clip.length);
        HudFlashLight.instance.currentBatery = HudFlashLight.instance.maxBatery;
        
        Inventory.instance.DestroyItemFromInventory(Inventory.instance.getItemByID(5),false);
        arm.anim.SetTrigger("Reload");
        yield return new WaitForSeconds(arm.anim.GetCurrentAnimatorClipInfo(0).Length);
        InputManager.instance.flInputs.FlashLightInputController.openclose.Enable();
        SwitchFlashLight();
        isReloading = false;

    }




    public void FlashLightFX()
    {
        

        if(HudFlashLight.instance.currentBatery <= 25 && HudFlashLight.instance.currentBatery > 5 && !initFx && !isReloading && !stopflicker)
        {
            initFx = true;
            StartCoroutine(playWithBeam());
        }


        if(HudFlashLight.instance.currentBatery <= 0 && initFx)
        {
            initFx = false;
  
        }
        

    }

    public void ManageTheLight()
    {
        if (switch_on)
        {
            if(HudFlashLight.instance.currentBatery > 0)
            {
                
                float val = (float)HudFlashLight.instance.currentBatery / (float)HudFlashLight.instance.maxBatery;
                flashlight.color = lightColor.Evaluate(val);
                flashlight.intensity = DefaultIntensity;
                flashlight.enabled = true;
                volumetricLight.intensityGlobal = volumetricLightDefaultIntensity;
            }

        }
        else
        {
            flashlight.enabled = false;
            volumetricLight.intensityGlobal = 0;
            initFx = false;
        }

        if(HudFlashLight.instance.currentBatery <= 0)
        {
            InputManager.instance.flInputs.FlashLightInputController.openclose.Disable();
        }
    }

    public IEnumerator playWithBeam()
    {
        

        while (initFx)
        {
            yield return new WaitForSeconds(0.1f);
            volumetricLight.intensityGlobal = Mathf.Lerp(volumetricLight.intensityGlobal, 0, Time.deltaTime / 2);
            flashlight.intensity = Mathf.Lerp(flashlight.intensity, 0, Time.deltaTime * 2);

            if(HudFlashLight.instance.currentBatery <= 0)
            {
                initFx = false;
                volumetricLight.intensityGlobal = 0;
                flashlight.intensity = 0;
            }
        }
        
        yield return null;
    }



    void Update()
    {

        if (switch_on)
        {
            timer += Time.deltaTime;
            FlashLightFX();
        }

        if (isFlashlightEquiped)
        {
            if(HudFlashLight.instance.currentBatery <= 0)
            {
                HudFlashLight.instance.ShowInputReload();
            }
            else
            {
                HudFlashLight.instance.HideInputReload();
            }
        }

        if (timer >= maxTimeToDecrease && switch_on)
        {
           HudFlashLight.instance.currentBatery -= bateryValueDecrease;
            timer = 0;
           float val = (float)HudFlashLight.instance.currentBatery / (float)HudFlashLight.instance.maxBatery;
            flashlight.color = lightColor.Evaluate(val);
            if (HudFlashLight.instance.currentBatery <= 0)
            {
                HudFlashLight.instance.currentBatery = 0;
                SwitchFlashLight();
            }
        }
        
    }
}
