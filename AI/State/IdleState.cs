using System.Collections;
using System.Collections.Generic;
using BlackPearl;
using UnityEngine;

public class IdleState : State
{
    public ChaseState chaseState;
    public override State Tick(AIManager enemyManager, AiAnimatorManager enemyAnimatorManager)
    {
        #region Handle Detetcion

        Collider[] colliders = Physics.OverlapSphere(transform.position, enemyManager.detectionRadius, enemyManager.detectionMask);
        for (int i = 0; i < colliders.Length; i++)
        {
            VitalState vital = colliders[i].transform.GetComponent<VitalState>();

            if (vital != null)
            {
                Vector3 targetDirection = vital.transform.position - enemyManager.transform.position;
                float viewableAngle = Vector3.Angle(targetDirection, enemyManager.transform.forward);

                if (viewableAngle > enemyManager.minAngle && viewableAngle < enemyManager.maxAngle)
                {
                   enemyManager.currentTarget = vital;
                    

                }
            }
        }
        #endregion

        #region Switch state

        if (enemyManager.currentTarget != null)
        {
            enemyManager.vital = enemyManager.currentTarget.GetComponent<VitalState>();
            return chaseState;
        }
        else
        {
            return this;
        }
        #endregion

    }
}
