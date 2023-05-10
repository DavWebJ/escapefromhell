using System.Collections;
using System.Collections.Generic;
using BlackPearl;
using UnityEngine;

public class AIDamageDealer : MonoBehaviour
{
    public bool canDamage;
    public List<GameObject> hasDealDamage;
    public float _damage;

    public AudioSource audios;
    [SerializeField] public AudioClip flesh;

    //[SerializeField] public GameObject Fx;

    void Start()
    {
        canDamage = false;
        audios = GetComponent<AudioSource>();
        hasDealDamage = new List<GameObject>();
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (collision != null && canDamage)
        {
            if (collision.collider.tag == "Player")
            {
                audios.PlayOneShot(flesh);
                if(collision.gameObject.GetComponent<VitalState>() != null)
                {
                    VitalState playerVital = collision.gameObject.GetComponent<VitalState>();
                    playerVital.TakeDamage(_damage);
                }
                //GameObject go = GameManager.instance.resources.getSurface(other.sharedMaterial.name);
               
                //if (go != null)
                //{
                //    Vector3 hit = other.gameObject.GetComponent<Collider>().ClosestPointOnBounds(transform.position);
                //    GameObject impact = Instantiate(go, hit, Quaternion.LookRotation(hit));

                //}
            }

        }
    }

    public void StartDealDamage()
    {
        canDamage = true;
        hasDealDamage.Clear();
    }
    public void EndDealDamage()
    {
        canDamage = false;

    }


}
