using System;
using DoubTech.MCC.IK;
using DoubTech.MCC.Input;
using UnityEngine;

namespace MessyJammersADF.Com.Doubtech.Unity.Mirrorcharactercontroller.Animations
{
    public class BaseAnimationSync : MonoBehaviour, IPlayerAnimSync
    {
        [SerializeField] private Transform animatorParent;

        private int _animIDSpeed = Animator.StringToHash("Speed");
        private int _animIDGrounded = Animator.StringToHash("Grounded");
        private int _animIDJump = Animator.StringToHash("Jump");
        private int _animIDFreeFall = Animator.StringToHash("FreeFall");
        private int _animIDMotionSpeed = Animator.StringToHash("MotionSpeed");

        private IAnimatorProvider animator;

        private void OnEnable()
        {
            if (null == animator)
            {
                animator = animatorParent.GetComponentInChildren<IAnimatorProvider>();
            }
        }

        public virtual bool AnimSyncJump
        {
            get => animator.Animator.GetBool(_animIDJump);
            set => animator.Animator.SetBool(_animIDJump, value);
        }
        public virtual bool AnimSyncFreeFall
        {
            get => animator.Animator.GetBool(_animIDFreeFall);
            set => animator.Animator.SetBool(_animIDFreeFall, value);
        }
        public virtual float AnimSyncSpeed
        {
            get => animator.Animator.GetFloat(_animIDSpeed);
            set => animator.Animator.SetFloat(_animIDSpeed, value);
        }
        public virtual float AnimSyncMotionSpeed
        {
            get => animator.Animator.GetFloat(_animIDMotionSpeed);
            set => animator.Animator.SetFloat(_animIDMotionSpeed, value);
        }
        public virtual bool AnimSyncIsGrounded
        {
            get => animator.Animator.GetBool(_animIDGrounded);
            set => animator.Animator.SetBool(_animIDGrounded, value);
        }
    }
}
