using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace DoubTech.MCC.IK
{
    public class IKTarget : MonoBehaviour
    {
        [SerializeField] public Transform target;
        [SerializeField] public float lerpSpeed = 4;
        [SerializeField] private IKTargetType ikTargetType;
        
        [SerializeField] private float targetWeight = 1;

        [SerializeField] private UnityEvent<float> onChangeTargetWeight;

        [SerializeField] private bool reparentToTaggedTransform;
        [SerializeField] private string targetTag;
        private Transform targetTagTransform;

        public IKTargetType IKTargetType => ikTargetType;

        public bool ReparentToTaggedTransform
        {
            get => reparentToTaggedTransform;
            set
            {
                reparentToTaggedTransform = value;
                if (reparentToTaggedTransform && !string.IsNullOrEmpty(targetTag))
                {
                    if (!targetTagTransform)
                    {
                        targetTagTransform = GameObject.FindWithTag(targetTag)?.transform;
                    }

                    if (targetTagTransform)
                    {
                        transform.parent = targetTagTransform;
                        transform.localPosition = Vector3.zero;
                        transform.localEulerAngles = Vector3.zero;
                    }
                    else
                    {
                        reparentToTaggedTransform = false;
                    }
                }
            }
        }

        public float TargetWeight
        {
            get => targetWeight;
            set
            {
                targetWeight = value;
                onChangeTargetWeight.Invoke(value);
            }
        }

        private void LateUpdate()
        {
            if (reparentToTaggedTransform) return;
            
            if (target)
            {
                var lerp = lerpSpeed > 0 ? lerpSpeed * Time.deltaTime : 1;
                var smoothLerpfactor = Mathf.SmoothStep(0, 1, lerp);
                transform.position = Vector3.Lerp(transform.position, target.position, smoothLerpfactor);
                transform.rotation = Quaternion.Slerp(transform.rotation, target.rotation, smoothLerpfactor);
            }
        }
    }

    [Flags]
    public enum IKTargetType
    {
        None,
        Aim = 1,
        RightHandIK = 2,
        RightHandIKHint = 4,
        LeftHandIK = 8,
        LeftHandIKHint = 16
    }
}