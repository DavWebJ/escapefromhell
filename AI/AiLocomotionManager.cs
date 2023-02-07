using System.Collections;
using System.Collections.Generic;
using BlackPearl;
using UnityEngine;

public class AiLocomotionManager : MonoBehaviour
{
    AIManager enemyManager;
    
    public AiAnimatorManager aiAnimatorManager;




    void Start()
    {
        enemyManager = GetComponent<AIManager>();
        aiAnimatorManager = GetComponent<AiAnimatorManager>();


    }



    void Update()
    {
        
    }
}
