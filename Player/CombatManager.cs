using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SUPERCharacter;
using BlackPearl;
using UnityEngine.InputSystem;

public class CombatManager : MonoBehaviour
{
    public bool isWeaponEquiped = false;
    public bool isAlreadyInAction = false;
    public Animator playerAnimator;
    public GameObject currentEquipedWeapon = null;
    public GameObject weaponHandSlot = null;
    public GameObject holderWeaponSlot = null;
    public VitalState vital;
    public float attackSpeed;
    public float animClipLength;
    public float timePassed;
    public bool isLockOn;
    public Transform targetDetected = null;
    public List<Transform> visibleTargets = new List<Transform>();
    public float viewRadius;
    public float viewAngle;
    public LayerMask EnemyMask;
    public float minAngle = -50;
    public float maxAngle = 50;
    public float distToTarget;
    public float minDistanceToDetectTarget;
    public bool isInvincible;
    public bool isInterracting;

    public bool attack = false;
 
    void Start()
    {
        
        vital = GetComponent<VitalState>();

    }

    private void Update()
    {
        if (isWeaponEquiped)
        {
            timePassed += Time.deltaTime;
        }
        isInvincible = playerAnimator.GetBool("isInvincible");
        isInterracting = playerAnimator.GetBool("isInterracting");
        if (isWeaponEquiped && attack)
        {
            
            animClipLength = playerAnimator.GetCurrentAnimatorClipInfo(1)[0].clip.length;
            attackSpeed = playerAnimator.GetCurrentAnimatorStateInfo(1).speed;

            if (timePassed >= animClipLength / attackSpeed && attack)
            {

                playerAnimator.SetFloat("Speed", 0,0.1f,Time.deltaTime);
                playerAnimator.applyRootMotion = true;
                
            }

            if (timePassed >= animClipLength / attackSpeed)
            {
                playerAnimator.SetTrigger("Move");
                playerAnimator.applyRootMotion = false;
                attack = false;
            }
        }

        if (isLockOn)
        {
            visibleTargets.Clear();
            Collider[] targetInViewRadius = Physics.OverlapSphere(transform.position, viewRadius, EnemyMask);

            for (int i = 0; i < targetInViewRadius.Length; i++)
            {
                Transform targetPos = targetInViewRadius[i].transform;
                Vector3 dirToTarget = (targetPos.position - transform.position).normalized;
                float distToTarget = Vector3.Distance(transform.position, targetPos.position);
                visibleTargets.Add(targetPos);
                if (Vector3.Angle(transform.forward, dirToTarget) < viewAngle / 2)
                {
                    if (distToTarget <= minDistanceToDetectTarget)
                    {
                        targetDetected = targetInViewRadius[i].transform;
                    }
                    else
                    {
                        targetDetected = null;
                    }
                }
            }
        }


    }
    public void OnSwordInputAction(InputAction.CallbackContext context)
    {
        if (isWeaponEquiped && !isAlreadyInAction && !vital.isdead)
        {
            int index = Random.Range(0, AudioM.instance.AttackVoicesPossibility.Length);
            attack = true;
            playerAnimator.applyRootMotion = false;
            timePassed = 0f;
            isAlreadyInAction = true;
            playerAnimator.SetFloat("Speed", 0, 0.1f, Time.deltaTime);
            playerAnimator.SetTrigger("Attack1");
            
            if (!AudioM.instance.VoicesAudiosource.isPlaying)
            {
                
                AudioM.instance.PlayOneShotClip(AudioM.instance.VoicesAudiosource, AudioM.instance.AttackVoicesPossibility[index]);
            }

            

        }
        else
        {
            
            return;
        }
    }

    public void LockOntarget(InputAction.CallbackContext context)
    {
        isLockOn = !isLockOn;

        if (!isLockOn)
        {
            targetDetected = null;
        }



    }

    public void DrawWeapon(InputAction.CallbackContext context)
    {
        if (currentEquipedWeapon == null || isWeaponEquiped) return;

        playerAnimator.SetTrigger("Draw");
        isWeaponEquiped = true;
    }

    public void UnDrawWeapon(InputAction.CallbackContext context)
    {
        if (currentEquipedWeapon == null || !isWeaponEquiped) return;

        playerAnimator.SetTrigger("Undraw");
        isWeaponEquiped = false;
    }

    public void EnableWeaponHandSlot()
    {
        if(weaponHandSlot != null)
        {
          
            weaponHandSlot.SetActive(true);
            holderWeaponSlot.SetActive(false);
            weaponHandSlot.GetComponentInChildren<DamageDealer>().SetDamage(2);
        }
        
    }

    public void DisableWeaponHandSlot()
    {
  
        holderWeaponSlot.SetActive(true);
        weaponHandSlot.SetActive(false);
        
    }

    public void AttackActionIsFinished()
    {

        isAlreadyInAction = false;

    }
    public void PlaySwordActionInputSound()
    {
        AudioM.instance.PlayOneShotClip(AudioM.instance.FxAudioSource, AudioM.instance.swordClip);
    }

    public void SetWeapon(GameObject weapon)
    {
        currentEquipedWeapon = weapon;

        
    }

    public void SetWeaponHandSlot(GameObject handSlot,GameObject _holderWeaponSlot)
    {
        if(handSlot != null)
        {
            weaponHandSlot = handSlot;
        }

        if(_holderWeaponSlot != null)
        {
            holderWeaponSlot = _holderWeaponSlot;
        }
    }
    public void StartDealDamage()
    {
        
        if(currentEquipedWeapon != null)
        {
            weaponHandSlot.GetComponentInChildren<DamageDealer>().StartDealDamage();
        }
            
    }

    public void EndDealDamage()
    {
        if(currentEquipedWeapon != null)
            weaponHandSlot.GetComponentInChildren<DamageDealer>().EndDealDamage();
    }

    public void EnableIsInvincible()
    {
        playerAnimator.SetBool("isInvincible", true);
    }
    public void DisableIsInvincible()
    {
        playerAnimator.SetBool("isInvincible", false);
    }
}
