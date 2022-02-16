using System;
using DoubTech.MCC;
using DoubTech.MCC.IK;
using UnityEngine;

namespace MessyJammersADF.Com.Doubtech.Unity.Mirrorcharactercontroller.Animations
{
    public class BaseAnimationSync : MonoBehaviour, IAnimationController
    {
        [SerializeField] private Transform animatorParent;

        private IAnimatorProvider animProvider;
        
        private bool currentAction;
        private bool currentLoop;
        
        private AnimatorOverrideController overrideController;

        private string CurrentActionName => currentAction ? "Action 1" : "Action 2";

        private AnimationClip Action
        {
            get => overrideController[CurrentActionName];
            set
            {
                currentAction = !currentAction;
                overrideController[CurrentActionName] = value;
            }
        }
        
        public void PlayAction(AnimationClip clip)
        {
            Action = clip;
            animProvider.Animator.CrossFade(CurrentActionName, .2f);
        }

        private void OnEnable()
        {
            if (null == animProvider)
            {
                if (animatorParent)
                {
                    animProvider = animatorParent.GetComponentInChildren<IAnimatorProvider>();
                }
                else
                {
                    animProvider = GetComponentInChildren<IAnimatorProvider>();
                }
            }
            
            if(null != animProvider) animProvider.OnAnimatorChanged += OnAnimatorChanged;
            if(animProvider?.Animator) OnAnimatorChanged(animProvider.Animator);
        }

        private void OnDisable()
        {
            if (null != animProvider) animProvider.OnAnimatorChanged -= OnAnimatorChanged;
        }

        private void OnAnimatorChanged(Animator animator)
        {
            if (animator.runtimeAnimatorController is AnimatorOverrideController oc)
            {
                overrideController = oc;
            }
            else
            {
                overrideController = new AnimatorOverrideController(animator.runtimeAnimatorController);
            }
        }

        public float Speed
        {
            get => animProvider.Animator.MCCSpeed();
            set => animProvider.Animator.MCCSpeed(value);
        }
        public float Horizontal
        {
            get => animProvider.Animator.MCCHorizontal();
            set => animProvider.Animator.MCCHorizontal(value);
        }
        public float Vertical
        {
            get => animProvider.Animator.MCCVertical();
            set => animProvider.Animator.MCCVertical(value);
        }
        public float Turn
        {
            get => animProvider.Animator.MCCTurn();
            set => animProvider.Animator.MCCTurn(value);
        }
        public bool Jump
        {
            get => animProvider.Animator.MCCJump();
            set => animProvider.Animator.MCCJump(value);
        }
        public bool Grounded
        {
            get => animProvider.Animator.MCCGrounded();
            set => animProvider.Animator.MCCGrounded(value);
        }
        public bool FreeFall
        {
            get => animProvider.Animator.MCCFreeFall();
            set => animProvider.Animator.MCCFreeFall(value);
        }
        public bool Crouch
        {
            get => animProvider.Animator.MCCCrouch();
            set => animProvider.Animator.MCCCrouch(value);
        }
    }

    public static class MCCAnimator
    {
        // Floats
        private static int _animIDSpeed = Animator.StringToHash("Speed");
        private static int _animIDHorizontal = Animator.StringToHash("Horizontal");
        private static int _animIDVertical = Animator.StringToHash("Vertical");
        private static int _animIDTurn = Animator.StringToHash("Turn");
        
        // Bools
        private static int _animIDGrounded = Animator.StringToHash("Grounded");
        private static int _animIDJump = Animator.StringToHash("Jump");
        private static int _animIDFreeFall = Animator.StringToHash("FreeFall");
        private static int _animIDCrouch = Animator.StringToHash("Crouch");

        public static float MCCSpeed(this Animator animator) => animator.GetFloat(_animIDSpeed);
        public static void MCCSpeed(this Animator animator, float value) => animator.SetFloat(_animIDSpeed, value);

        public static float MCCHorizontal(this Animator animator) => animator.GetFloat(_animIDHorizontal);
        public static void MCCHorizontal(this Animator animator, float value) => animator.SetFloat(_animIDHorizontal, value);

        public static float MCCVertical(this Animator animator) => animator.GetFloat(_animIDVertical);
        public static void MCCVertical(this Animator animator, float value) => animator.SetFloat(_animIDVertical, value);

        public static float MCCTurn(this Animator animator) => animator.GetFloat(_animIDTurn);
        public static void MCCTurn(this Animator animator, float value) => animator.SetFloat(_animIDTurn, value);

        public static bool MCCGrounded(this Animator animator) => animator.GetBool(_animIDGrounded);
        public static void MCCGrounded(this Animator animator, bool value) => animator.SetBool(_animIDGrounded, value);

        public static bool MCCJump(this Animator animator) => animator.GetBool(_animIDJump);
        public static void MCCJump(this Animator animator, bool value) => animator.SetBool(_animIDJump, value);

        public static bool MCCFreeFall(this Animator animator) => animator.GetBool(_animIDFreeFall);
        public static void MCCFreeFall(this Animator animator, bool value) => animator.SetBool(_animIDFreeFall, value);

        public static bool MCCCrouch(this Animator animator) => animator.GetBool(_animIDCrouch);
        public static void MCCCrouch(this Animator animator, bool value) => animator.SetBool(_animIDCrouch, value);
    }
}
