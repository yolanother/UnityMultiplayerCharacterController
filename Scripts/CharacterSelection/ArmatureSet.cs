using System;
using DoubTech.MCC.IK;
using UnityEngine;
using UnityEngine.Events;

namespace DoubTech.MCC.CharacterSelection
{
    public class ArmatureSet : MonoBehaviour, IAnimatorProvider
    {
        [Header("AnimatorController")]
        [SerializeField] private RuntimeAnimatorController animatorController;
        
        [Header("Armatures Selection")]
        [SerializeField] private int selectedArmature;

        [SerializeField] private int selectedMaterial;

        [Header("Armature State")]
        [SerializeField] private bool fpsMode;

        [Header("Armatures")]
        [SerializeField] private Armature[] armatures;

        [Header("Events")]
        [SerializeField] private UnityEvent onArmatureChanged = new UnityEvent();
        
        private Animator animator;
        private Transform leftHandBone;
        private Transform rightHandBone;
        private Transform headBone;
        private Transform backBone;

        private Transform rightHandSlot;
        private Transform leftHandSlot;
        private Transform headSlot;
        private Transform backSlot;

        public Transform LeftHandBone => leftHandBone;
        public Transform RightHandBone => rightHandBone;
        public Transform HeadBone => headBone;
        public Transform BackBone => backBone;

        public Transform LeftHandSlot => leftHandSlot;
        public Transform RightHandSlot => rightHandSlot;
        public Transform HeadSlot => headSlot;
        public Transform BackSlot => backSlot;

        private int ArmatureMaxIndex => ArmatureCount - 1;
        private int MaterialMaxIndex => MaterialCount - 1;

        public int ArmatureCount => armatures?.Length ?? 0;
        public int MaterialCount => null != armatures && selectedArmature < armatures.Length ? armatures[selectedArmature].MaterialCount : 0;

        public bool FPSMode
        {
            get => fpsMode;
            set
            {
                fpsMode = value;
                if (selectedArmature < armatures.Length)
                {
                    var armature = armatures[selectedArmature];
                    armature.FPSMode = value;
                    animator = armature.GetComponentInChildren<Animator>(false);
                    animator.applyRootMotion = false;
                    HandleAnimatorChange(animator);
                }
            }
        }

        private void HandleAnimatorChange(Animator animator)
        {
            leftHandBone = animator.GetBoneTransform(HumanBodyBones.LeftHand);
            rightHandBone = animator.GetBoneTransform(HumanBodyBones.RightHand);
            headBone = animator.GetBoneTransform(HumanBodyBones.Head);
            backBone = animator.GetBoneTransform(HumanBodyBones.Chest);
            UpdateSlot(leftHandBone, "Left Hand Slot", ref leftHandSlot);
            UpdateSlot(rightHandBone, "Right Hand Slot", ref rightHandSlot);
            UpdateSlot(headBone, "Head Slot", ref headSlot);
            UpdateSlot(backBone, "Back Slot", ref backSlot);
            animator.runtimeAnimatorController = animatorController;
            OnAnimatorChanged?.Invoke(animator);
            onArmatureChanged.Invoke();
        }

        private void UpdateSlot(Transform bone, string slotName, ref Transform slot)
        {
            if (bone)
            {
                slot = bone.Find(slotName);
                if (!slot)
                {
                    var slotgo = new GameObject(slotName);
                    slot = slotgo.transform;
                    slot.transform.parent = bone;
                    slot.transform.localPosition = Vector3.zero;
                    slot.transform.localEulerAngles = Vector3.zero;
                    slot.transform.localScale = Vector3.one;
                }
            }
        }

        private void OnValidate()
        {
            SelectedArmatureIndex = selectedArmature;
            SelectedMaterialIndex = selectedMaterial;
            FPSMode = fpsMode;
        }

        private void OnEnable()
        {
            SelectedArmatureIndex = selectedArmature;
            SelectedMaterialIndex = selectedMaterial;
            FPSMode = fpsMode;
        }

        public int SelectedArmatureIndex
        {
            get => selectedArmature;
            set
            {
                if (value < armatures.Length)
                {
                    selectedArmature = value;
                    for (int i = 0; i < armatures.Length; i++)
                    {
                        armatures[i].gameObject.SetActive(i == value);
                        armatures[i].FPSMode = FPSMode;
                    }
                }
            }
        }

        public int SelectedMaterialIndex
        {
            get => selectedMaterial;
            set
            {
                if (selectedArmature < armatures.Length && value < armatures[selectedArmature].MaterialCount)
                {
                    selectedMaterial = value;
                    armatures[selectedArmature].SelectedMaterial = selectedMaterial;
                }
            }
        }

        public Animator Animator => animator;
        public event Action<Animator> OnAnimatorChanged;
    }
}
