using System.Collections;
using System.Collections.Generic;
using SUPERCharacter;
using UnityEngine.InputSystem;
using UnityEngine;

public class AxeController : MonoBehaviour
{

    public GameObject axeDamageObject;
    public ArmsController arm;
    public SUPERCharacterAIO player;
    public AxeInput axeInput;
    AxeInput.AxeInputControllerActions axeActions;
    public float cooldown = 0.5f;
    private float timer = 0;
    private void Awake()
    {
        axeInput = new AxeInput();
        axeActions = axeInput.AxeInputController;
  
        axeActions.Attack.performed += ctx => Attack();
        //axeActions.Attack.canceled += ctx => EndDealDamage();
    }

    private void OnEnable()
    {
        axeActions.Enable();
    }

    private void OnDisable()
    {
        axeActions.Disable();
    }
    void Start()
    {
        arm = GetComponent<ArmsController>();
        player = arm.player;
    }


    public void StartDealDamage()
    {



        axeDamageObject.GetComponent<DamageDealer>().StartDealDamage();
        

    }

    public void EndDealDamage()
    {

        axeDamageObject.GetComponent<DamageDealer>().EndDealDamage();
        return;
    }

    public void SetDamage(string type)
    {

    }

    public void PlaySwouch()
    {
        AudioM.instance.PlayOneShotClip(AudioM.instance.FxAudioSource, AudioM.instance.swordClip);
    }


    public void Attack()
    {
        if (arm.isSprinting) { return; }


        if (Time.time > timer)
        {
            arm.anim.SetTrigger("Attack");
            int index = Random.Range(0, AudioM.instance.AttackVoicesPossibility.Length);
            if (!AudioM.instance.VoicesAudiosource.isPlaying)
            {

                AudioM.instance.PlayOneShotClip(AudioM.instance.VoicesAudiosource, AudioM.instance.AttackVoicesPossibility[index]);
            }
            
            timer = Time.time + cooldown;
        }

    }


    void Update()
    {
        
    }
}
