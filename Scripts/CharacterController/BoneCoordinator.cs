using System;
using UnityEngine;
using UnityEngine.Events;

namespace DoubTech.Multiplayer
{
    public class BoneCoordinator : MonoBehaviour
    {
        [SerializeField] private UnityEvent<Transform> onHeadSet;
        private Animator animator;

        public Animator Animator
        {
            get => animator;
            set
            {
                if (animator != value)
                {
                    animator = value;
                    if (animator)
                    {
                        onHeadSet?.Invoke(animator.GetBoneTransform(HumanBodyBones.Head));
                    }
                }
            }
        }

        private void OnEnable()
        {
            if (!animator)
            {
                Animator = GetComponentInChildren<Animator>();
            }
        }
    }
}