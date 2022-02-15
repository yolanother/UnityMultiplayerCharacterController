using System;
using DoubTech.MCC;
using DoubTech.MCC.IK;
using DoubTech.MCC.Input;
using UnityEngine;

namespace MessyJammersADF.Com.Doubtech.Unity.Mirrorcharactercontroller.Animations
{
    public class BaseAnimationSync : MonoBehaviour, IAnimationController
    {
        [SerializeField] private Transform animatorParent;

        private IAnimatorProvider animator;

        private void OnEnable()
        {
            if (null == animator)
            {
                animator = animatorParent.GetComponentInChildren<IAnimatorProvider>();
            }
        }

        public float Speed
        {
            get => animator.Animator.MCCSpeed();
            set => animator.Animator.MCCSpeed(value);
        }
        public float Horizontal
        {
            get => animator.Animator.MCCHorizontal();
            set => animator.Animator.MCCHorizontal(value);
        }
        public float Vertical
        {
            get => animator.Animator.MCCVertical();
            set => animator.Animator.MCCVertical(value);
        }
        public float Turn
        {
            get => animator.Animator.MCCTurn();
            set => animator.Animator.MCCTurn(value);
        }
        public bool Jump
        {
            get => animator.Animator.MCCJump();
            set => animator.Animator.MCCJump(value);
        }
        public bool Grounded
        {
            get => animator.Animator.MCCGrounded();
            set => animator.Animator.MCCGrounded(value);
        }
        public bool FreeFall
        {
            get => animator.Animator.MCCFreeFall();
            set => animator.Animator.MCCFreeFall(value);
        }
        public bool Crouch
        {
            get => animator.Animator.MCCCrouch();
            set => animator.Animator.MCCCrouch(value);
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
