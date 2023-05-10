using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiHealthManager : MonoBehaviour
{
    public float maxHealth;
    public float currentHealth;
    public GameObject fx;

    //public BlazeAI blazeAI;
    void Start()
    {
        //blazeAI = GetComponent<BlazeAI>();
        currentHealth = maxHealth;
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        
        if(currentHealth <= 0)
        {
            currentHealth = 0;
            //blazeAI.Death();
            //blazeAI.gameObject.GetComponent<Rigidbody>().useGravity = false;
            //blazeAI.gameObject.GetComponent<CapsuleCollider>().enabled = false;
        }
    }
}
