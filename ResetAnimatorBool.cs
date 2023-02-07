using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetAnimatorBool : StateMachineBehaviour
{

    public string isInterractingBool = "isInterracting";
    public bool status = false;

    public string isRotateWithRootMotionBool = "isRotateWithRootMotion";
    public bool isRotateStatus = false;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool(isInterractingBool, status);
        animator.SetBool(isRotateWithRootMotionBool, isRotateStatus);
    }

}
