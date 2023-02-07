using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecoilWeapon : MonoBehaviour
{
    Vector3 currentRot, targetRot, targetPos, currentPos, initialPos;
    public Transform cam;

    [SerializeField] float recoilX;
    [SerializeField] float recoilY;
    [SerializeField] float recoilZ;
    [SerializeField] float kickBackZ;

    public float snap, returnAmount;
    void Start()
    {
        initialPos = transform.localPosition;
        cam = GetComponentInParent<Camera>().transform;
    }

    public void Recoil()
    {
        targetPos -= new Vector3(0, 0, kickBackZ);
        targetRot += new Vector3(recoilX, Random.Range(-recoilY, recoilY), Random.Range(-recoilZ, recoilZ));
        
        cam.Rotate(Random.Range(-3, 3), 0,0);
       
    }

    public void Back()
    {
        targetPos = Vector3.Lerp(targetPos, initialPos, Time.deltaTime * returnAmount);
        currentPos = Vector3.Lerp(currentPos, targetPos, Time.fixedDeltaTime * snap);
        transform.localPosition = currentPos;
    }

    void Update()
    {
        targetRot = Vector3.Lerp(targetRot, Vector3.zero, Time.deltaTime * returnAmount);
        currentRot = Vector3.Slerp(currentRot, targetRot, Time.fixedDeltaTime * snap);
        transform.localRotation = Quaternion.Euler(currentRot);
        //cam.localRotation = Quaternion.Euler(currentRot);
        Back();
    }
}
