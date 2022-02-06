using System;
using UnityEngine;

namespace DoubTech.MCC.IK
{
    public class AnimatorProvider : MonoBehaviour, IAnimatorProvider
    {
        [SerializeField] private Animator animator;
        public Animator Animator
        {
            get => animator;
            set
            {
                animator = value;
                OnAnimatorChanged?.Invoke(animator);
            }
        }

        public event Action<Animator> OnAnimatorChanged;
    }
}
