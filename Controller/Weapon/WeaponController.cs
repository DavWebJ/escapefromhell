using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SUPERCharacter;
using UnityEngine.InputSystem;
using BlackPearl;
[RequireComponent(typeof(AudioSource))]

public class WeaponController : MonoBehaviour
{


    [SerializeField] private AudioSource audios = null;

    public AudioClip gun_reload_clip, empty_clip, fire_clip;

    public Transform spawn;

    public ArmsController arm;

    public GunInput gunInput;
    GunInput.GunInputControllerActions gunActions;

    [Header("Properties weapon")]
    
    private float timer = 0;
    [SerializeField] private Transform muzzle = null;
    [SerializeField] private Transform sightPosition = null;
    [SerializeField] private Transform parentsModels = null;
    public Vector3 parentsModelOrigin = new Vector3();

    public bool isAiming = false;
    public bool canreload;
    public bool isReloading = false;
    public bool isGunEquiped = false;
    public SUPERCharacterAIO player;
    public bool canFire;
    public float fovOrigin;
    public float fov_zooming = 15;

    [Header("FX particule")]
    public ParticleSystem muzzle_particle = null;
    public ParticleSystem cartridge_particle = null;

    [Header("Prefab")]
    [SerializeField] private GameObject prefab_bullet = null;
    [Header("Ammo Count:")]
    public int AmmoRemain = 0;
    public bool isInit = false;

    public bool isFiring = false;
    public Vector3 aimingPos;


    public float cooldown = 0.3f;


    private void Awake()
    {

        gunInput = new GunInput();
        gunActions = gunInput.GunInputController;

        //HUDWeapon.instance.GetWeaponInfos(null);
        
    }

    private void OnEnable()
    {
        gunActions.Enable();
    }
    private void OnDisable()
    {
        gunActions.Disable();
    }
    private void Start()
    {
        arm = GetComponent<ArmsController>();
        player = arm.player;
        InitaliseGunController();
        gunActions.FireGun.performed += ctx => Fire();
        gunActions.FireGun.canceled += ctx => Null();
        gunActions.ReloadGun.performed += ctx => CheckForReloading();
        gunActions.Aim.performed += ctx => isAiming = !arm.isSprinting;
        gunActions.Aim.canceled += ctx => isAiming = false;
        canFire = !isReloading && !arm.isSprinting;
    }



    public void InitaliseGunController()
    {

      
        audios = GetComponent<AudioSource>();

        audios.playOnAwake = false;
        audios.loop = false;
      
        fovOrigin = player.fpscam.GetComponent<Camera>().fieldOfView;
        prefab_bullet = Resources.Load<GameObject>("Weapon/FX/Bullet");
        parentsModels = transform.Find("parentmodel");
        muzzle_particle = parentsModels.transform.Find("Hp_Base/muzzle").GetComponentInChildren<ParticleSystem>();
       
        cartridge_particle = parentsModels.transform.Find("Hp_Base/cartridge").GetComponentInChildren<ParticleSystem>();
      
        muzzle = parentsModels.transform.Find("Hp_Base/muzzle").transform;

        spawn = parentsModels.transform.Find("Hp_Base/Bn_Trigger/spawn").transform;
        
      
        parentsModelOrigin = new Vector3(0, -1.7f, 0);
        aimingPos = new Vector3(0, -1.7f, 0);

        HUD.instance.ChangeCrossHair(HUD.crosshair_type.gun);
        
        isInit = true;
       
    }


    public void Gun9mmEquiped()
    {
        isGunEquiped = !isGunEquiped;

        if (!isGunEquiped)
        {
            HUDWeapon.instance.gunEquiped = false;
            HUDWeapon.instance.HideHudAmmo();
            HUDWeapon.instance.HideReloadInput();

        }
        else
        {
            HUDWeapon.instance.gunEquiped = true;
            HUDWeapon.instance.ShowHudAmmo();
            HUDWeapon.instance.ShowReloadInput();
        }
    }
    public void Null()
    {
        isFiring = false;
        arm.anim.SetBool("isFire", isFiring);
        return;
    }

    public void updateGunInputs()
    {
        
        if(arm.isSprinting) {
            arm.anim.SetBool("isAiming", false);
             return; }

        HUD.instance.ChangeCrossHair(HUD.crosshair_type.gun);


        Vector3 originPos = player.fpscam.armsHolder.localPosition;
        
    
        Zooming(isAiming && !arm.isSprinting);

        //arm.anim.SetBool("isAiming",isAiming);



            arm.anim.SetBool("isAiming", isAiming);
        


            if (isAiming)
            {
                player.fpscam.armsHolder.localPosition = Vector3.Lerp(player.fpscam.armsHolder.localPosition, aimingPos, Time.deltaTime * 8);
            }
            else
            {
                player.fpscam.armsHolder.localPosition = Vector3.Lerp(player.fpscam.armsHolder.localPosition, parentsModelOrigin, Time.deltaTime * 8);
                arm.anim.SetBool("isAiming", isAiming);

            }

        



    }



    public void CheckForReloading()
    {
        if(player.fpscam.currentItem != null) { return; }

        if (!Inventory.instance.isInventoryOpen)
        {
            if (HUDWeapon.instance.currentAmmo < HUDWeapon.instance.maxAmmo)
            {
                if (HUDWeapon.instance.CheckRemainingAmmoGunInInventory() > 0)
                {

                    StartCoroutine(ReloadingGun());
                    
                }
                else
                {
                    ScreenEventsManager.instance.SetVisualMessage("Plus de munition dans votre inventaire", ScreenEventsManager.instance.prf_inventory_message, ScreenEventsManager.instance.gridInventoryMessage);
                    

                }
            }
        }
    }

    private void Fire()
    {
        
        if (!Inventory.instance.isInventoryOpen)
        {

            if (Time.time > timer && canFire)
            {
                isFiring = true;
                if (HUDWeapon.instance.currentAmmo <= 0)
                {
                    
                    if (isAiming)
                    {
                 
                        arm.anim.SetBool("isFire",isFiring);
                        arm.anim.SetBool("isAiming", true);
                        audios.PlayOneShot(empty_clip);
                    }
                    else
                    {
                 
                        arm.anim.SetBool("isFire",isFiring);
                        arm.anim.SetBool("isAiming",false);
                        audios.PlayOneShot(empty_clip);
                    }
                }
                else
                {

                    HUDWeapon.instance.currentAmmo--;

                    muzzle_particle.Play();
                    cartridge_particle.Play();
                    audios.PlayOneShot(fire_clip);

                    if (isAiming)
                    {
                        arm.anim.SetBool("isFire", isFiring);
                        arm.anim.SetBool("isAiming", true);
                    }
                    else
                    {
                        arm.anim.SetBool("isFire", isFiring);
                        arm.anim.SetBool("isAiming", false);

                    }
             
                    Instantiate(prefab_bullet, spawn.position, spawn.rotation);
                    RecoilWeapon recoilFx = GetComponent<RecoilWeapon>();
                    recoilFx.Recoil();
                }
                //Null();
                timer = Time.time + cooldown;
               

            }
        }
    }

    public IEnumerator ReloadingGun()
    {
        
        isReloading = true;
        InputManager.instance.DisableAllActions();
       

        audios.PlayOneShot(gun_reload_clip);
        
        arm.anim.Play("Recharge");
        yield return new WaitForSeconds(arm.anim.GetCurrentAnimatorClipInfo(0).Length);
        


        int ammoToReload = HUDWeapon.instance.maxAmmo - HUDWeapon.instance.currentAmmo;
        AmmoRemain = HUDWeapon.instance.CheckRemainingAmmoGunInInventory();
        if (ammoToReload >= AmmoRemain)
        {
            ammoToReload = AmmoRemain;
            Inventory.instance.DestroyItemFromInventoryWithAmount(Inventory.instance.getItemByname("ammo_gun"), ammoToReload);
            
            HUDWeapon.instance.currentAmmo = ammoToReload + HUDWeapon.instance.currentAmmo;

            
        }else
        {
            Inventory.instance.DestroyItemFromInventoryWithAmount(Inventory.instance.getItemByname("ammo_gun"), ammoToReload);
            HUDWeapon.instance.currentAmmo += ammoToReload;
          
          
            
   
        
            
        }
        
        isReloading = false;
        InputManager.instance.EnableInputGun();
        // InputManager.instance.inputs.UI.Enable();
        yield break;

    }


    public IEnumerator onScoped()
    {
        
        yield return new WaitForSeconds(0.35f);
       // HUD.instance.setScopedImage(true);
    //    player.fpscam.weapon_cam.SetActive(false);
    }

    public IEnumerator onUncoped()
    {
        // player.fpscam.weapon_cam.SetActive(true);
        yield return null;
       // HUD.instance.setScopedImage(false);
        
    }

    public void Zooming(bool isZooming)
    {
        
        if(isZooming)
        {
            player.fpscam.cam.fieldOfView = Mathf.Lerp(player.fpscam.cam.fieldOfView,fovOrigin - fov_zooming,Time.deltaTime * 8);
        }else
        {
            
            player.fpscam.cam.fieldOfView = Mathf.Lerp(player.fpscam.cam.fieldOfView,fovOrigin,Time.deltaTime * 8);
        }
    }

    void Update()
    {
        if (isInit && isGunEquiped)
        {
            updateGunInputs();
            muzzle.LookAt(player.fpscam.targetLook);
        }

           

    }
}
