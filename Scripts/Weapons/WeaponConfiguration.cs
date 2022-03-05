using System;
using UnityEngine;

namespace DoubTech.MCC.Weapons
{
    [CreateAssetMenu(fileName = "WeaponConfig", menuName = "DoubTech/MCC/Weapon Configuration", order = 0)]
    public class WeaponConfiguration : ScriptableObject
    {
        [SerializeField] public AnimatorOverrideController controller;
        
        [SerializeField] public AnimationClip[] deathAnimations;
        [SerializeField] public AnimationClip[] hitAnimations;

        [Header("Equip/Unequip Animations")]
        [SerializeField] public AnimationClip equip;
        [SerializeField] public float equipGrabTime;
        [SerializeField] public AnimationClip unequip;
        [SerializeField] public float unequpReleaseTime;
        
        [SerializeField] public AnimationClip reload;

        [Header("Right Hand Attachments")]
        [SerializeField] public BodyAttachment rightHandAttachment;
        [SerializeField] public BodyAttachment rightHandAttachmentAim;
        [SerializeField] public BodyAttachment rightHandAttachmentFirstPerson;
        [SerializeField] public BodyAttachment rightHandAttachmentFirstPersonAim;

        [Header("Left Hand Attachments")]
        [SerializeField] public BodyAttachment leftHandAttachment;
        [SerializeField] public BodyAttachment leftHandAttachmentAim;
        [SerializeField] public BodyAttachment leftHandAttachmentFirstPerson;
        [SerializeField] public BodyAttachment leftHandAttachmentFirstPersonAim;

        [Header("Unequipped Attachment")]
        [SerializeField] public HumanBodyBones unequippedBone;
        [SerializeField] public Vector3 unequippedPosition;
        [SerializeField] public Vector3 unequippedRotation;

        public BodyAttachment GetRightHandAttachment(bool isFps, bool aim)
        {
            var attachment = aim ? rightHandAttachmentAim : rightHandAttachment;
            if (isFps)
            {
                attachment =
                    aim ? rightHandAttachmentFirstPersonAim : rightHandAttachmentFirstPerson;
            }

            return attachment;
        }

        public BodyAttachment GetLeftHandAttachment(bool isFps, bool aim)
        {
            var attachment = aim ? leftHandAttachmentAim : leftHandAttachment;
            if (isFps)
            {
                attachment =
                    aim ? leftHandAttachmentFirstPersonAim : leftHandAttachmentFirstPerson;
            }

            return attachment;
        }
    }

    [Serializable]
    public class BodyAttachment
    {
        [SerializeField] public bool attachToBodyAttachment;
        [SerializeField] public bool attachToBonePosition;
        [SerializeField] public HumanBodyBones ikBone;
        [SerializeField] public Vector3 ikPosition;
        [SerializeField] public Vector3 ikRotation;
        [SerializeField] public Vector3 ikHintPosition;
        [SerializeField] public Vector3 ikHintRotation;
    }
}
