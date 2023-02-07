using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ThunderType
{
    menu,game
}
public class ThunderManager : MonoBehaviour
{
    public static ThunderManager instance;
    public ThunderType thunderType;

    [SerializeField] public AudioSource thunder_audios;

    [Header("Thunder outdoor clip")]
    public AudioClip[] thunderclip;
    public AudioClip thunderEndClip;

    [Header("light thunder support")]
    public Light lighting;
    [Header("Thunder Param:")]
    public float timer = 0;
    public float thunderTransitionTime = 50;
    public float thunderIntroTransitionTime;
    public bool enableThunder;
    public bool enableThunderIntro = false;

    public float TransitionTimeBetwwenThunder;
    public float minTransitionTimeBetwwenThunder = 15;
    public float maxTransitionTimeBetwwenThunder = 60;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        lighting.enabled = false;
    }
    void Start()
    {
        enableThunderIntro = false;
       
    }

    public void StopAllThunder()
    {
        enableThunder = false;
        enableThunderIntro = false;
        thunder_audios.Stop();
    }

    public void EnableMenuThunder()
    {
        if (!enableThunderIntro)
            enableThunderIntro = true;
       
    }

    public void EnableGameThunder()
    {
        if(!enableThunder)
            enableThunder = true;
    }

    public IEnumerator LightingThunderSupport()
    {

        if (thunder_audios.isPlaying)
        {
            thunder_audios.Stop();
        }
        TransitionTimeBetwwenThunder = Random.Range(minTransitionTimeBetwwenThunder, maxTransitionTimeBetwwenThunder);
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
        //if (index == 3 && enableScreamer)
        //{

        //    monster.GetChild(0).gameObject.SetActive(true);
        //}



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
        //if (enableScreamer)
        //{
        //    yield return new WaitForSeconds(0.5f);
        //    monster.GetChild(0).gameObject.SetActive(false);
        //    index++;
        //    if (index >= spawn.Length)
        //    {
        //        index = 0;
        //    }
        //    monster.position = spawn[index].transform.position;
        //}


        yield break;
    }


    public void PlayThunder()
    {
        thunder_audios.clip = thunderclip[Random.Range(0, thunderclip.Length)];
        StartCoroutine(LightingThunderSupport());
        thunder_audios.PlayOneShot(thunder_audios.clip);
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= TransitionTimeBetwwenThunder && enableThunder)
        {
            StopCoroutine(LightingThunderSupport());
            thunder_audios.clip = thunderclip[Random.Range(0, thunderclip.Length)];
            StartCoroutine(LightingThunderSupport());
            thunder_audios.PlayOneShot(thunder_audios.clip);
            timer = 0.0f;
        }

        if (timer >= thunderIntroTransitionTime && enableThunderIntro)
        {
            //int randomVoice = Random.Range(0, screamerMenu.Length);
            //menu_fx_screamer.PlayOneShot(screamerMenu[randomVoice]);
            StopAllCoroutines();
            thunder_audios.clip = thunderclip[Random.Range(0, thunderclip.Length)];

            StartCoroutine(LightingThunderIntro());
            thunder_audios.PlayOneShot(thunder_audios.clip);

            timer = 0.0f;
        }
    }

    public void PlayThunderEndMenu()
    {

    }



}
