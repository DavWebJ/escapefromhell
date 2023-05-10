using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SUPERCharacter;
namespace BlackPearl
{

    public class AnimatorHandler : MonoBehaviour
    {
        public Animator animator;
        //PlayerLocomotion playerLocomotion;
        //InputHandler inputHandler;
        public SUPERCharacterAIO player;
        int vertical;
        int horizontal;
        public bool canRotate;


        private void Start()
        {
            Init();
        }
        public void Init()
        {
            //inputHandler = GetComponentInParent<InputHandler>();
            player = GetComponentInParent<SUPERCharacterAIO>();
            animator = GetComponentInChildren<Animator>();
            vertical = Animator.StringToHash("Speed");
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



        void Update()
        {



        }
    }
}
