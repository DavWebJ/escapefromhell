using System.Collections;
using System.Collections.Generic;
using SUPERCharacter;
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
        private SUPERCharacterAIO player;
        public Camera cam = null;
        public float dropForce = 8;
        public LayerMask mask;
        [SerializeField] public float RayLength;
        public bool resetRotation = true;
        public ItemToPickUp currentItem;
        public PickUp pickup;
        internal Vector3 cameraStartingPosition;
        public GameObject armEmpty;
        public GameObject smoke;

        private void Start() {

            cam = GetComponent<Camera>();

            cameraStartingPosition = transform.localPosition;
            smoke.SetActive(false);
        }


        public void Init(SUPERCharacterAIO _player)
        {
            this.player = _player;
         
            pickup = player.GetComponent<PickUp>();

            targetLook = transform.Find("TargetLook");
            targetEject = transform.Find("DropPoint");
            armsHolder = transform.Find("HolderArms");
            targetZoom = transform.Find("TargetZoom");
            StartCoroutine(ApplyRotation());
            transform.LookAt(targetLook);
            

        }

        public IEnumerator ApplyRotation()
        {
            yield return new WaitForSeconds(0.5f);
            resetRotation = false;
            yield return new WaitForSeconds(2);
            resetRotation = true;
            yield break;
        }


        public void CreateEmptyArm()
        {
            GameObject go = Instantiate(armEmpty, armsHolder);
            go.GetComponent<ArmsController>().enabled = true;
            go.GetComponent<ArmsController>().canHideArm = true;
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

        private void FixedUpdate()
        {
            if (transform.rotation != Quaternion.identity && !resetRotation)
            {
                Quaternion targetRotation = Quaternion.Euler(0, 0, 0);
                transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * 8f);

            }
            else
            {
                resetRotation = true;
            }
        }


        private void Update()
        {

            Vector3 origin = cam.transform.position;
            Vector3 direction = cam.transform.forward;
            RaycastHit hit;

            if (Physics.Raycast(origin, direction * RayLength, out hit,mask))
            {
                float dist = hit.distance;
                if (hit.transform.TryGetComponent(out ItemToPickUp item) && dist <= RayLength)
                {
                    
                    if (item != null)
                    {
                        
                        ActivateCurrentInterract(item);
                        pickup.OnInterract(item);

                    }

                    
                }else if(hit.transform.TryGetComponent(out OpenDoor door) && dist <= RayLength)
                {
                    
                    InputManager.instance.EnabledInterractpinput(true);

                    DoorUi.instance.ShowUi(door.isClosed);
                    if (InputManager.instance.inventoryInputs.InventoryAction.Interract.triggered)
                    {
                        door.PlayOpenDoor();
                    }
                }
                else
                {
                    DoorUi.instance.HideUi();
                    ClearLastInterract();
                    pickup.DisableInterraction();
                    return;
                }
            }
        }


        void ClearLastInterract()
        {
            if (currentItem != null)
            {
                currentItem.ActivateCurrentOutlines(false);
                currentItem = null;
            }
        }

        public void ActivateCurrentInterract(ItemToPickUp item)
        {
            if(currentItem != item)
            {
                
                ClearLastInterract();
                item.ActivateCurrentOutlines(true);
                currentItem = item;
            }
        }

        public void DestroyCurrentArms()
        {
            if(armsHolder.childCount > 0)
            {
                for (int i = 0; i < armsHolder.childCount; i++)
                {
                    Destroy(armsHolder.GetChild(i).gameObject);
                }
                
            }
        }

        public void AddNewArms(Item go)
        {
            if(go != null)
            {

                Instantiate(go.ArmPrefab, armsHolder);


            }

        }


        public IEnumerator CameraShake(float Duration, float Magnitude)
        {
            
            float elapsed = 0;
            while (elapsed < Duration)
            {
                transform.localPosition = Vector3.MoveTowards(transform.localPosition, new Vector3(cameraStartingPosition.x + Random.Range(-1, 1) * Magnitude, cameraStartingPosition.y + Random.Range(-1, 1) * Magnitude, cameraStartingPosition.z), Magnitude * 2);
                yield return new WaitForSecondsRealtime(0.001f);
                elapsed += Time.deltaTime;
                yield return null;
            }
            transform.localPosition = cameraStartingPosition;
        }
        //private void OnDrawGizmos()
        //{
        //    Gizmos.color = Color.red;
        //    Gizmos.DrawLine(transform.position, transform.forward * RayLength);
        //}

    }




}