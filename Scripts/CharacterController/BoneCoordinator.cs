using System;
using DoubTech.MCC.IK;
using UnityEngine;
using UnityEngine.Events;

namespace DoubTech.MCC.Utilities
{
    public class BoneCoordinator : MonoBehaviour
    {
        [SerializeField] private Transform animatorParent;
        [SerializeField] private UnityEvent<Transform> onHeadSet;
        private IAnimatorProvider animator;

        private void OnEnable()
        {
            if (animatorParent) animator = animatorParent.GetComponentInChildren<IAnimatorProvider>();
            else animator = GetComponentInChildren<IAnimatorProvider>();
            animator.OnAnimatorChanged += OnAnimatorChanged;
        }

        private void OnAnimatorChanged(Animator animator)
        {
            onHeadSet?.Invoke(animator.GetBoneTransform(HumanBodyBones.Head));
        }
    }
}
