using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageDealer : MonoBehaviour
{
    public bool canDamage;
    public List<GameObject> hasDealDamage;
    public float _damage;
    public AudioSource audios;
    [SerializeField] public AudioClip metal, concrete, wood,flesh;

    [SerializeField] public GameObject woodFx, metalFx, ConcreteFx;
    [SerializeField] public float weaponLength;
    void Start()
    {
        canDamage = false;
        audios = GetComponent<AudioSource>();
        hasDealDamage = new List<GameObject>();
    }

    public void SetDamage(float damage)
    {
        _damage = damage;
    }




    private void OnTriggerEnter(Collider other)
    {
        if(other != null && canDamage)
        {
            if (other.tag == "weaponfx")
            {
                GameObject go = GameManager.instance.resources.getSurface(other.sharedMaterial.name);
                switch (other.sharedMaterial.name)
                {
                    case "Wood":
                        audios.PlayOneShot(wood);

                        break;
                    case "Metal":
                        audios.PlayOneShot(metal);
                        break;
                    case "Concrete":
                        audios.PlayOneShot(concrete);
                        break;
                    case "Character":
                        audios.PlayOneShot(flesh);
                        break;
                    default:
                        break;
                }
                if (go != null)
                {
                    Vector3 hit = other.gameObject.GetComponent<Collider>().ClosestPointOnBounds(transform.position);
                    GameObject impact = Instantiate(go, hit, Quaternion.LookRotation(hit));

                }
            }

        }
    }
    void Update()
    {
     
        //if (canDamage)
        //{
        //    RaycastHit hit;
        //    if(Physics.Raycast(transform.position, transform.forward , out hit, weaponLength))
        //    {
        //        if (hit.collider)
        //        {
        //            print("hit");
        //        }
                    
        //            GameObject gos = GameManager.instance.resources.getSurface(hit.collider.sharedMaterial.name);


        //            switch (hit.collider.sharedMaterial.name)
        //            {
        //                case "Wood":
        //                    audios.PlayOneShot(wood);

        //                    break;
        //                case "Metal":
        //                    audios.PlayOneShot(metal);
        //                    break;
        //                case "Concrete":
        //                    audios.PlayOneShot(concrete);
        //                    break;
        //                default:
        //                    break;
        //            }
        //            if (gos != null)
        //            {
        //                GameObject impact = Instantiate(gos, hit.point + hit.normal * 0.001f, Quaternion.LookRotation(hit.normal));
        //                impact.transform.parent = hit.collider.transform;



        //            }

        //        hasDealDamage.Add(hit.transform.gameObject);


        //    }
        //}
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

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position - transform.forward * weaponLength);
    }
}
