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

        public IKTarget LeftHandControl => leftHandControl;
        public IKTarget LeftHandHintControl => leftHandHintControl;
        public IKTarget RightHandControl => rightHandControl;

        public bool AttachAimToMainCameraTarget
        {
            get => attachAimToMainCameraTarget;
            set
            {
                attachAimToMainCameraTarget = value;
                Attach();
            }
        }

        public IKAttachmentPoint LeftHandAttachmentPoint
        {
            get => leftHandAttachmentPoint;
            set
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
                leftHandAttachmentPoint = value;
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
            }
        }
        
        public IKAttachmentPoint RightHandAttachmentPoint
        {
            get => rightHandAttachmentPoint;
            set
            {
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
                rightHandAttachmentPoint = value;
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
            }
        }

        public void Detach()
        {
            LeftHandAttachmentPoint = null;
            RightHandAttachmentPoint = null;
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
                        LeftHandAttachmentPoint = attachmentPoint;
                        break;
                    case IKAttachmentType.RightHand:
                        RightHandAttachmentPoint = attachmentPoint;
                        break;
                }
            }
            

            if (null != aimTarget)
            {
                aimTarget.ReparentToTaggedTransform = attachAimToMainCameraTarget;
            }
        }
    }
}
