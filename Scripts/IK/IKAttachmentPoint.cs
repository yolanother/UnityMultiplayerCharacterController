using UnityEngine;

namespace DoubTech.MCC.IK
{
    public class IKAttachmentPoint : MonoBehaviour
    {
        [Header("Attachment Details")]
        [SerializeField] private IKAttachmentType attachmentType;

        [SerializeField] private float targetTransformWeight = 1;
        [SerializeField] private float targetHintTransformWeight = 1;

        [SerializeField] private float targetTransformLerp = 0;
        [SerializeField] private float targetHintTransformLerp = 0;
        
        [Header("Target Transforms")]
        [SerializeField] private Transform targetTransform;
        [SerializeField] private Transform targetHintTransform;

        public IKAttachmentType AttachmentType => attachmentType;
        public Transform TargetTransform => targetTransform;
        public Transform TargetHintTransform => targetHintTransform;

        public float TargetTransformLerp => targetTransformLerp;
        public float TargetHintTransformLerp => targetHintTransformLerp;

        public float TargetTransformWeight => targetTransformWeight;
        public float TargetHintTransformWeight => targetHintTransformWeight;
    }

    public enum IKAttachmentType
    {
        LeftHand,
        RightHand
    }
}