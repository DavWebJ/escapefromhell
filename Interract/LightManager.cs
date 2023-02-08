using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightManager : MonoBehaviour
{
    public static LightManager instance;

    public GameObject[] lights;
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }
    void Start()
    {
        
    }


    public void SwichOn()
    {
        foreach (GameObject light in lights)
        {
            light.SetActive(true);
            
            light.GetComponentInParent<MeshRenderer>().material.EnableKeyword("_EMISSION");
            light.GetComponent<Light>().intensity = 3;
            light.GetComponent<Light>().range = 8;

        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
