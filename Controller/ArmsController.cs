using System.Collections;
using System.Collections.Generic;
using BlackPearl;
using SUPERCharacter;
using UnityEngine;

public enum ArmType
{
    flashlight,
    gun,
    lighter,
    shotgun,
    axe_one_hand,
    axe_two_hand,
    grenade,
    none

}

public class ArmsController : MonoBehaviour
{
    public SUPERCharacterAIO player;
    public Animator anim;
    public string armName = "";
    public Item armItem;
    public string walkAnim = "Walk";
    public string sprintAnim = "Sprint";
    public string IdleAnim = "Idle";
    public PlayerStat playerStat;
    public bool isSprinting;
    public bool isWalking;
    public bool isIdle;
    public bool isJumping;
    public ArmType armtype;
    public bool startTimer = false;
    public bool canChangeIdle = false;
    public float timer = 0;
    public bool isInAction = false;
    private void Awake()
    {
        armName = armItem.ArmPrefab.name;
        SetArmtype();
    }
    void Start()
    {
        anim = GetComponent<Animator>();
        player = FindObjectOfType<SUPERCharacterAIO>();
        
        
    }

    public void SetAnimation()
    {
        if(GetComponent<WeaponController>() != null)
        {
            WeaponController weapon = GetComponent<WeaponController>();
            //if (weapon.isAiming && !isSprinting)
            //{
            //    anim.SetBool(sprintAnim, false);
            //    anim.SetBool(walkAnim, false);
            //    return;
            //}
        }

        
        // define the animation
        switch (playerStat)
        {
            case PlayerStat.idle:
                anim.SetBool(sprintAnim, false);
                anim.SetBool(walkAnim, false);
                break;
            case PlayerStat.walk:
                anim.SetBool(sprintAnim, false);
                anim.SetBool(walkAnim, true);
                break;
            case PlayerStat.sprint:
                anim.SetBool(sprintAnim, true);
                anim.SetBool(walkAnim, false);
                break;
            case PlayerStat.jump:
                anim.SetBool(sprintAnim, false);
                anim.SetBool(walkAnim, false);
                break;
            default:
                break;
        }
    }

    public void SetArmtype()
    {
        switch (armtype)
        {
            case ArmType.flashlight:
                InputManager.instance.EnableInputFL();
                GetComponent<FlashLightController>().FlashLightEquiped();
                break;
            case ArmType.gun:
                InputManager.instance.EnableInputGun();
                GetComponent<WeaponController>().Gun9mmEquiped();
                break;
            case ArmType.lighter:
                InputManager.instance.DisableAllActions();
                break;
            case ArmType.shotgun:
                InputManager.instance.EnableInputGun();
                GetComponent<ShotGunController>().ShotGunEquiped();
                break;
            case ArmType.axe_one_hand:
                canChangeIdle = true;
                break;
            case ArmType.axe_two_hand:
                InputManager.instance.EnableAxeInput();

                break;
            case ArmType.grenade:
           
                break;
            case ArmType.none:
                InputManager.instance.DisableAllActions();
                break;
            default:
                break;
        }
    }

    public void ResetAllArm()
    {
        InputManager.instance.DisableAllActions();
        switch (armtype)
        {
            case ArmType.flashlight:
       
                GetComponent<FlashLightController>().ResetAll();
                break;
            case ArmType.gun:
      
                GetComponent<WeaponController>().Gun9mmEquiped();
                break;
            case ArmType.lighter:
          
                break;
            case ArmType.shotgun:
                GetComponent<ShotGunController>().ShotGunEquiped();
                break;
            case ArmType.axe_one_hand:
                
                break;
            case ArmType.axe_two_hand:
                
                break;
            case ArmType.grenade:
                break;
            case ArmType.none:
                break;
            default:
                break;
        }
    }

    public void PlayHideAnimation()
    {
        ResetAllArm();
        anim.SetTrigger("Hide");
    }

    public void PlayInitAnimation()
    {
        anim.Play("Get");
    }


    public void HideArms()
    {
        gameObject.SetActive(false);
    }

   
    void Update()
    {

        if (canChangeIdle)
        {
            if (isIdle && !isInAction)
            {
                startTimer = true;
            }
            else
            {
                startTimer = false;
                timer = 0;
            }
            
        }

        if (startTimer)
        {
            timer += Time.deltaTime;

            if(timer >= 10 && isIdle)
            {
                anim.SetTrigger("changeIdle");
                timer = 0;
            }
            
        }
        isSprinting = player.isSprinting;
        isWalking = !player.isSprinting && !player.isIdle;
        isIdle = player.isIdle;
        isJumping = player.jumpInput;

        if (isIdle)
        {
            playerStat = PlayerStat.idle;
        }
        else if (isWalking)
        {
            playerStat = PlayerStat.walk;
        }
        else if(isSprinting)
        {
            playerStat = PlayerStat.sprint;
        }
        else
        {
            playerStat = PlayerStat.jump;
        }

        SetAnimation();
    }

   public enum PlayerStat
    {
        idle,
        walk,
        sprint,
        jump
    }
}
