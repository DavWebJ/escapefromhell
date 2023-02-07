using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseState : State
{
    public CombatState combatState;
    public override State Tick(AIManager enemyManager, AiAnimatorManager enemyAnimatorManager)
    {
        if (enemyManager.isPerformingAction || enemyManager.vital.isdead)
        {
            enemyAnimatorManager.animator.SetFloat("Vertical", 0, 0.1f, Time.deltaTime);
            return this;
        }

        enemyManager.currentSpeed = enemyManager.runspeed;
        enemyManager.agent.speed = enemyManager.currentSpeed;
        Vector3 targetDirection = enemyManager.currentTarget.transform.position - enemyManager.transform.position;
        float distanceFromTarget = Vector3.Distance(enemyManager.currentTarget.transform.position, enemyManager.transform.position);
        float viewableAngle = Vector3.Angle(targetDirection, enemyManager.transform.forward);
        HandleRotateTowardsTarget(enemyManager);
        if (distanceFromTarget > enemyManager.maximumAttackRange)
        {

            enemyAnimatorManager.animator.SetFloat("Vertical", 2, 0.1f, Time.deltaTime);
            
        }

        

        
        enemyManager.agent.transform.localPosition = Vector3.zero;
        enemyManager.agent.transform.localRotation = Quaternion.identity;

        if(distanceFromTarget <= enemyManager.maximumAttackRange)
        {
            return combatState;
        }
        else
        {
            return this;
        }
        
    }

    public void HandleRotateTowardsTarget(AIManager enemyManager)
    {
        // Rotate ai manuallly
        if (enemyManager.isPerformingAction)
        {
           
            Vector3 direction = enemyManager.currentTarget.transform.position - enemyManager.transform.position;
            direction.y = 0;
            direction.Normalize();

            if (direction == Vector3.zero)
            {
                direction = transform.forward;
            }

            Quaternion targetRotation = Quaternion.LookRotation(direction);
            enemyManager.transform.rotation = Quaternion.Slerp(enemyManager.transform.rotation, targetRotation, enemyManager.rotationSpeed / Time.deltaTime);
        }
        else // Rotate with pathfinding (agent)
        {
            
            Vector3 relativeDirection = enemyManager.transform.InverseTransformDirection(enemyManager.agent.desiredVelocity);
            Vector3 targetVelocity = enemyManager.rb.velocity;

            enemyManager.agent.enabled = true;
            Quaternion rot = Quaternion.LookRotation(enemyManager.currentTarget.transform.position - enemyManager.transform.position);
            enemyManager.transform.rotation = Quaternion.Slerp(enemyManager.transform.rotation, rot, enemyManager.rotationSpeed / Time.deltaTime);
            enemyManager.agent.SetDestination(enemyManager.currentTarget.transform.position);
           enemyManager.rb.velocity = targetVelocity;

        }

    }


}
