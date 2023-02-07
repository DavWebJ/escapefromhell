using System.Collections;
using System.Collections.Generic;
using BlackPearl;
using UnityEngine;

public class ItemGroundManager : MonoBehaviour
{
    public Item itemRef;
    public bool isDropped = false;
    Transform objectTransform;
    Rigidbody rb;
    public float timer = 0;
    public bool startTimer = false;
    private void Start()
    {
        itemRef = GetComponent<ItemToPickUp>().item;
        objectTransform = transform;
        rb = GetComponent<Rigidbody>();
    }


    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "ground")
        {
            if (isDropped)
            {
                timer = 0;
                ReEnableGroundItem();
            }

        }
    }



    public void ReEnableGroundItem()
    {
        rb.useGravity = false;
        gameObject.GetComponent<MeshCollider>().enabled = false;
        gameObject.GetComponent<SphereCollider>().isTrigger = true;
        rb.velocity = Vector3.zero;
        if (itemRef.isPerimable)
        {
            startTimer = true;
            timer = itemRef.timeToDestroy;
        }
       
       
    }

    private void Update()
    {
        if(isDropped && startTimer)
        {
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                
                startTimer = false;
                timer = 0;
                HUDInfos.instance.ClosePickupInfos();
                Destroy(gameObject);
            }
        }

    }

}
