using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDestruct : MonoBehaviour
{


    private void OnEnable()
    {
        Destroy(gameObject, 5);
    }
}
