using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;

namespace BlackPearl{
    public class FPSCamera : MonoBehaviour
    {
        
        public Transform targetLook = null;
        public Transform targetEject = null;
        public Transform targetZoom = null;
        public Transform armsHolder = null;
        public float zoomAim = 15;
        private FirstPersonAIO player;
        public Camera cam = null;
        public const float rayDist = 2;
        public float dropForce = 20;

        private PickUp currInteract = null;
        private float fov_origin = 60;
        public bool isInterracting = false;

        private void Awake()
        {


                
            
        }
        private void Start() {
           
            // rain = GetComponentInChildren<RainCameraController>();
        
        }


        public void Init(FirstPersonAIO _player)
        {
            this.player = _player;
            cam = GetComponent<Camera>();
            fov_origin = cam.fieldOfView;
            targetLook = transform.Find("TargetLook");
            targetEject = transform.Find("DropPoint");
            armsHolder = transform.Find("HolderArms");
            targetZoom = transform.Find("TargetZoom");

 
        }


        public void Aim(bool aiming)
        {
            // if(aiming)
            // {
            //     cam.fieldOfView = Mathf.Lerp(cam.fieldOfView,fov_origin - zoomAim , Time.deltaTime * 8);
            // }
            // else
            // {
            //     cam.fieldOfView = Mathf.Lerp(cam.fieldOfView,fov_origin , Time.deltaTime * 8);
            // }
        }

        public void updateTargetLook()
        {
            Vector3 origin = cam.transform.position;
            Vector3 direction = cam.transform.forward;
            Debug.DrawLine(origin,direction * 1000f,Color.blue);
            RaycastHit hit;
            if(Physics.Raycast(cam.transform.position,cam.transform.forward * 1000f,out hit))
            {
                targetLook.position = hit.point;
            }else
            {
                targetLook.position = cam.transform.forward * 1000f;
            }
        }
        
        public void updateRaycast()
        {
            
            if(CanInterract())
            {

                // Creates a Ray from the center of the viewport
                Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
                RaycastHit hit;
                Physics.Raycast(ray,out hit,rayDist);
                Vector3 hitPosition = hit.point;
                float hitDistance = hit.distance;
                float desiredDistance = 2f;


                if (Physics.Raycast(ray, out hit, rayDist) && hit.collider.GetComponent<PickUp>())
                {
                    
                    if(hitDistance < desiredDistance)
                    {
                        //Debug.DrawRay(ray.origin, ray.direction, Color.green);
                        if (currInteract == null || currInteract != hit.collider.GetComponent<PickUp>())
                        {
                            if (currInteract != null)
                            {

                                currInteract.ActivateOutlines(false);
                                currInteract.ActivateCurrentOutlines(false);
                                HUDInfos.instance.SceneObjectInfos(null);
                                isInterracting = false;

                            }
                            currInteract = hit.collider.GetComponent<PickUp>();
                            isInterracting = true;
                            if (currInteract.item.objectType == ObjectType.Interractable)
                            {
                                currInteract.ActivateOutlines(false);
                                currInteract.ActivateCurrentOutlines(true);
                            }
                            else
                            {
                                currInteract.ActivateOutlines(isInterracting);
                            }
                            

                            
                     
                            HUD.instance.ChangeCrossHair(HUD.crosshair_type.pickup);
                            HUDInfos.instance.SceneObjectInfos(isInterracting ? currInteract : null);


                        }
                    }
                }
                else
                {
                    isInterracting = false;
                    if (currInteract != null)
                    {
                        currInteract.ActivateOutlines(isInterracting);
                        currInteract.ActivateCurrentOutlines(false);

                        HUDInfos.instance.SceneObjectInfos(null);
                        currInteract = null;
                        isInterracting = false;

                    }
                    HUD.instance.ChangeCrossHair(HUD.crosshair_type.normal);
                    
                }

               
                    
                // HUD.instance.ChangeCrossHair(HUD.crosshair_type.pickup);


            }
            else
            {
               
                if(currInteract != null)
                {
                    currInteract.ActivateOutlines(isInterracting);
                    currInteract.ActivateCurrentOutlines(false);
                    HUDInfos.instance.SceneObjectInfos(null);
                    currInteract = null;
                 
                }
            }
        }

        public bool CanInterract()
        {

            return(Inventory.instance.isInventoryOpen == false);
        }

        public void PickUpInput()
        {
            
            if(isInterracting && currInteract != null)
            {


                switch (currInteract.actionType)
                {
                    case PickUp.ActionType.pickable:
                        currInteract.PickUpItem();
                        break;
                    case PickUp.ActionType.equipable:
                        currInteract.EquipItem();
                        break;
                    case PickUp.ActionType.interractable:
                        currInteract.InterractAction();
                        break;
                    default:
                        break;
                }

                
                HUDInfos.instance.SceneObjectInfos(null);
            }
        }



        private void OnTriggerEnter(Collider other)
        {
            if(other.tag == "lb_bird")
            {
                player.playerVitals.Takedamage(15);
            }
        }
    }

   


}