using System.Collections;
using System.Collections.Generic;
using SUPERCharacter;
using EZCameraShake;
using BlackPearl;
using UnityEngine;

public class EventsTrigger : MonoBehaviour
{
    public bool hasTriggered = false;
    public float shakePower;
    public float smoothness;
    public float fadein;
    public float fadeout;
    private CameraShakeInstance _shakeInstance;
    public AudioClip clip;
    public bool useAnim = false;
    public bool useCam = false;
    public bool useAfraidSound = false;
    public bool playThunder = false;
    public bool useCloseDoor = false;
    public Camera cam;
    public GameObject objectToAnim;
    public bool useShake;
    public float EventsDuration;

    public Animation animationToPlay;
    public Animator anim;
    private SUPERCharacterAIO player;


    
    private void Start()
    {
        _shakeInstance = CameraShaker.Instance.StartShake(shakePower, smoothness, fadein);

        //Immediately make the shake inactive.  
        _shakeInstance.StartFadeOut(0);

        //We don't want our shake to delete itself once it stops shaking.
        _shakeInstance.DeleteOnInactive = true;

        if (useCam)
        {
            cam = GetComponentInChildren<Camera>();
            cam.enabled = false;
        }

        if (objectToAnim)
        {
            anim = objectToAnim.GetComponent<Animator>();
            animationToPlay = objectToAnim.GetComponent<Animation>();
        }

    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player" && !hasTriggered)
        {
            SUPERCharacterAIO _player = other.GetComponent<SUPERCharacterAIO>();
            player = _player;
            player.isInTrigger = true;

            StartCoroutine(StartPausedPlayer(_player, EventsDuration));

            AudioM.instance.PlayOneShotClip(AudioM.instance.BossAudioSource, clip);
            if (useShake)
            {
                useCam = false;
               
                CameraShaker.Instance.isShaked = true;
                CameraShaker.Instance.ShakeOnce(shakePower, smoothness, fadein, fadeout);
            }


            if (useAnim)
            {
                if(objectToAnim != null)
                {
                    anim.enabled = true;
                    animationToPlay.Play();
                }
            }

            if (playThunder)
            {
                ThunderManager.instance.PlayThunder();
            }

            if (useCam)
            {
                cam.transform.position = player.playerCamera.transform.position;
                cam.enabled = true;
                player.playerCamera.enabled = false;
                
            }

            hasTriggered = true;

        }
    }

    public IEnumerator StartPausedPlayer(SUPERCharacterAIO player,float time)
    {
        player.PausePlayer(PauseModes.FreezeInPlace);
        yield return new WaitForSeconds(time);
        if(useCloseDoor)
            AudioM.instance.PlayOneShotClip(AudioM.instance.FxAudioSource, AudioM.instance.closeDoor);
        player.UnpausePlayer();
        if (useCam)
        {
            useCam = false;
            cam.enabled = false;
        }

        player.isInTrigger = false;
        player.playerCamera.enabled = true;
        DestroyTrigger();
        yield break;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            SUPERCharacterAIO player = other.GetComponent<SUPERCharacterAIO>();
            player.isInTrigger = false;
            DestroyTrigger();

        }
    }

    public void DestroyTrigger()
    {
        CameraShaker.Instance.isShaked = false;
        if(useAfraidSound)
            AudioM.instance.PlayOneShotClip(AudioM.instance.FxAudioSource, AudioM.instance.afraid);
        if (hasTriggered)
        {
            hasTriggered = false;
            Destroy(objectToAnim);
            Destroy(gameObject, 1);
            
        }
            
    }

    private void FixedUpdate()
    {
        if (useCam && cam.enabled && hasTriggered)
        {
            cam.transform.LookAt(objectToAnim.transform);
        }
        else
        {
            if (hasTriggered)
            {
                player.playerCamera.transform.LookAt(objectToAnim.transform);
            }
        }
    }
}
