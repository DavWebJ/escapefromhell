using System.Collections;
using System.Collections.Generic;
using BlackPearl;
using UnityEngine;

public class AiAnimatorManager : AnimationManager
{
    AIManager enemyManager;
    private void Awake()
    {
        if(animator == null)
            animator = GetComponent<Animator>();

        enemyManager = GetComponent<AIManager>();
    }
    void Start()
    {
        
    }

    private void OnAnimatorMove()
    {
        //if (playerManager.isInterracting == false)
        //{
        //    return;
        //}

        float delta = Time.deltaTime;
        enemyManager.rb.drag = 0;
        Vector3 deltaPosition = animator.deltaPosition;
        deltaPosition.y = 0;
        Vector3 velocity = deltaPosition / delta;
        enemyManager.rb.velocity = velocity;
        if (enemyManager.isRotatingWithRootMotion)
        {
            enemyManager.transform.rotation *= animator.deltaRotation;
        }
    }

    void Update()
    {
        
    }
}
