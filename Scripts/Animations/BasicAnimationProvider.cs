using System;
using DoubTech.MCC.IK;
using UnityEngine;

namespace MessyJammersADF.DoubTech.MCC.Animations
{
    [RequireComponent(typeof(Animator))]
    public class BasicAnimationProvider : MonoBehaviour, IAnimatorProvider
    {
        private Animator animator;

        private void Awake()
        {
            animator = GetComponent<Animator>();
            OnAnimatorChanged?.Invoke(animator);
        }

        public Animator Animator => animator;
        public event Action<Animator> OnAnimatorChanged;
    }
}
