using System.Collections;
using System.Collections.Generic;
using BlackPearl;
using UnityEngine;

public class AnimationManager : MonoBehaviour
{
    public static AnimationManager instance;

    public Animator animator;
    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void SpellHealAnimation()
    {
        if(animator != null)
        {
            
            animator.SetTrigger("SpellHeal");
        }
    }

    public void PlayHealSound()
    {
        if (!AudioM.instance.FxAudioSource.isPlaying)
        {
            AudioM.instance.PlayOneShotClip(AudioM.instance.FxAudioSource, AudioM.instance.heal);

        }
        
    }

    public void PlayTargetAnimation(string targetAnimation, bool isInterracting)
    {
        animator.applyRootMotion = isInterracting;
        animator.SetBool("isInterracting", isInterracting);
        animator.CrossFade(targetAnimation, 0.2f);
    }

    public void PlayTargetAnimationWithRootRotation(string targetAnimation, bool isInterracting)
    {
        animator.applyRootMotion = isInterracting;
        animator.SetBool("isInterracting", isInterracting);
        animator.SetBool("isRotateWithRootMotion", true);
        animator.CrossFade(targetAnimation, 0.2f);
    }



}
