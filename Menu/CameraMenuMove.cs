using System.Collections;
using System.Collections.Generic;
using BlackPearl;
using UnityEngine;

public class CameraMenuMove : MonoBehaviour
{
    
    public bool backToOrigin = false;
    public bool goTogame = false;
    public Vector3 origin;
    public Quaternion originRot;
    public Animator animator;
    public GameObject bookShelf;
    public GameObject door;
    public GameObject scareCrow;
    public GameObject title;
    public bool enableCameraShake;
    public float rotationSpeed = 2.5f;
    public bool isInsideCorridor = false;
    public Light[] lights;
    public float LightFlashSeconds;
    public bool isApplyRotation = false;
    public bool isMovingBookShelf = false;
    private IEnumerator playTheLights;
    void Start()
    {
        origin = this.transform.position;
        originRot = Quaternion.identity;
        animator = GetComponent<Animator>();
        bookShelf.GetComponent<Animator>().enabled = false;
        title.GetComponentInChildren<Animator>().enabled = false;
        title.SetActive(false);
        isInsideCorridor = false;
        foreach (Light light in lights)
        {
            light.enabled = false;
        }
        playTheLights = PlayTheLights();
        ThunderManager.instance.EnableMenuThunder();
    }

    public void ResetRotation()
    {
        goTogame = true;
    }

    public void applyRotation()
    {
        isApplyRotation = true;
    }



    public void OpenDoor()
    {
        
        door.GetComponent<Animator>().enabled = true;
        isInsideCorridor = true;
        
        AnimateTheLight();
        
        
    }

    public void DeactivateScareCrow()
    {
        scareCrow.SetActive(false);
    }
    public void PlayCameraShakeAnimation()
    {

        StartCoroutine(CameraShake(0.5f,0.5f));

    }

    public void PlayThunder()
    {
        ThunderManager.instance.PlayThunder();
    }

    public void DisableAnimator()
    {
        door.GetComponent<Animator>().enabled = false;
        bookShelf.GetComponent<Animator>().enabled = false;
    }

    public void AnimBookShelf()
    {
        isMovingBookShelf = true;
        MenuAudioManager.instance.PlayOneShotClip(MenuAudioManager.instance.menu_fx_screamer, MenuAudioManager.instance.bookShelfMove);

    }

    public void ShowPanelMenu()
    {

        MainMenuManager.instance.panel_menu.SetActive(true);
        MainMenuManager.instance.onShowMenu();
            
        

    }

    public void StopTheLights()
    {
        

        StopAllCoroutines();
        StartCoroutine(CameraShake(0.5f, 0.5f));




    }


    public void PlayTension()
    {
        MenuAudioManager.instance.tension_audio.Play();
        
        StartCoroutine(PlayFinalSound());

        

    }

    public IEnumerator PlayFinalSound()
    {
        
        yield return new WaitForSeconds(7);
        isInsideCorridor = false;
        AnimateTheLight();
        yield return new WaitForSeconds(2f);
        title.SetActive(true);
        title.GetComponentInChildren<Animator>().enabled = true;

        foreach (Light light in lights)
        {

            light.enabled = true;
        }
        
        MenuAudioManager.instance.PlayEndScreamer();
        StartCoroutine(CameraShake(0.25f, 0.25f));

        yield return new WaitForSeconds(MenuAudioManager.instance.endscreamerClip.length);
        yield return new WaitForSeconds(3);
        AudioM.instance.ResetAllSound();
        
        title.SetActive(false);
        foreach (Light light in lights)
        {

            light.enabled = false;
        }
        
        
        
        ThunderManager.instance.PlayThunderEndMenu();
        StartCoroutine(LaunchLoadingScrenn());
        yield break;
    }




    public void AnimateTheLight()
    {
        if (isInsideCorridor)
        {
            StartCoroutine(PlayTheLights());
        }
        else
        {
            
            StopCoroutine(playTheLights);

            foreach (Light light in lights)
            {

                light.enabled = false;
            }
        }
            
        
    }

    public IEnumerator PlayTheLights()
    {
        if (!isInsideCorridor)
            yield break;

        while (isInsideCorridor)
        {
            
            int randomLights = Random.Range(0, lights.Length);
            lights[randomLights].enabled = true;
            yield return new WaitForSeconds(Random.Range(0.1f, 0.2f));
            lights[randomLights].enabled = false;
            yield return new WaitForSeconds(0);
        }
        yield return null;

    }

    public IEnumerator LaunchLoadingScrenn()
    {
        yield return new WaitForSeconds(3);
        LoadingScreenManager.instance.LoadScene(LoadingScreenManager.instance.gameScene);
        yield break;
    }
    public void MoveToGame()
    {
   
        animator.SetBool("goToMenu", false);
        animator.SetBool("goGame", true);
 
    }

    public void DisableAudioMenu()
    {
        if (MenuAudioManager.instance.menu_audio.isPlaying)
        {
            MenuAudioManager.instance.StopAllSound(MenuAudioManager.instance.menu_audio);
        }
        
    }



    public void MoveToTheMenu()
    {
        MenuAudioManager.instance.enableScreamer = false;
        animator.enabled = true;
        MainMenuManager.instance.DisableButtonStart();
        animator.SetBool("goToMenu", true);
    }

    public IEnumerator CameraShake(float Duration, float Magnitude)
    {
        animator.enabled = false;
        float elapsed = 0;
        Vector3 cameraStartingPosition = this.transform.position;
        while (elapsed < Duration)
        {
            this.transform.localPosition = Vector3.MoveTowards(this.transform.localPosition, new Vector3(cameraStartingPosition.x + Random.Range(-1, 1) * Magnitude, cameraStartingPosition.y + Random.Range(-1, 1) * Magnitude, cameraStartingPosition.z), Magnitude * 2);
            yield return new WaitForSecondsRealtime(0.001f);
            elapsed += Time.deltaTime;
            yield return null;
        }
        this.transform.localPosition = cameraStartingPosition;

        yield break;

    }
    void Update()
    {
        if (goTogame)
        {
            
            if (this.transform.rotation == originRot)
            {
                
                goTogame = false;
                return;
            }

           
            this.transform.rotation = Quaternion.Lerp(this.transform.rotation, originRot, Time.deltaTime * 5f);

        }

        if (isApplyRotation)
        {
            Quaternion targetRotation = Quaternion.Euler(0,0,0);
            Vector3 pos = transform.position;
            pos.y = 2;
    
            if (this.transform.rotation == targetRotation && transform.position == pos)
            {
                isApplyRotation = false;
            }

            this.transform.rotation = Quaternion.Lerp(transform.rotation,targetRotation , Time.deltaTime * 2f);

            transform.position = pos;
        }

        if (isMovingBookShelf)
        {
            Vector3 camTargetPos = transform.position;
            camTargetPos.x = 2.5f;
            
            

            Vector3 pos = new Vector3(-1.48f, 0, 14.49f);
            bookShelf.transform.position = Vector3.Lerp(bookShelf.transform.position, pos, Time.deltaTime * 0.3f);
            transform.position = Vector3.Lerp(transform.position, camTargetPos, Time.deltaTime * 2f);

        }
    }


}
