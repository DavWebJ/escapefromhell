using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using BlackPearl;
[RequireComponent(typeof(AudioSource))]

public class WeaponController : MonoBehaviour
{
    public static WeaponController instance;
    [Header("References")]
    public GunInput inputActions;
    public WeaponItem weapon = null;
    [SerializeField] private AudioSource audios = null;
    private Animator animator = null;
    public string weaponItemName;
    




    [Header("Properties weapon")]
    
    private float timer = 0;
    [SerializeField] private Transform muzzle = null;
    [SerializeField] private Transform sightPosition = null;
    [SerializeField] private Transform parentsModels = null;
    public Vector3 parentsModelOrigin = new Vector3();

    public bool isAiming = false;
    public bool canreload;
    public bool isReloading = false;
    private FirstPersonAIO player;
    public bool canFire => !isReloading && !GameManager.instance.CheckHUD && player.IsGrounded && !player.isSprinting;
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




    private void Awake()
    {

        if(instance == null)
        {
            instance = this;
        }
        inputActions = new GunInput();
       HUDWeapon.instance.GetWeaponInfos(null);
        

        
            inputActions.GunInputController.FireGun.performed += ctx => Fire(isAiming);
            inputActions.GunInputController.FireGun.canceled += ctx => Null();
            inputActions.GunInputController.ReloadGun.performed += ctx => CheckForReloading();
            inputActions.GunInputController.Aim.performed += ctx => isAiming = true;
            inputActions.GunInputController.Aim.canceled += ctx => isAiming = false;
        

    }
    private void Start()
    {


      
    }

    private void OnEnable()
    {
        inputActions.GunInputController.Enable();
        
    }

    private void OnDisable()
    {
        HUDWeapon.instance.GetWeaponInfos(null);
        inputActions.GunInputController.Disable();

    }




    public void InitaliseGunController()
    {

      
        audios = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
        audios.playOnAwake = false;
        audios.loop = false;

        player = FindObjectOfType<FirstPersonAIO>();

        audios.PlayOneShot(weapon.gun_equiped_sound);
      
        fovOrigin = player.fpscam.GetComponent<Camera>().fieldOfView;
        prefab_bullet = Resources.Load<GameObject>("Weapon/FX/Bullet");
    
        muzzle_particle = transform.Find(weapon.pathToMuzzleFx).GetComponentInChildren<ParticleSystem>();
       
        cartridge_particle = transform.Find(weapon.pathToCartridgeFx).GetComponentInChildren<ParticleSystem>();
      
        muzzle = transform.Find(weapon.pathToMuzzleTransform);
       
        parentsModels = transform.Find("parentmodel");
      
        parentsModelOrigin = new Vector3(0, 0, 0);
   
        HUDWeapon.instance.isFlashlight = false;
        HUDWeapon.instance.ShowReload(false, "");



        HUDWeapon.instance.GetWeaponInfos(weapon);
        isInit = true;
       
    }

    public void SetItem(Item item)
    {

            if (item.itemType != ItemType.Weapon)
                return;
            weapon = item as WeaponItem;
        
        
        
        InitaliseGunController();

    }


    public void Null()
    {
        return;
    }

    public void updateGunInputs()
    {
        
        HUD.instance.ChangeCrossHair(HUD.crosshair_type.gun);
        AmmoRemain = Inventory.instance.AmountConsumableInInventory("AmmoGun");
        
        if (AmmoRemain <= 0 && weapon.ammo <= 0)
        {
            AmmoRemain = 0;
            
            HUDWeapon.instance.ShowReload(true,"Trouver des munitions");
        }

        if(weapon.ammo <= 0 && AmmoRemain > 0)
        {
            HUDWeapon.instance.ShowReload(true,"Appuyer sur R pour recharger");
        }

        Vector3 originPos = player.fpscam.armsHolder.localPosition;
        
        HUDWeapon.instance.GetWeaponInfos(weapon);
        Zooming(isAiming);

        if(isAiming && weapon.weaponType == WeaponType.scoped)
        {
            // player.zoomFOV = 20f;
            //StartCoroutine(onScoped());
           
        }else
        {
            // player.zoomFOV = 30f;
            //StartCoroutine(onUncoped());
            
        }
        if(player.IsGrounded && isAiming)
        {
            animator.SetBool(weapon.aim_animation,isAiming);
            
            player.fpscam.armsHolder.localPosition = Vector3.Lerp(player.fpscam.armsHolder.localPosition,weapon.aimingPos,Time.deltaTime * 8);
        }else
        {
            player.fpscam.armsHolder.localPosition = Vector3.Lerp(player.fpscam.armsHolder.localPosition,Vector3.zero,Time.deltaTime * 8);
            animator.SetBool(weapon.aim_animation,false);
        }
        
        if(!player.IsGrounded)
        {
            if(weapon.walk_aim_animation != string.Empty)
            {
                animator.SetBool(weapon.walk_aim_animation,false);
            }
            

            animator.SetBool(weapon.aim_animation,false);
            animator.SetBool(weapon.walk_animation,false);
            animator.SetBool(weapon.sprint_animation,false);

        }

        if(!player.isWalking && !player.isSprinting)
        {
    
            if(weapon.walk_aim_animation != string.Empty)
            {
                animator.SetBool(weapon.walk_aim_animation,false);
            }
            // animator.SetBool(weapon.aim_animation,isAiming);
            animator.SetBool(weapon.walk_animation,false);
            animator.SetBool(weapon.sprint_animation,false);
            
        }

        if(player.isWalking && player.fps_Rigidbody.velocity != Vector3.zero && !isAiming)
        {
    

            if(weapon.walk_aim_animation != string.Empty)
            {
                animator.SetBool(weapon.walk_aim_animation,false);
            }
            animator.SetBool(weapon.walk_animation,true);
            animator.SetBool(weapon.aim_animation,false);
           
           animator.SetBool(weapon.sprint_animation,false);
            
        }
        if(player.isWalking && player.fps_Rigidbody.velocity != Vector3.zero && isAiming)
        {
            if(weapon.walk_aim_animation != string.Empty)
            {
                animator.SetBool(weapon.walk_aim_animation,true);
            }
    
            animator.SetBool(weapon.walk_animation,false);
            // animator.SetBool(weapon.aim_animation,true);
           
           animator.SetBool(weapon.sprint_animation,false);
        }
        if(player.isSprinting && player.fps_Rigidbody.velocity != Vector3.zero)
        {
            if(weapon.walk_aim_animation != string.Empty)
            {
                animator.SetBool(weapon.walk_aim_animation,false);
            }
            animator.SetBool(weapon.walk_animation,false);
            animator.SetBool(weapon.sprint_animation,true);
            animator.SetBool(weapon.aim_animation,false);
       
        }
 



        



    }

    public void CheckForReloading()
    {
        if (!Inventory.instance.isInventoryOpen && !player.fpscam.isInterracting)
        {
            if (weapon.ammo < weapon.max_ammo)
            {
                if (AmmoRemain > 0)
                {
                    //HUDWeapon.instance.ShowReload(true,"Appuyer sur R pour recharger");
                    InputManager.instance.inputs.UI.Disable();
                    StartCoroutine(ReloadingGun());
                    
                }
                else
                {
                    HUDWeapon.instance.ShowReload(false, "");
                    return;

                }
            }
        }
    }

    private void Fire(bool Aiming)
    {
        if (!Inventory.instance.isInventoryOpen)
        {
            // FIRE
            if (weapon.firemode == Firemode.auto)
            {
                weapon.cooldown = weapon.cooldown_auto;
            }
            else
            {
                weapon.cooldown = weapon.cooldown_semi;
            }
            
            if (Time.time > timer && canFire)
            {
                if (weapon.ammo <= 0)
                {

                    if (Aiming)
                    {
                        animator.Play(weapon.shot_aim_animation);
                        audios.PlayOneShot(weapon.empty_clip);
                    }
                    else
                    {
                        animator.Play(weapon.shot_animation);
                        audios.PlayOneShot(weapon.empty_clip);
                    }
                }
                else
                {
                    //FX
                    weapon.ammo--;

                    muzzle_particle.Play();
                    cartridge_particle.Play();
                    audios.PlayOneShot(weapon.fire_clip);

                    if (Aiming)
                    {
                        animator.Play(weapon.shot_aim_animation);
                    }
                    else
                    {
                        animator.Play(weapon.shot_animation);
                    }
                    HUDWeapon.instance.GetWeaponInfos(weapon);
                    Instantiate(prefab_bullet, muzzle.position, muzzle.rotation);
                    player.CamRecoil(weapon.recoil_force);
                }
                timer = Time.time + weapon.cooldown;

            }
        }
    }

    public IEnumerator ReloadingGun()
    {
        
        isReloading = true;
        
        if(!audios.isPlaying)
        {
            audios.PlayOneShot(weapon.gun_reload_clip);
        }
        animator.Play(weapon.reload_animation);
        yield return new WaitForSeconds(weapon.gun_reload_clip.length);
        


        int ammoToReload = weapon.max_ammo - weapon.ammo;
        
        if(ammoToReload >= AmmoRemain)
        {
            ammoToReload = AmmoRemain;
            weapon.ammo = ammoToReload + weapon.ammo;
            
            Inventory.instance.UpdateConsumableInInventory("AmmoGun",ammoToReload);
            
            HUDWeapon.instance.GetWeaponInfos(weapon);
            
        }else
        {
         
            weapon.ammo += ammoToReload;
            Inventory.instance.UpdateConsumableInInventory("AmmoGun",ammoToReload);

          
            
            HUDWeapon.instance.GetWeaponInfos(weapon);
        
            
        }
        HUDWeapon.instance.ShowReload(false,"");
        isReloading = false;
        InputManager.instance.inputs.UI.Enable();


    }


    public IEnumerator onScoped()
    {
        
        yield return new WaitForSeconds(0.35f);
        HUD.instance.setScopedImage(true);
    //    player.fpscam.weapon_cam.SetActive(false);
    }

    public IEnumerator onUncoped()
    {
        // player.fpscam.weapon_cam.SetActive(true);
        yield return null;
        HUD.instance.setScopedImage(false);
        
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

    #region FlashLight
    
    #endregion
    void Update()
    {
        if (isInit)
        {
            updateGunInputs();
            muzzle.LookAt(player.fpscam.targetLook);
        }

           

    }
}
