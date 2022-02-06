using System;
using DoubTech.MCC.Input;
using UnityEngine;

namespace MessyJammersADF.Com.Doubtech.Unity.Mirrorcharactercontroller.Animations
{
    public class BaseAnimationSync : MonoBehaviour, IPlayerAnimSync
    {
        [SerializeField] private Animator animator;
        [SerializeField] private Transform animatorParent;
        
        private int _animIDSpeed = Animator.StringToHash("Speed");
        private int _animIDGrounded = Animator.StringToHash("Grounded");
        private int _animIDJump = Animator.StringToHash("Jump");
        private int _animIDFreeFall = Animator.StringToHash("FreeFall");
        private int _animIDMotionSpeed = Animator.StringToHash("MotionSpeed");

        private void OnEnable()
        {
            if (!animator)
            {
                if(animatorParent) animator = animatorParent.GetComponentInChildren<Animator>();
                else animator = GetComponentInChildren<Animator>();
            }
        }

        public virtual bool AnimSyncJump
        {
            get => animator.GetBool(_animIDJump);
            set => animator.SetBool(_animIDJump, value);
        }
        public virtual bool AnimSyncFreeFall
        {
            get => animator.GetBool(_animIDFreeFall);
            set => animator.SetBool(_animIDFreeFall, value);
        }
        public virtual float AnimSyncSpeed
        {
            get => animator.GetFloat(_animIDSpeed);
            set => animator.SetFloat(_animIDSpeed, value);
        }
        public virtual float AnimSyncMotionSpeed
        {
            get => animator.GetFloat(_animIDMotionSpeed);
            set => animator.SetFloat(_animIDMotionSpeed, value);
        }
        public virtual bool AnimSyncIsGrounded
        {
            get => animator.GetBool(_animIDGrounded);
            set => animator.SetBool(_animIDGrounded, value);
        }
    }
}
