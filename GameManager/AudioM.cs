using System.Collections;
using System.Collections.Generic;
using UnityEngine.Audio;
using UnityEngine;
// using BlackPearl;


public class AudioM : MonoBehaviour
{
    public static AudioM instance;
    [SerializeField] public AudioSource ambientIndoorAudioSource;
    [SerializeField] public AudioSource ambientOutdoorAudioSource;
    [SerializeField] public AudioSource AudiosHUD;
    [SerializeField] public AudioSource thunder_audios;
    [SerializeField] public AudioSource objectif_audios;
    [SerializeField] public AudioSource screamer_audios;
    [SerializeField] public AudioSource tension_audio;

    public float maxvol = 1;
    public float minvol = 0;
    public float currentVolume;
    [SerializeField] private float AudioFadeTime = 1.5f;
    [Header("Ambient clip: ")]
    public AudioClip ambientIndoorClip;
    public AudioClip ambientOutdoorClip;
    public AudioClip ambientBeforeScreamer;
    [Header("UI clip:")]
    public AudioClip hover_clip, openHudClip, selectItemClip;

    [Header("Thunder outdoor clip")]
    public AudioClip[] thunderclip;
    public bool isPlayingAmbientIndoor;

    [Header("objectif clip:")]
    public AudioClip newObjectifClip, ValideObjectif;
    [Header("light thunder support")]
    public Light lighting;

    public bool isAlreadyPlaying = false;

    public float timer = 0;
    public float thunderTransitionTime = 5;
    public bool enableThunder = true;
    public bool isIntro = false;


    private void Awake() {
        if(instance == null)
        {
            instance = this;
        }

        lighting.enabled = false;


    }
    private void Start() {
        
        currentVolume = ambientIndoorAudioSource.volume;

        ambientIndoorAudioSource.loop = true;
        ambientOutdoorAudioSource.loop = true;
        ambientIndoorAudioSource.playOnAwake = false;
        ambientOutdoorAudioSource.playOnAwake = true;
        AudiosHUD.playOnAwake = false;
        AudiosHUD.loop = false;
        if (!isIntro)
        {
            PlayDefaultAmbientBackground();
           
        }
        else
        {
            thunderTransitionTime = 17;
        }
            


    }

    public void StopAllSound(AudioSource source)
    {
        enableThunder = false;
        StartCoroutine(SmoothStopLevelVolume(source));
    }
    public void PlayHUDHoverClip()
    {
        currentVolume =  ambientIndoorAudioSource.volume;

        if (!AudiosHUD.isPlaying)
        {
            AudiosHUD.PlayOneShot(hover_clip, 0.5f);
            return;
        }
    }

    public void PlayOneShotClip(AudioSource source,AudioClip clip)
    {
        
        source.PlayOneShot(clip);
    }



    public void PlayAmbientBackground(AudioClip clip)
    {

        StopAllCoroutines();
        StartCoroutine(SmoothLevelVolume(clip));



        isPlayingAmbientIndoor = !isPlayingAmbientIndoor;
    }

    public void PlayDefaultAmbientBackground()
    {
        PlayAmbientBackground(ambientOutdoorClip);
    }

    public void PlayScreamerAmbience()
    {
        StopAllCoroutines();
        StartCoroutine(SmoothLevelVolume(ambientBeforeScreamer));



        isPlayingAmbientIndoor = !isPlayingAmbientIndoor;
    }

    public IEnumerator SmoothLevelVolume(AudioClip clip)
    {
        float timeToFade = 5f;
        float timeElapsed = 0;
        currentVolume = ambientIndoorAudioSource.volume;
        if (isPlayingAmbientIndoor)
        {
            ambientOutdoorAudioSource.clip = clip;
            ambientOutdoorAudioSource.Play();
            while (timeElapsed < timeToFade)
            {
                ambientOutdoorAudioSource.volume = Mathf.Lerp(minvol, maxvol, timeElapsed / timeToFade);
                ambientIndoorAudioSource.volume = Mathf.Lerp(maxvol, minvol, timeElapsed / timeToFade);
                timeElapsed += Time.deltaTime;
                yield return null;
            }
            ambientIndoorAudioSource.Stop();


        }
        else
        {

            ambientIndoorAudioSource.clip = clip;
            ambientIndoorAudioSource.Play();
            while (timeElapsed < timeToFade)
            {
                ambientIndoorAudioSource.volume = Mathf.Lerp(minvol, maxvol, timeElapsed / timeToFade);
                ambientOutdoorAudioSource.volume = Mathf.Lerp(maxvol, minvol, timeElapsed / timeToFade);
                timeElapsed += Time.deltaTime;
                yield return null;
            }
            ambientOutdoorAudioSource.Stop();

        }

    }

    public IEnumerator SmoothStopLevelVolume(AudioSource source)
    {
        float timeToFade = 3f;
        float timeElapsed = 0;
        currentVolume = source.volume;
        if (source.isPlaying)
        {
            while (timeElapsed < timeToFade)
            {
                source.volume = Mathf.Lerp(currentVolume, 0, timeElapsed / timeToFade);
                timeElapsed += Time.deltaTime;
                yield return null;
            }
            


        }
        

    }


    private void Update() {

        timer += Time.deltaTime;
        if (timer >= thunderTransitionTime && enableThunder)
        {
            StopCoroutine(LightingThunderSupport());
            thunder_audios.clip = thunderclip[Random.Range(0, thunderclip.Length)];
            StartCoroutine(LightingThunderSupport());
            thunder_audios.PlayOneShot(thunder_audios.clip);
            timer = 0.0f;
        }
    }

    public IEnumerator LightingThunderSupport()
    {
        if (thunder_audios.isPlaying)
        {
            thunder_audios.Stop();
        }
        lighting.enabled = true;
        yield return new WaitForSeconds(0.1f);
        lighting.enabled = false;
        yield return new WaitForSeconds(0.2f);
        lighting.enabled = true;
        yield return new WaitForSeconds(0.1f);
        lighting.enabled = false;
        yield return new WaitForSeconds(0.2f);
        lighting.enabled = true;
        yield return new WaitForSeconds(0.1f);
        lighting.enabled = false;
        yield break;
    }

    public IEnumerator LightingThunderIntro()
    {
        if (thunder_audios.isPlaying)
        {
            thunder_audios.Stop();
        }
        lighting.enabled = true;
        yield return new WaitForSeconds(0.1f);
        lighting.enabled = false;
        yield return new WaitForSeconds(0.2f);
        lighting.enabled = true;
        yield return new WaitForSeconds(0.2f);
        lighting.enabled = false;
        yield return new WaitForSeconds(0.1f);
        lighting.enabled = true;
        yield return new WaitForSeconds(0.2f);
        lighting.enabled = false;
        yield return new WaitForSeconds(0.2f);
        lighting.enabled = true;
        yield return new WaitForSeconds(0.1f);
        lighting.enabled = false;
        yield return new WaitForSeconds(0.1f);
        lighting.enabled = true;     
        yield break;
    }


    public void PlayThunder()
    {
        //thunder_audios.clip = thunderclip[Random.Range(0, thunderclip.Length)];
        StartCoroutine(LightingThunderIntro());
        //thunder_audios.PlayOneShot(thunder_audios.clip);
    }

}
