using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseExit : MonoBehaviour
{

    public GameObject door;
    public AudioSource AudioSource;
    public AudioClip close,stringer,tension;
    public string doortype = "";

    private void Start()
    {
        AudioSource = GetComponent<AudioSource>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            
            AudioSource.PlayOneShot(close);
            StartCoroutine(CloseDoor());
        }
    }

    public IEnumerator CloseDoor()
    {
        
        door.GetComponent<Animator>().enabled = true;
        yield return new WaitForSeconds(close.length);
        switch (doortype)
        {
            case "exit":
                AudioM.instance.screamer_audios.PlayOneShot(tension);
                break;
            default:
                AudioM.instance.screamer_audios.PlayOneShot(stringer);
                break;
        }
        
        
        Destroy(gameObject);
    }
}
