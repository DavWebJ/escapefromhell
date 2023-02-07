using System.Collections;
using System.Collections.Generic;
using SUPERCharacter;
using UnityEngine.InputSystem;
using UnityEngine;

public class KnifeController : MonoBehaviour
{
    public GameObject KnifeDamageObject;
    public ArmsController arm;
    public SUPERCharacterAIO player;
    public AxeInput axeInput;
    AxeInput.AxeInputControllerActions axeActions;
    public float cooldown = 0.5f;
    private float timer = 0;
    public bool isInAction = false;



    private void Awake()
    {
        axeInput = new AxeInput();
        axeActions = axeInput.AxeInputController;
        axeActions.Attack.performed += ctx => Attack();

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



        KnifeDamageObject.GetComponent<DamageDealer>().StartDealDamage();


    }



    public void EndDealDamage()
    {

        KnifeDamageObject.GetComponent<DamageDealer>().EndDealDamage();
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
            isInAction = true;
            arm.isInAction = true;
            arm.anim.SetTrigger("Attack");
            int index = Random.Range(0, AudioM.instance.AttackVoicesPossibility.Length);
            if (!AudioM.instance.VoicesAudiosource.isPlaying)
            {

                AudioM.instance.PlayOneShotClip(AudioM.instance.VoicesAudiosource, AudioM.instance.AttackVoicesPossibility[index]);
            }

            timer = Time.time + cooldown;
        }

    }


    public void ResetActions()
    {
        isInAction = false;
        arm.isInAction = false;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
