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
  
    public GameObject door;
    public GameObject scareCrow;
    public GameObject title;
    public bool enableCameraShake;
    void Start()
    {
        origin = this.transform.position;
        originRot = this.transform.rotation;
        animator = GetComponent<Animator>();
        title.SetActive(false);
        scareCrow.SetActive(true);
       
    }

    public void ResetRotation()
    {
        goTogame = true;
    }



    public void OpenDoor()
    {
       
        door.GetComponent<Animator>().enabled = true;
        door.GetComponent<OpenDoorMenu>().PlaySound();
        
        
    }

    public void DeactivateScareCrow()
    {
        scareCrow.SetActive(false);
    }
    public void PlayScareCrowAnimation()
    {
        
        AudioM.instance.PlayThunder();
        MainMenuManager.instance.AudioSource.clip = MainMenuManager.instance.start;
        MainMenuManager.instance.AudioSource.Play();
        title.SetActive(true);
        StartCoroutine(CameraShake(0.5f,0.5f));
        
        StartCoroutine(LaunchLoadingScrenn());
    }

    public void DisableAnimator()
    {
        door.GetComponent<Animator>().enabled = false;
    }

    public void ShowPanelMenu()
    {

           MainMenuManager.instance.panel_menu.SetActive(true);
            
        

    }

    public void PlayTension()
    {
        AudioM.instance.tension_audio.Play();
    }

    public void GoToGameLauncher()
    {
        goTogame = false;
        
        StartCoroutine(MoveToGame());
    }

    private void FixedUpdate()
    {
        
    }

    public IEnumerator MoveToMenu()
    {
        
        animator.enabled = true;
        animator.SetBool("goToMenu", true);
        yield break;
    }
    public IEnumerator LaunchLoadingScrenn()
    {
        //canva.GetComponent<MainMenuManager>().AudioSource.PlayOneShot(canva.GetComponent<MainMenuManager>().start);
       
        yield return new WaitForSeconds(3);
        MainMenuManager.instance.LoadCutSceneIntro();
        yield break;
    }
    public IEnumerator MoveToGame()
    {
        backToOrigin = false;
        animator.SetBool("goToMenu", false);
        animator.SetBool("goGame", true);
        

        yield break;
    }

    public IEnumerator CameraShake(float Duration, float Magnitude)
    {
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
        
    }
    void Update()
    {
        if (backToOrigin)
        {
            if(this.transform.position == origin && this.transform.rotation == originRot)
            {
                backToOrigin = false;
                
                StartCoroutine(MoveToMenu());
                
            }
           this.transform.position =  Vector3.Lerp(this.transform.position, origin, Time.deltaTime * 2.5f);
           this.transform.rotation =  Quaternion.Lerp(this.transform.rotation,originRot, Time.deltaTime * 2.5f);
        }

        if (goTogame)
        {
            if (this.transform.rotation == Quaternion.identity)
            {
                goTogame = false;
            }
                
            this.transform.rotation = Quaternion.Lerp(this.transform.rotation, Quaternion.identity, Time.deltaTime * 2.5f);
        }
    }

    
}
