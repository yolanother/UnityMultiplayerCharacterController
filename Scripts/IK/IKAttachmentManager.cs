using System;
using UnityEngine;
using UnityEngine.PlayerLoop;

namespace DoubTech.MCC.IK
{
    public class IKAttachmentManager : MonoBehaviour
    {
        [SerializeField] private IKAttachmentPoint leftHandAttachmentPoint;
        [SerializeField] private IKAttachmentPoint rightHandAttachmentPoint;

        [SerializeField] private IKTarget leftHandControl;
        [SerializeField] private IKTarget leftHandHintControl;
        [SerializeField] private IKTarget rightHandControl;
        [SerializeField] private IKTarget rightHandHintControl;
        [SerializeField] private IKTarget aimTarget;
        private IKTargetType currentTargetTypes;
        
        [SerializeField] public bool attachAimToMainCameraTarget;

        public bool AttachAimToMainCameraTarget
        {
            get => attachAimToMainCameraTarget;
            set
            {
                attachAimToMainCameraTarget = value;
                Attach();
            }
        }

        public void Detach()
        {
            if (leftHandControl)
            {
                leftHandControl.target = null;
                leftHandControl.TargetWeight = 0;
            }
            if (leftHandHintControl)
            {
                leftHandHintControl.target = null;
                leftHandHintControl.TargetWeight = 0;
            }
            if (rightHandControl)
            {
                rightHandControl.target = null;
                rightHandControl.TargetWeight = 0;
            }
            if (rightHandHintControl)
            {
                rightHandHintControl.target = null;
                rightHandHintControl.TargetWeight = 0;
            }
        }

        public void Attach()
        {
            // Detach current ik bindings
            Detach();

            // TODO: This could use an optimization
            var controlPoints = transform.GetComponentsInChildren<IKTarget>();
            foreach (var controlPoint in controlPoints)
            {
                switch (controlPoint.IKTargetType)
                {
                    case IKTargetType.LeftHandIK:
                        leftHandControl = controlPoint;
                        break;
                    case IKTargetType.LeftHandIKHint:
                        leftHandHintControl = controlPoint;
                        break;
                    case IKTargetType.RightHandIK:
                        rightHandControl = controlPoint;
                        break;
                    case IKTargetType.RightHandIKHint:
                        rightHandHintControl = controlPoint;
                        break;
                    case IKTargetType.Aim:
                        aimTarget = controlPoint;
                        break;
                }
            }

            leftHandAttachmentPoint = null;
            rightHandAttachmentPoint = null;
            
            var attachmentPoints = transform.GetComponentsInChildren<IKAttachmentPoint>();
            foreach (var attachmentPoint in attachmentPoints)
            {
                switch (attachmentPoint.AttachmentType)
                {
                    case IKAttachmentType.LeftHand:
                        leftHandAttachmentPoint = attachmentPoint;
                        break;
                    case IKAttachmentType.RightHand:
                        rightHandAttachmentPoint = attachmentPoint;
                        break;
                }
            }
            
            // Detach new ik bindings to attach to next target
            Detach();

            if (leftHandAttachmentPoint && leftHandAttachmentPoint.TargetTransform)
            {
                if (leftHandControl)
                {
                    leftHandControl.target = leftHandAttachmentPoint.TargetTransform;
                    leftHandControl.TargetWeight = leftHandAttachmentPoint.TargetTransformWeight;
                    leftHandControl.lerpSpeed = leftHandAttachmentPoint.TargetTransformLerp;
                }

                if (leftHandHintControl)
                {
                    leftHandHintControl.target = leftHandAttachmentPoint.TargetHintTransform;
                    leftHandHintControl.TargetWeight = leftHandAttachmentPoint.TargetHintTransformWeight;
                    leftHandHintControl.lerpSpeed = leftHandAttachmentPoint.TargetHintTransformLerp;
                }
            }

            if (rightHandAttachmentPoint && rightHandAttachmentPoint.TargetTransform)
            {
                if (rightHandControl)
                {
                    rightHandControl.target = rightHandAttachmentPoint.TargetTransform;
                    rightHandControl.TargetWeight = rightHandAttachmentPoint.TargetTransformWeight;
                    rightHandControl.lerpSpeed = rightHandAttachmentPoint.TargetTransformLerp;
                }

                if (leftHandHintControl)
                {
                    rightHandHintControl.target = rightHandAttachmentPoint.TargetHintTransform;
                    rightHandHintControl.TargetWeight = rightHandAttachmentPoint.TargetHintTransformWeight;
                    rightHandHintControl.lerpSpeed = rightHandAttachmentPoint.TargetHintTransformLerp;
                }
            }

            if (null != aimTarget)
            {
                aimTarget.ReparentToTaggedTransform = attachAimToMainCameraTarget;
            }
        }
    }
}