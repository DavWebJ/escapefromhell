using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateTowardsTarget : State
{
    public CombatState combatState;

    public override State Tick(AIManager enemyManager, AiAnimatorManager enemyAnimatorManager)
    {
        enemyAnimatorManager.animator.SetFloat("Vertical", 0);
        enemyAnimatorManager.animator.SetFloat("Horizontal", 0);

        Vector3 targetDirection = enemyManager.currentTarget.transform.position - enemyManager.transform.position;
        float viewableAngle = Vector3.SignedAngle(targetDirection, enemyManager.transform.forward,Vector3.up);

        if(viewableAngle >= 100 && viewableAngle <= 180 && !enemyManager.isInterracting)
        {
            enemyAnimatorManager.PlayTargetAnimationWithRootRotation("TurnComplete", true);
            return this;
        }else if(viewableAngle <= -101 && viewableAngle >= -180 && !enemyManager.isInterracting)
        {
            enemyAnimatorManager.PlayTargetAnimationWithRootRotation("TurnComplete", true);
            return this;
        }
        else if(viewableAngle <= -45 && viewableAngle >= -100 && !enemyManager.isInterracting)
        {
            enemyAnimatorManager.PlayTargetAnimationWithRootRotation("TurnRight", true);
            return this;
        }
        else if (viewableAngle >= 45 && viewableAngle <= 100 && !enemyManager.isInterracting)
        {
            enemyAnimatorManager.PlayTargetAnimationWithRootRotation("TurnLeft", true);
            return this;
        }


        return combatState;
    }
}
