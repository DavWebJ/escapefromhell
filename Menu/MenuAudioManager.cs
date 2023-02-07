using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuAudioManager : MonoBehaviour
{
    public static MenuAudioManager instance;
    [SerializeField] public AudioSource menu_fx_screamer;
    [SerializeField] public AudioSource screamer_audios;
    [SerializeField] public AudioSource tension_audio;
    [SerializeField] public AudioSource menu_audio;

    [Header("MainMenu")]
    public AudioClip bookShelfMove, endscreamerClip;

    [Header("Monster")]
    public Transform monster;
    public Transform[] spawn;
    public int index;
    public bool enableScreamer = false;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        monster.GetChild(1).gameObject.SetActive(false);
    }


    /// <summary>
    /// play a one shot clip for dynamique sound
    /// </summary>
    /// <param name="source"></param>
    /// <param name="clip"></param>
    public void PlayOneShotClip(AudioSource source, AudioClip clip)
    {
        if (source.volume < 1)
        {
            source.volume = 1;
        }
        source.loop = false;
        if (!source.isPlaying)
        {
            source.PlayOneShot(clip);
        }

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

    public void PlayEndScreamer()
    {
        menu_fx_screamer.clip = endscreamerClip;
        menu_fx_screamer.Play();
    }
}
