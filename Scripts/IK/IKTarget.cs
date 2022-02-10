using System;
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

        public IKTargetType IKTargetType => ikTargetType;

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
            if (target)
            {
                var lerp = lerpSpeed > 0 ? lerpSpeed * Time.deltaTime : 1;
                transform.position = Vector3.Lerp(transform.position, target.position, lerp);
                transform.rotation = Quaternion.Slerp(transform.rotation, target.rotation, lerp);
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