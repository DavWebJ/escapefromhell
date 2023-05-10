using System.Collections;
using System.Collections.Generic;
using BlackPearl;
using UnityEngine;

public class CameraEvents : MonoBehaviour
{
    public float shakeDuration = 0.1f;
    public float shakeIntensity = 0.05f;
    public AnimationCurve shakeCurve = AnimationCurve.Linear(0.0f, 1.0f, 1.0f, 0.0f);

    // the initial position of the camera
    private Vector3 initialPosition;

    private Quaternion initialRot; 


    // coroutine that shakes the camera
    private IEnumerator ShakeCoroutine()
    {
        float elapsed = 0.0f;

        while (elapsed < shakeDuration)
        {
            float progress = elapsed / shakeDuration;

            // calculate the Perlin noise offsets using the shake progress and a random seed
            float x = Random.Range(0.0f, 1.0f);
            float y = Random.Range(0.0f, 1.0f);
            float z = Random.Range(0.0f, 1.0f);
            Vector3 noiseOffset = new Vector3(x, y, z) * 100.0f;

            // apply the shake intensity and the curve to the noise offsets
            float curveValue = shakeCurve.Evaluate(progress);
            Vector3 shakeOffset = new Vector3(Mathf.PerlinNoise(noiseOffset.x, 0.0f), Mathf.PerlinNoise(noiseOffset.y, 0.0f), Mathf.PerlinNoise(noiseOffset.z, 0.0f));
            shakeOffset = shakeOffset * shakeIntensity * curveValue;

            // convert the shake offset to a rotation and apply it to the camera rotation
            Quaternion shakeRotation = Quaternion.Euler(shakeOffset);
            transform.localRotation = initialRot * shakeRotation;

            elapsed += Time.deltaTime;
            yield return null;
        }

        // reset the camera position
        transform.position = initialPosition;
        transform.rotation = initialRot;
    }
    public void StartShaker()
    {

        initialPosition = transform.position;
        initialRot = transform.rotation;
        StartCoroutine(ShakeCoroutine());
    }
    
    
}
