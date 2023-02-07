using System.Collections;
using System.Collections.Generic;
using SUPERCharacter;
using UnityEngine.InputSystem;
using BlackPearl;
using UnityEngine;

public class ShotGunController : MonoBehaviour
{
    [SerializeField] private AudioSource audios = null;

    public AudioClip shotgun_reload_clip, empty_clip, fire_clip,cartridge_clip;

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
    public bool isShotGunEquiped = false;
    public SUPERCharacterAIO player;
    public bool canFire;
    public float fovOrigin;
    public float fov_zooming = 15;

    [Header("FX particule")]
    public GameObject muzzle_particle = null;
    public ParticleSystem cartridge_particle = null;

    [Header("Prefab")]
    [SerializeField] private GameObject prefab_bullet = null;
    [Header("Ammo Count:")]
    public int AmmoRemain = 0;
    public bool isInit = false;

    public bool isFiring = false;
    public Vector3 aimingPos;


    public float cooldown = 0.7f;



    private void Awake()
    {

        gunInput = new GunInput();
        gunActions = gunInput.GunInputController;
    }

    private void OnEnable()
    {
        gunActions.Enable();
    }
    private void OnDisable()
    {
        gunActions.Disable();
    }
    void Start()
    {
        arm = GetComponent<ArmsController>();
        player = arm.player;
        InitaliseShotGunController();
        gunActions.FireGun.performed += ctx => Fire();
        gunActions.FireGun.canceled += ctx => Null();
        gunActions.ReloadGun.performed += ctx => CheckForReloading();
        gunActions.Aim.performed += ctx => isAiming = !arm.isSprinting;
        gunActions.Aim.canceled += ctx => isAiming = false;
        canFire = !isReloading && !arm.isSprinting;
    }

    public void InitaliseShotGunController()
    {


        audios = GetComponent<AudioSource>();

        audios.playOnAwake = false;
        audios.loop = false;

        fovOrigin = player.fpscam.GetComponent<Camera>().fieldOfView;
        prefab_bullet = Resources.Load<GameObject>("Weapon/FX/bullet shotgun");
        //parentsModels = transform.Find("parentmodel");
        muzzle_particle = Resources.Load<GameObject>("Weapon/shotgun/Shotgun_fx");

        //cartridge_particle = parentsModels.transform.Find("Hp_Base/cartridge").GetComponentInChildren<ParticleSystem>();

        //muzzle = parentsModels.transform.Find("Hp_Base/muzzle").transform;

        //spawn = parentsModels.transform.Find("Hp_Base/Bn_Trigger/spawn").transform;


        parentsModelOrigin = new Vector3(0, -1.7f, 0);
        aimingPos = new Vector3(0, -1.7f, 0);

        HUD.instance.ChangeCrossHair(HUD.crosshair_type.gun);

        isInit = true;

    }

    public void Null()
    {
        isFiring = false;
        arm.anim.SetBool("isFire", isFiring);
        return;
    }


    public void updateGunInputs()
    {

        if (arm.isSprinting)
        {
            arm.anim.SetBool("isAiming", false);
             return;
        }

        HUD.instance.ChangeCrossHair(HUD.crosshair_type.gun);


        Vector3 originPos = player.fpscam.armsHolder.localPosition;


        Zooming(isAiming && !arm.isSprinting);

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
        if (player.fpscam.currentItem != null) { return; }

        if (!Inventory.instance.isInventoryOpen)
        {
            if (HUDShotgun.instance.currentAmmo < HUDShotgun.instance.maxAmmo)
            {
                if (HUDShotgun.instance.CheckRemainingAmmoShotGunInInventory() > 0)
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
                if (HUDShotgun.instance.currentAmmo <= 0)
                {

                    if (isAiming)
                    {
                        arm.anim.SetTrigger("Fire");
                        arm.anim.SetBool("isAiming", isAiming);
                        audios.PlayOneShot(empty_clip);
                    }
                    else
                    {
                        arm.anim.SetTrigger("Fire");
                        arm.anim.SetBool("isAiming", isAiming);
                        audios.PlayOneShot(empty_clip);
                    }
                }
                else
                {

                    HUDShotgun.instance.currentAmmo--;
                    GameObject fx = Instantiate(muzzle_particle, muzzle.position, Quaternion.identity);

                    
                    //cartridge_particle.Play();
                    //audios.PlayOneShot(fire_clip);

                    if (isAiming)
                    {
                        arm.anim.SetTrigger("Fire");
                        arm.anim.SetBool("isAiming", isAiming);
                    }
                    else
                    {
                        arm.anim.SetTrigger("Fire");
                        arm.anim.SetBool("isAiming", isAiming);

                    }

                    Instantiate(prefab_bullet, spawn.position, spawn.rotation);
                    RecoilWeapon recoilFx = GetComponent<RecoilWeapon>();
                    recoilFx.Recoil();
                    //Destroy(fx);
                }
    
                timer = Time.time + cooldown;

            }
        }
    }


    public void ShotGunEquiped()
    {
        isShotGunEquiped = !isShotGunEquiped;

        if (!isShotGunEquiped)
        {
            HUDShotgun.instance.shotGunEquiped = false;
            HUDShotgun.instance.HideHudAmmo();
            HUDShotgun.instance.HideReloadInput();

        }
        else
        {
            HUDShotgun.instance.shotGunEquiped = true;
            HUDShotgun.instance.ShowHudAmmo();
            HUDShotgun.instance.ShowReloadInput();
        }
    }
    public IEnumerator ReloadingGun()
    {

        isReloading = true;
        InputManager.instance.DisableAllActions();


        
        int ammoToReload = HUDShotgun.instance.maxAmmo - HUDShotgun.instance.currentAmmo;
        AmmoRemain = HUDShotgun.instance.CheckRemainingAmmoShotGunInInventory();







        if (ammoToReload >= AmmoRemain)
        {
            ammoToReload = AmmoRemain;
            Inventory.instance.DestroyItemFromInventoryWithAmount(Inventory.instance.getItemByname("shotgun_ammo"), ammoToReload);

            HUDShotgun.instance.currentAmmo = ammoToReload + HUDShotgun.instance.currentAmmo;


        }
        else
        {
            Inventory.instance.DestroyItemFromInventoryWithAmount(Inventory.instance.getItemByname("shotgun_ammo"), ammoToReload);
            HUDShotgun.instance.currentAmmo += ammoToReload;

        }
        arm.anim.SetTrigger("isReloading");
        
        for (int i = 0; i < ammoToReload; i++)
        {
            arm.anim.SetBool("Pellet",true);
            yield return new WaitForSeconds(arm.anim.GetCurrentAnimatorClipInfo(0).Length);
        }
        arm.anim.SetBool("Pellet", false);
        isReloading = false;
        InputManager.instance.EnableInputGun();
        yield break;

    }

    public void PlayReloadRecovery()
    {
        if(!audios.isPlaying)
            audios.PlayOneShot(empty_clip);
    }

    public void playEnterCartridge()
    {
        if (!audios.isPlaying)
            audios.PlayOneShot(cartridge_clip);
    }

    public void Zooming(bool isZooming)
    {

        if (isZooming)
        {
            player.fpscam.cam.fieldOfView = Mathf.Lerp(player.fpscam.cam.fieldOfView, fovOrigin - fov_zooming, Time.deltaTime * 8);
        }
        else
        {

            player.fpscam.cam.fieldOfView = Mathf.Lerp(player.fpscam.cam.fieldOfView, fovOrigin, Time.deltaTime * 8);
        }
    }
  
    void Update()
    {
        if (isInit && isShotGunEquiped)
        {
            updateGunInputs();
            muzzle.LookAt(player.fpscam.targetLook);
        }
    }
}
