using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace BlackPearl
{

    public class AnimatorHandler : AnimationManager
    {
        public Animator animator;
        //PlayerLocomotion playerLocomotion;
        //InputHandler inputHandler;
        //PlayerManager playerManager;
        int vertical;
        int horizontal;
        public bool canRotate;

        public void Init()
        {
            //inputHandler = GetComponentInParent<InputHandler>();
            //playerManager = GetComponentInParent<PlayerManager>();
            animator = GetComponent<Animator>();
            vertical = Animator.StringToHash("Vertical");
            horizontal = Animator.StringToHash("Horizontal");
            //playerLocomotion = GetComponentInParent<PlayerLocomotion>();
        }

        public void UpdateAnimator(float verticalMovement,float horizontalMovement,bool isSprinting)
        {


            #region Vertical
            float v = 0;
            if (verticalMovement > 0 && verticalMovement < 0.55f)
            {
                v = 0.5f;
            }else if (verticalMovement > 0.55f)
            {
                v = 1;
            }
            else if (verticalMovement < 0 && verticalMovement > -0.55f)
            {
                v = -0.5f;
            }
            else if (verticalMovement < -0.55f)
            {
                v = -1;
            }
            else
            {
                v = 0;
            }
            #endregion

            #region Horizontal
            float h = 0;
            if (horizontalMovement > 0 && horizontalMovement < 0.55f)
            {
                h = 0.5f;
            }
            else if (horizontalMovement > 0.55f)
            {
                h = 1;
            }
            else if (horizontalMovement < 0 && horizontalMovement > -0.55f)
            {
                h = -0.5f;
            }
            else if (horizontalMovement < -0.55f)
            {
                h = -1;
            }
            else
            {
                h = 0;
            }
            #endregion


            if (isSprinting && verticalMovement > 0)
            {
                v = 2;
                h = horizontalMovement;
            }

            //if (playerManager.isInAir)
            //{
            //    v = 0;
            //    h = horizontalMovement;
            //    animator.SetFloat(vertical, 0, 0, Time.deltaTime);
            //    animator.SetFloat(horizontal, 0, 0, Time.deltaTime);
            //}
            animator.SetFloat(vertical, v, 0.1f, Time.deltaTime);
            animator.SetFloat(horizontal, h, 0.1f, Time.deltaTime);
        }

        public void CanRotate()
        {
            canRotate = true;
        }

        public void StopRotation()
        {
            canRotate = false;
        }

        public void PlayTargetAnimation(string targetAnimation,bool isInterracting)
        {
            animator.applyRootMotion = isInterracting;
            animator.SetBool("isInterracting", isInterracting);
            animator.CrossFade(targetAnimation, 0.2f);
        }


        void Update()
        {



        }
    }
}
