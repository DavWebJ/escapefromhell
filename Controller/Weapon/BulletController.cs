using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BlackPearl;
public class BulletController : MonoBehaviour
{
   [SerializeField] private float speed = 0.1f;
   [SerializeField] private float impact = 200;

    [SerializeField] private AudioSource audios;


    public float damage;
    [SerializeField] public AudioClip metal, concrete, wood;
   private Vector3 lastPos = new Vector3();
    void Start()
    {
        lastPos = transform.position;

        
    }





    void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
        RaycastHit hit;
          if(Physics.Linecast(lastPos,transform.position,out hit))
          {
              if(hit.collider)
              {
               
                if (hit.collider.sharedMaterial)
                {
                    
                    GameObject go = GameManager.instance.resources.getSurface(hit.collider.sharedMaterial.name);
                    GameObject hole = GameManager.instance.resources.GetHole(hit.collider.sharedMaterial.name);

                    switch (hit.collider.sharedMaterial.name)
                    {
                        case "Wood":
                            if(audios != null)
                            {
                                if(wood != null)
                                {
                                    audios.PlayOneShot(wood);
                                    
                                }
                            }
                            
                            break;
                        case "Metal":
                            audios.PlayOneShot(metal);
                            break;
                        case "Concrete":
                            audios.PlayOneShot(concrete);
                            break;
                        case "Character":
                            if(hit.collider.tag == "NPC")
                            {
                                Quaternion rot = Quaternion.Euler(0, 180, 0);
                                Instantiate(hit.collider.gameObject.GetComponent<AiHealthManager>().fx, hit.transform.position, rot);
                                //hit.collider.gameObject.GetComponent<BlazeAI>().Hit();

                                hit.collider.gameObject.GetComponent<AiHealthManager>().TakeDamage(damage);
                            }
                            break;
                        default:
                            break;
                    }
                    if (go != null)
                    {
                        GameObject impact = Instantiate(go, hit.point + hit.normal * 0.001f, Quaternion.LookRotation(hit.normal));
                        impact.transform.parent = hit.collider.transform;


                        
                    }
                    if(hole != null)
                    {
                        GameObject bullet_hole = Instantiate(hole, hit.point + hit.normal * 0.001f, Quaternion.LookRotation(hit.normal));
                        bullet_hole.transform.parent = hit.collider.transform;
                    }
                }

                    Destroy(gameObject, 3);

            }
            else
            {
                Destroy(gameObject, 5);
            }
          }
        
        lastPos = transform.position;
    }
}
