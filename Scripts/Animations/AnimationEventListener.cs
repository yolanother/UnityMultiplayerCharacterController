using System;
using DoubTech.ScriptableEvents.BuiltinTypes;
using UnityEngine;

namespace ReactorScripts.Common.MCC.Animations
{
    public class AnimationEventListener : MonoBehaviour
    {
        [Header("Foot Transforms")]
        [SerializeField] private Transform leftFootTransform;
        [SerializeField] private Transform rightFootTransform;
        
        [Header("Events")]
        [SerializeField] private TransformGameEvent onRightFoot;
        [SerializeField] private TransformGameEvent onLeftFoot;
        [SerializeField] private TransformGameEvent onJumpStart;
        [SerializeField] private TransformGameEvent onLand;

        private void OnValidate()
        {
            if (!leftFootTransform)
            {
                var animator = GetComponent<Animator>();
                leftFootTransform = animator.GetBoneTransform(HumanBodyBones.LeftFoot);
            }
            if (!rightFootTransform)
            {
                var animator = GetComponent<Animator>();
                rightFootTransform = animator.GetBoneTransform(HumanBodyBones.RightFoot);
            }
        }

        /// <summary>
        /// When on an object with an animator, this will be called by animator events.
        /// </summary>
        protected virtual void OnLeftFootDown()
        {
            onLeftFoot?.Invoke(leftFootTransform ? leftFootTransform : transform);
        }

        protected virtual void OnRightFootDown()
        {
            onRightFoot?.Invoke(rightFootTransform ? rightFootTransform : transform);
        }

        protected virtual void OnJumpStart()
        {
            onJumpStart?.Invoke(transform);
        }

        protected virtual void OnJumpLand()
        {
            onLand?.Invoke(transform);
        }
    }
}