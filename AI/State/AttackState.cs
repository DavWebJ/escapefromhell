using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : State
{
    public CombatState combatState;
    public ChaseState chaseState;
    public EnemyAttackActions currentAttack;
    public RotateTowardsTarget rotateState;
    public bool hasPerformedAttack = false;
    public bool canDoCombo = false;
    public bool canDoPary;
    public override State Tick(AIManager enemyManager, AiAnimatorManager enemyAnimatorManager)
    {
        float distanceFromTarget = Vector3.Distance(enemyManager.currentTarget.transform.position, enemyManager.transform.position);
        RotateTowardsTargetWhilstAttacking(enemyManager);
        if(distanceFromTarget > enemyManager.maximumAttackRange)
        {
            return chaseState;
        }

        if (!hasPerformedAttack && !canDoPary)
        {
            AttackTarget(enemyAnimatorManager,enemyManager);
            ChooseIfCanDoCombo(enemyManager);
        }

        if(!hasPerformedAttack && canDoPary)
        {
            MakeAPary(enemyAnimatorManager, enemyManager);
        }

        if (enemyManager.canPary)
        {
            ChooseIfICanPary(enemyManager);
        }

       

        if(canDoCombo)
        {
            AttackTargetWithCombo(enemyAnimatorManager,enemyManager);
            
        }

        return rotateState;


    }

    private void AttackTarget(AiAnimatorManager enemyAnimatorManager,AIManager enemyManager)
    {
        enemyAnimatorManager.PlayTargetAnimation(currentAttack.actionAnimation,true);
        enemyManager.currentRecoveryTime = currentAttack.recoveryTime;
        hasPerformedAttack = true;
    }

    private void AttackTargetWithCombo(AiAnimatorManager enemyAnimatorManager, AIManager enemyManager)
    {
        
        canDoCombo = false;
        enemyAnimatorManager.PlayTargetAnimation("Combo", true);

        currentAttack = null;
        enemyManager.currentRecoveryTime = 3;

    }

    public void MakeAPary(AiAnimatorManager enemyAnimatorManager, AIManager enemyManager)
    {
        
        enemyAnimatorManager.animator.SetBool("Pary", true);
        currentAttack = null;
        enemyManager.currentRecoveryTime = 1;
        canDoPary = false;
        enemyManager.canPary = false;
    }

    public void ChooseIfICanPary(AIManager enemyManager)
    {
        float paryChance = Random.Range(0, 100);
        if (paryChance >= enemyManager.chanceToPary)
        {
            canDoPary = true;

        }
        else
        {
            canDoPary = false;
            currentAttack = null;

        }
    }

    public void ChooseIfCanDoCombo(AIManager enemyManager)
    {
        float comboChance = Random.Range(0, 100);

        if(comboChance >= enemyManager.comboLikelyHood)
        {
            canDoCombo = true;
            
        }
        else
        {
            canDoCombo = false;
            currentAttack = null;
           
        }
    }

    public void RotateTowardsTargetWhilstAttacking(AIManager enemyManager)
    {
        // Rotate ai manuallly
        if (enemyManager.canRotate && enemyManager.isInterracting)
        {

            Vector3 direction = enemyManager.currentTarget.transform.position - transform.position;
            direction.y = 0;
            direction.Normalize();

            if (direction == Vector3.zero)
            {
                direction = transform.forward;
            }

            Quaternion targetRotation = Quaternion.LookRotation(direction);
            enemyManager.transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, enemyManager.rotationSpeed / Time.deltaTime);
        }
       
    }
}
