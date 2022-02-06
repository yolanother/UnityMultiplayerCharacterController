using System;
using System.Collections.Generic;
using DoubTech.MCC.IK;
using UnityEditor;
using UnityEngine;

namespace DoubTech.MCC.Weapons
{
    public class ModelArmatures : MonoBehaviour, IAnimatorProvider
    {
        [SerializeField] internal List<Armature> armatures = new List<Armature>();
        [SerializeField] private Armature activeArmature;
        [SerializeField] private GameObject prefabInstance;

        private IAnimatorProvider animator;

        private Transform leftHandSlot;
        private Transform rightHandSlot;

        private Transform leftHandBone;
        private Transform rightHandBone;

        public Transform LeftHandSlot => leftHandSlot;
        public Transform RightHandSlot => rightHandSlot;

        private void OnValidate()
        {
            if (prefabInstance)
            {
                ActivePrefabInstance = prefabInstance;
            }
        }

        private void OnEnable()
        {
            if (prefabInstance)
            {
                ActivePrefabInstance = prefabInstance;
            }
        }

        public Armature ActiveArmature => activeArmature;

        public GameObject ActivePrefabInstance
        {
            get => prefabInstance;
            set
            {
                prefabInstance = value;
                activeArmature = armatures.Find(a => a.prefab == value || a.prefab.name == value.name);

                if (null != activeArmature)
                {
                    if (null != animator) animator.OnAnimatorChanged -= OnAnimatorChanged;
                    animator = value.GetComponentInChildren<IAnimatorProvider>();
                    animator.OnAnimatorChanged += OnAnimatorChanged;
                    leftHandBone = animator.Animator.GetBoneTransform(HumanBodyBones.LeftHand);
                    rightHandBone = animator.Animator.GetBoneTransform(HumanBodyBones.RightHand);

                    if (leftHandBone)
                    {
                        leftHandSlot = leftHandBone.Find("LeftHandSlot");
                        if (!leftHandSlot)
                        {
                            var slotgo = new GameObject("LeftHandSlot");
                            leftHandSlot = slotgo.transform;
                            leftHandSlot.transform.parent = leftHandBone;
                            leftHandSlot.transform.localPosition = activeArmature.leftHand.position;
                            leftHandSlot.transform.localEulerAngles =
                                activeArmature.leftHand.rotation;
                            leftHandSlot.transform.localScale = activeArmature.leftHand.scale;
                        }
                    }

                    if (rightHandBone)
                    {
                        rightHandSlot = rightHandBone.Find("RightHandSlot");
                        if (!rightHandSlot)
                        {
                            var slotgo = new GameObject("RightHandSlot");
                            rightHandSlot = slotgo.transform;
                            rightHandSlot.transform.parent = rightHandBone;
                            rightHandSlot.transform.localPosition =
                                activeArmature.rightHand.position;
                            rightHandSlot.transform.localEulerAngles =
                                activeArmature.rightHand.rotation;
                            rightHandSlot.transform.localScale = activeArmature.rightHand.scale;
                        }
                    }
                }
                else
                {
                    Debug.LogError($"{value.name} is not in the configured armatures.");
                }
            }
        }

        public Animator Animator => animator.Animator;
        public event Action<Animator> OnAnimatorChanged;
    }

    [Serializable]
    public class Armature
    {
        public GameObject prefab;
        public SlotAdjustment leftHand;
        public SlotAdjustment rightHand;
    }

    [Serializable]
    public class SlotAdjustment
    {
        public Vector3 position;
        public Vector3 rotation;
        public Vector3 scale = Vector3.one;
    }

    #if UNITY_EDITOR
    [CustomEditor(typeof(ModelArmatures))]
    public class ModelArmaturesEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (target is ModelArmatures mae)
            {
                var activeArmature = mae.armatures.Find(a => a.prefab == mae.ActiveArmature.prefab || a.prefab.name == mae.ActiveArmature.prefab.name);
                if (null != activeArmature && (mae.LeftHandSlot || mae.RightHandSlot) && GUILayout.Button("Copy Current Transforms"))
                {
                    activeArmature.leftHand.position = mae.LeftHandSlot.transform.localPosition;
                    activeArmature.leftHand.rotation = mae.LeftHandSlot.transform.localEulerAngles;
                    activeArmature.leftHand.scale = mae.LeftHandSlot.transform.localScale;

                    activeArmature.rightHand.position = mae.RightHandSlot.transform.localPosition;
                    activeArmature.rightHand.rotation = mae.RightHandSlot.transform.localEulerAngles;
                    activeArmature.rightHand.scale = mae.RightHandSlot.transform.localScale;
                }
                //UnityEditor.TransformWorldPlacementJSON:{"position":{"x":-0.05446725711226463,"y":1.4079991579055787,"z":0.21733804047107697},"rotation":{"x":0.8537428975105286,"y":0.45028454065322878,"z":-0.03904362767934799,"w":-0.2585402727127075},"scale":{"x":1.0,"y":1.0,"z":1.0}}
            }
        }
    }
    #endif
}
