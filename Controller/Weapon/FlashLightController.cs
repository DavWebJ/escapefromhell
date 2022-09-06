using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using BlackPearl;
using UnityEngine;
[RequireComponent(typeof(AudioSource))]
public class FlashLightController : MonoBehaviour
{
    [Header("References")]
    private FlashLightInput inputActions;
    public WeaponItem weapon = null;
    [SerializeField] private AudioSource audios = null;
    private Animator animator = null;
    private FirstPersonAIO player;

    [Header("FlashLight parameter")]
    [SerializeField] public Light flashlight;
    public float DefaultIntensity = 8f;
    public int batery_remain = 0;
    public bool isFlashlightOn = false;
    public float timerLight = 0;
    public bool isReloading = false;
    public string weaponItemName;
    public bool isInit = false;

    private void Awake()
    {
        inputActions = new FlashLightInput();
        HUDWeapon.instance.GetWeaponInfos(null);

 

        if (!GameManager.instance.CheckHUD)
        {
            inputActions.FlashLightInputController.openclose.performed += ctx => OpenCloseFlashLight();
        }

        inputActions.FlashLightInputController.reload.performed += ctx => CheckForReloadingBattery();
    }

    private void OnEnable()
    {
        inputActions.FlashLightInputController.Enable();
        
    }

    private void OnDisable()
    {
        inputActions.FlashLightInputController.Disable();

    }

    private void OnDestroy()
    {
        this.gameObject.SetActive(false);
    }

    public void InitialiseFlashLightController()
    {
        audios = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();

        audios.playOnAwake = false;
        audios.loop = false;

        player = FindObjectOfType<FirstPersonAIO>();
        isFlashlightOn = false;
        flashlight.enabled = false;
        
        audios.PlayOneShot(weapon.fl_equiped_sound);
        HUD.instance.ChangeCrossHair(HUD.crosshair_type.normal);
        flashlight.intensity = DefaultIntensity;
        HUDWeapon.instance.isFlashlight = true;

        HUDWeapon.instance.GetWeaponInfos(weapon);
        isInit = true;
    }

    public void SetItem(Item item)
    {

        if (item.itemType != ItemType.FlashLight)
            return;
        weapon = item as WeaponItem;



        InitialiseFlashLightController();

    }

    public void CheckForReloadingBattery()
    {
        if (weapon.batery <= 0 && !player.fpscam.isInterracting && !GameManager.instance.CheckHUD)
        {
            if (weapon.batery <= 0 && batery_remain > 0 )
            {
                InputManager.instance.inputs.UI.Disable();
                StartCoroutine(ReloadingBatery());
            }

        }
    }

    public void OpenCloseFlashLight()
    {
        if (weapon.batery <= 0) return;

        isFlashlightOn = !isFlashlightOn;
        SwitchFlashLight(isFlashlightOn);

    }

    public void SwitchFlashLight(bool activate)
    {
        if (!audios.isPlaying)
            audios.PlayOneShot(weapon.on_off_clip);

        if (activate)
        {

            flashlight.enabled = true;

        }
        else
        {

            flashlight.enabled = false;

        }


    }



    public IEnumerator ReloadingBatery()
    {
        isReloading = true;
        animator.SetTrigger("hide");
        if (!audios.isPlaying)
        {
            audios.PlayOneShot(weapon.batery_out_clip);
        }
        yield return new WaitForSeconds(weapon.batery_out_clip.length);
        weapon.batery = weapon.batery_max;
        Inventory.instance.UpdateConsumableInInventory("Batery", 1);
        if (!audios.isPlaying)
        {
            audios.PlayOneShot(weapon.fl_reload_clip);
        }
        yield return new WaitForSeconds(weapon.fl_reload_clip.length);
        animator.Play("Get");

        HUDWeapon.instance.Ui_Batery(weapon.batery, weapon.batery_max);
        flashlight.intensity = DefaultIntensity;
        isReloading = false;
        InputManager.instance.inputs.UI.Enable();

    }

    public void UpdateIcon()
    {
        HUDWeapon.instance.GetWeaponInfos(weapon);
        if (weapon.batery > 0)
        {
            HUDWeapon.instance.batery.SetActive(true);
            HUDWeapon.instance.icon.sprite = HUDWeapon.instance.batery_normal;
        }
        else
        {
            HUDWeapon.instance.batery.SetActive(false);
            HUDWeapon.instance.icon.sprite = HUDWeapon.instance.batery_empty;
        }
    }

    public void CheckMessage()
    {
        if (!isFlashlightOn && weapon.batery > 0)
        {
            
            HUDWeapon.instance.canUseFlashLight = true;
            HUDInfos.instance.ReloadInput(false);
        }
        else if (!isFlashlightOn && weapon.batery <= 0 && batery_remain > 0)
        {
            HUDWeapon.instance.canUseFlashLight = false;
            HUDInfos.instance.ReloadInput(true);
            HUDWeapon.instance.ShowReload(false, "");
            // HUDWeapon.instance.ShowReload(true,"Appuyer sur " + GameManager.instance.input.input_reload+ " pour recharger votre lampe torche");

        }
        else if (!isFlashlightOn && weapon.batery <= 0 && batery_remain <= 0)
        {
            HUDWeapon.instance.canUseFlashLight = false;
            HUDWeapon.instance.ShowReload(true, "Trouver des piles pour recharger votre lampe torche");
        }
        else if (isFlashlightOn && weapon.batery != 0)
        {
            HUDWeapon.instance.canUseFlashLight = true;
            HUDWeapon.instance.ShowReload(false, "");
            HUDInfos.instance.ReloadInput(false);
        }
        else if (isFlashlightOn && weapon.batery <= 0 && batery_remain > 0)
        {
            HUDWeapon.instance.canUseFlashLight = false;
            HUDInfos.instance.ReloadInput(true);
            HUDWeapon.instance.ShowReload(true, "Appuyer sur X pour recharger votre lampe torche");
        }
        else
        {
            HUDWeapon.instance.canUseFlashLight = false;
            // HUDWeapon.instance.ShowReload(true,"Trouver des piles pour recharger votre lampe torche");
        }

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

    // Update is called once per frame
    void Update()
    {
        batery_remain = Inventory.instance.AmountConsumableInInventory("Batery");


        if (isInit && HUDWeapon.instance.isFlashlight)
        {
            updateInputsFlashLight();

            CheckMessage();
            UpdateIcon();



            if (isFlashlightOn && !GameManager.instance.CheckHUD)
            {

                if (weapon.batery > 0)
                {
                    weapon.batery -= weapon.batery_reduce_value * Time.deltaTime;
                    if (weapon.batery <= 20f)
                    {

                        flashlight.intensity = Mathf.Lerp(flashlight.intensity, 0, Time.deltaTime * 0.25f);
                        // LumiereTorche.gameObject.GetComponent<Animation>().Play("BatteryVide");

                    }
                    if (weapon.batery <= 0 && batery_remain > 0)
                    {

                        isFlashlightOn = false;
                        weapon.batery = 0;
                        flashlight.enabled = false;
                        HUDWeapon.instance.batery.SetActive(false);
                        HUDWeapon.instance.icon.sprite = HUDWeapon.instance.batery_empty;
                        isFlashlightOn = false;
                        SwitchFlashLight(false);
                    }

                    if (weapon.batery <= 0 && batery_remain <= 0)
                    {
                        HUDWeapon.instance.ShowReload(false, "");
                        HUDWeapon.instance.batery.SetActive(false);
                        HUDWeapon.instance.icon.sprite = HUDWeapon.instance.batery_empty;
                        flashlight.enabled = false;
                        isFlashlightOn = false;
                        SwitchFlashLight(false);
                    }


                    HUDWeapon.instance.Ui_Batery(weapon.batery, weapon.batery_max);
                }
            }
        }
    }
}
