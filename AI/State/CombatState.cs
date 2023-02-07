using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatState : State
{
    public AttackState attackState;
    public ChaseState chaseState;
    public EnemyAttackActions[] enemyAttacks;
    bool randomDestination = false;
    float verticalMovementValue = 0;
    float horizontalMovementValue = 0;
    public override State Tick(AIManager enemyManager, AiAnimatorManager enemyAnimatorManager)
    {
        float distanceFromTarget = Vector3.Distance(enemyManager.currentTarget.transform.position, enemyManager.transform.position);
        enemyAnimatorManager.animator.SetFloat("Vertical", verticalMovementValue, 0.2f, Time.deltaTime);
        enemyAnimatorManager.animator.SetFloat("Horizontal", horizontalMovementValue, 0.2f, Time.deltaTime);
        attackState.hasPerformedAttack = false;
        if (enemyManager.isInterracting || enemyManager.vital.isdead)
        {
            enemyAnimatorManager.animator.SetFloat("Vertical", 0);
            enemyAnimatorManager.animator.SetFloat("Horizontal", 0);
            return this;
        }
            

        if (distanceFromTarget > enemyManager.maximumAttackRange)
        {

            return chaseState;
        }

        if (!randomDestination)
        {
            randomDestination = true;
            DecideCirclingAction(enemyAnimatorManager);
        }

        HandleRotateTowardsTarget(enemyManager);
        

        if (enemyManager.currentRecoveryTime <= 0 && attackState.currentAttack != null)
        {
            randomDestination = false;
            return attackState;
        }
        else
        {
            GetNewAttack(enemyManager,enemyAnimatorManager);
            
        }
        return this;
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

    public void DecideCirclingAction(AiAnimatorManager enemyAnimatorManager)
    {
        WalkAroundTarget(enemyAnimatorManager);
    }

    public void WalkAroundTarget(AiAnimatorManager enemyAnimatorManager)
    {
        verticalMovementValue = Random.Range(0, 1);

        if(verticalMovementValue <= 1 && verticalMovementValue > 0)
        {
            verticalMovementValue = 0.5f;
        }else if(verticalMovementValue >= -1 && verticalMovementValue < 0)
        {
            verticalMovementValue = -0.5f;
        }

        horizontalMovementValue = Random.Range(-1, 1);

        if (horizontalMovementValue <= 1 && horizontalMovementValue >= 0)
        {
            horizontalMovementValue = 0.5f;
        }
        else if (horizontalMovementValue >= -1 && horizontalMovementValue < 0)
        {
            horizontalMovementValue = -0.5f;
        }
    }
    public void GetNewAttack(AIManager enemyManager, AiAnimatorManager animator)
    {
        Vector3 targetDirection = enemyManager.currentTarget.transform.position - transform.position;
        float viewableAngle = Vector3.Angle(targetDirection, transform.forward);
        float distanceFromTarget = Vector3.Distance(enemyManager.currentTarget.transform.position, transform.position);
        
        int maxScore = 0;

        for (int i = 0; i < enemyAttacks.Length; i++)
        {
            EnemyAttackActions enemyAttackAction = enemyAttacks[i];
            if (distanceFromTarget <= enemyAttackAction.maximumDistanceNeededToAttack && distanceFromTarget >= enemyAttackAction.minimumDistanceNeededToAttack)
            {

                if (viewableAngle <= enemyAttackAction.maximumAttackAngle && viewableAngle >= enemyAttackAction.minimumAttackAngle)
                {

                    maxScore += enemyAttackAction.attackScore;
                }
            }
        }
        int randomValue = Random.Range(0, maxScore);
        int temporaryScore = 0;

        for (int j = 0; j < enemyAttacks.Length; j++)
        {

            EnemyAttackActions enemyAttackAction = enemyAttacks[j];
            if (distanceFromTarget <= enemyAttackAction.maximumDistanceNeededToAttack && distanceFromTarget >= enemyAttackAction.minimumDistanceNeededToAttack)
            {
                if (viewableAngle <= enemyAttackAction.maximumAttackAngle && viewableAngle >= enemyAttackAction.minimumAttackAngle)
                {
                    if (attackState.currentAttack != null)
                    {

                        return;
                    }

                    temporaryScore += enemyAttackAction.attackScore;

                    if (temporaryScore > randomValue)
                    {
        
                        attackState.currentAttack = enemyAttackAction;

                    }
                }
            }


        }
    }
}
