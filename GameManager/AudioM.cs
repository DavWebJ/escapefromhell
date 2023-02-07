using System.Collections;
using System.Collections.Generic;
using UnityEngine.Audio;
using UnityEngine;
// using BlackPearl;

public enum AudioBackGroundType
{
    Shop,
    Outdoor,
    Victory,
    Chase,
    Boss,
    Town,
    GrassLand,
    GameOver
}
public class AudioM : MonoBehaviour
{
    public static AudioM instance;
    [Header("AudioSource References:")]
    [SerializeField] public AudioSource BossAudioSource;
    [SerializeField] public AudioSource GameOverAudioSource;
    [SerializeField] public AudioSource PickupAudioSource;
    [SerializeField] public AudioSource InventoryAudioSource;
    [SerializeField] public AudioSource interractAudioSource;
    [SerializeField] public AudioSource FxAudioSource;
    [SerializeField] public AudioSource VoicesAudiosource;
    [SerializeField] public AudioSource AudiosHUD;
    [SerializeField] public AudioSource objectif_audios;
    [SerializeField] public AudioSource ambientIndoorAudioSource;
    [SerializeField] public AudioSource ambientOutdoorAudioSource;
    [SerializeField] public AudioSource GhostVoicesAudioSource;

    public AudioSource[] listAudio;
    [Header("Parameters:")]
    public AudioBackGroundType audioBackGroundType;
    public float maxvol = 1;
    public float minvol = 0;
    [SerializeField] private float AudioFadeTime;

    [Header("Ambient clip: ")]

    public AudioClip gameOverClip;
    public AudioClip bossClip;
    public AudioClip ambientOutdoorClip, ambientIndoorClip;

    public bool canPlayNewSound;
    public bool canPlayGhostVoices = true;


    public float timer;
    public float TransitionTimeBetwwenVoices;
    public float minTransitionTimeBetwwenVoices = 25;
    public float maxTransitionTimeBetwwenVoices = 60;
    public int index;


    [Header("HUD clip:")]
    public AudioClip hover_clip, openHudClip, selectItemClip,inventory_full_clip,noBackPack_equiped_clip;

    
    

    [Header("objectif clip:")]
    public AudioClip newObjectifClip, ValideObjectif;


    [Header("Interract Clip")]
    public AudioClip pickUpClip,lockedDoor,unLockedDoor,openDoor,closeDoor,openDraw,closedraw;


    [Header("FX Clip")]
    public AudioClip swordClip;
    public AudioClip equipeClip;
    public AudioClip heal;

    [Header("Voices Player")]
    public AudioClip AttackVoices, SpellCastVoices, Pain,Die,Jump;
    public AudioClip[] AttackVoicesPossibility;
    public AudioClip[] GhostVoicesPossibility;


    [Header("player Clip")]
    public AudioClip drink, eat, pills, seringue,exhal,afraid;


    private void Awake() {
        if(instance == null)
        {
            instance = this;
        }
    
    }
    private void Start() {
        
        ResetAllSound();
        ResetVolume();
        InitClipForAudioSource();
        PlayDefaultAmbientBackground();

    }


    /// <summary>
    /// Init all clip for audiosource static
    /// </summary>
    public void InitClipForAudioSource()
    {

        BossAudioSource.loop = true;


        GameOverAudioSource.clip = gameOverClip;
        BossAudioSource.clip = bossClip;

        ambientIndoorAudioSource.clip = ambientIndoorClip;
        ambientOutdoorAudioSource.clip = ambientOutdoorClip;

        ambientIndoorAudioSource.loop = true;
        ambientOutdoorAudioSource.loop = true;

    }

    public void StopAllSound(AudioSource source)
    {
       
        StartCoroutine(SmoothStopLevelVolume(source));
    }

    public IEnumerator SmoothStopLevelVolume(AudioSource source)
    {
        float timeToFade = 3f;
        float timeElapsed = 0;
       
        if (source.isPlaying)
        {
            while (timeElapsed < timeToFade)
            {
                source.volume = Mathf.Lerp(source.volume, 0, timeElapsed / timeToFade);
                timeElapsed += Time.deltaTime;
                yield return null;
            }



        }


    }

    /// <summary>
    /// reset all volume to 1
    /// </summary>
    public void ResetVolume()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            AudioSource source = transform.GetChild(i).GetComponent<AudioSource>();
            source.volume = 1;
        }
    }

    /// <summary>
    /// stop all coroutine and stop all audiosource and reset boolean
    /// </summary>
    public void ResetAllSound()
    {
        
        StopAllCoroutines();
        for (int i = 0; i < transform.childCount; i++)
        {
            AudioSource source = transform.GetChild(i).GetComponent<AudioSource>();
            source.Stop();
            source.volume = 1;
            source.playOnAwake = false;
            source.loop = false;

        }


 
    }

    /// <summary>
    /// play a one shot clip for dynamique sound
    /// </summary>
    /// <param name="source"></param>
    /// <param name="clip"></param>
    public void PlayOneShotClip(AudioSource source, AudioClip clip)
    {
        if(source.volume < 1)
        {
            source.volume = 1;
        }
        source.loop = false;
        if (!source.isPlaying)
        {
            source.PlayOneShot(clip);
        }
        
    }

    public AudioSource GetCurrentAudioSource()
    {
        AudioSource currentAS = null;
        for (int i = 0; i < listAudio.Length; i++)
        {
            if (listAudio[i].isPlaying)
            {
                currentAS = listAudio[i];
            }
            
        }
        return currentAS;
        
        
    }


    /// <summary>
    /// set the background type with a switch and play the good sound
    /// can be edit and custom with over backgroundtype
    /// </summary>
    /// <param name="backGroundType"></param>
    public void SetBackGroundAudioType(AudioBackGroundType backGroundType)
    {
       
        switch (backGroundType)
        {
            case AudioBackGroundType.Shop:
                audioBackGroundType = AudioBackGroundType.Shop;
                //play shop
                break;
            case AudioBackGroundType.Outdoor:
                audioBackGroundType = AudioBackGroundType.Outdoor;
                PlayOutDoorSound();
                break;
            case AudioBackGroundType.Boss:
                audioBackGroundType = AudioBackGroundType.Boss;
                PlayBossSound();
                break;
            case AudioBackGroundType.GrassLand:
                audioBackGroundType = AudioBackGroundType.GrassLand;
                break;
            case AudioBackGroundType.GameOver:
                audioBackGroundType = AudioBackGroundType.GameOver;
                ResetAllSound();
                PlayGameOverSound();
                break;
            default:
                break;
        }
        
    }
    public void PlayGameOverSound()
    {
        if (!GameOverAudioSource.isPlaying)
        {
            ResetAllSound();
            PlayOneShotClip(GameOverAudioSource, gameOverClip);
        }
    }

    public void PlayBossSound()
    {
        if (!BossAudioSource.isPlaying && canPlayNewSound)
        {

            StartCoroutine(PlayClipSmoothLevelVolume(BossAudioSource));
        }
    }


    public void PlayLockedDoor()
    {
        if (!objectif_audios.isPlaying)
        {
            PlayOneShotClip(objectif_audios, lockedDoor);
        }
    }
    public void PlayOpenDraw()
    {
        if (!objectif_audios.isPlaying)
        {
            PlayOneShotClip(objectif_audios, openDraw);
        }
    }
    public void PlayCloseDraw()
    {
        if (!objectif_audios.isPlaying)
        {
            PlayOneShotClip(objectif_audios, closedraw);
        }
    }


    public void PlaySwordAction()
    {
        FxAudioSource.loop = false;
        if (!FxAudioSource.isPlaying)
        {
            if(FxAudioSource.volume < 1f)
            {
                FxAudioSource.volume = 1;
            }
            
            PlayOneShotClip(FxAudioSource,swordClip);
        }

    }
    public void PlayHUDHoverClip()
    {

        
        if (!AudiosHUD.isPlaying)
        {
            PlayOneShotClip(AudiosHUD, hover_clip);
            
        }
    }


    public void PlayOutDoorSound()
    {

        //if (!OutdoorAudioSource.isPlaying && canPlayNewSound)
        //{
        //    StartCoroutine(PlayClipSmoothLevelVolume(OutdoorAudioSource));
        //}
        

    }

    public void PlayNoBackPack()
    {
        AudiosHUD.loop = false;
        AudiosHUD.clip = noBackPack_equiped_clip;
        AudiosHUD.Play();
    }

    public void InventoryFull()
    {
        AudiosHUD.loop = false;
        AudiosHUD.clip = inventory_full_clip;
        AudiosHUD.Play();
    }

    public void PlayNewObjectif()
    {
        AudiosHUD.loop = false;
        AudiosHUD.clip = newObjectifClip;
        AudiosHUD.Play();
    }

    public void PlayAmbientBackground(AudioClip clip)
    {

        StopAllCoroutines();

        ambientIndoorAudioSource.Play();




    }

    public void PlayDefaultAmbientBackground()
    {
        PlayAmbientBackground(ambientOutdoorClip);
    }

    public void Playjump()
    {
        PlayOneShotClip(FxAudioSource, Jump);
    }


    public void PlayOutDoorBackGround()
    {
        //if (!OutdoorAudioSource.isPlaying && canPlayNewSound)
        //{

        //    StartCoroutine(PlayClipSmoothLevelVolume(OutdoorAudioSource));
        //}
    }

    public void PlayPickUp()
    {
        if(!AudiosHUD.isPlaying)
            PlayOneShotClip(AudiosHUD, pickUpClip);
    }

    public void PlayEquiped()
    {
        if (!AudiosHUD.isPlaying)
        {
            PlayOneShotClip(AudiosHUD, equipeClip);
        }
    }





    

    public IEnumerator PlayClipSmoothLevelVolume(AudioSource newAudioSource)
    {
        float timeToFade = 3f;
        float timeElapsed = 0;
        AudioSource currentAudioSource = GetCurrentAudioSource();
    
        newAudioSource.volume = 0;

        if (currentAudioSource != null)
        {
           
            if (canPlayNewSound)
            {
                
                canPlayNewSound = false;
                newAudioSource.Play();
                while (timeElapsed < timeToFade)
                {

                    newAudioSource.volume = Mathf.Lerp(minvol, maxvol, timeElapsed / timeToFade);
                    currentAudioSource.volume = Mathf.Lerp(maxvol, minvol, timeElapsed / timeToFade);
                    timeElapsed += Time.deltaTime;
                    yield return null;
                }
            }
           
            currentAudioSource.Stop();
            canPlayNewSound = true;

        
            
        }

   

    }

    public IEnumerator PlayGhostVoice()
    {
        canPlayGhostVoices = false;
        TransitionTimeBetwwenVoices = Random.Range(minTransitionTimeBetwwenVoices, maxTransitionTimeBetwwenVoices);
        index = Random.Range(0, GhostVoicesPossibility.Length);
        PlayOneShotClip(GhostVoicesAudioSource, GhostVoicesPossibility[index]);
        
        index = 0;
        canPlayGhostVoices = true;
        yield break;
    }


    private void Update()
    {
        timer += Time.deltaTime;

        if(timer >= TransitionTimeBetwwenVoices && canPlayGhostVoices)
        {
         
                StartCoroutine(PlayGhostVoice());
            timer = 0.0f;
        }
    }



}
