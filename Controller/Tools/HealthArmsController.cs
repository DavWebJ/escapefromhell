using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class HealthArmsController : MonoBehaviour
{

    private Animator animator = null;


    private void Awake()
    {

        animator = GetComponent<Animator>();

    }



    public void Heal()
    {
        AudioM.instance.PlayOneShotClip(AudioM.instance.FxAudioSource, AudioM.instance.seringue);

    }

    public void Hide()
    {
        Destroy(gameObject);
    }
   

}
