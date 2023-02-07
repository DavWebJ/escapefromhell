using System.Collections;
using System.Collections.Generic;
using BlackPearl;
using UnityEngine;

public class AIDamageDealer : MonoBehaviour
{
    public bool canDamage;
    public bool hasDealDamage;
    public LayerMask mask;
    public AIManager aIManager;

    [SerializeField] public float weaponLength;
    void Start()
    {
        canDamage = false;
        hasDealDamage = false;
    }

    void Update()
    {

        
        if (canDamage && !hasDealDamage)
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, -transform.up, out hit, weaponLength, mask))
            {

                if (!hit.transform.gameObject.GetComponent<VitalState>().isdead)
                {
                    VitalState vital = hit.transform.gameObject.GetComponent<VitalState>();
                    hasDealDamage = true;
                    Vector3 spawn = transform.position - transform.up * weaponLength;
                    GameObject go = Instantiate(vital.hitprefab, spawn, Quaternion.identity);

                    hit.transform.gameObject.GetComponent<VitalState>().TakeDamage(aIManager.enemy.damage);
                    
                }


                   

            }
        }
        
    }

    public void StartDealDamage()
    {
        canDamage = true;
        hasDealDamage = false;
    }
    public void EndDealDamage()
    {
        canDamage = false;

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position - transform.up * weaponLength);
    }
}
