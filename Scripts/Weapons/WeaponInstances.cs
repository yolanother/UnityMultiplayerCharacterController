#if UNITY_EDITOR
using UnityEditor;
#endif
using System;
using DoubTech.MCC.IK;
using UnityEngine;

namespace DoubTech.MCC.Weapons
{
    public class WeaponInstances : MonoBehaviour
    {
        [Header("Active Weapon")]
        [SerializeField] private int activeLeftWeapon = -1;
        [SerializeField] private int activeRightWeapon = -1;

        [Header("Instances")]
        [SerializeField] private WeaponInstance[] rightHandInstances;
        [SerializeField] private WeaponInstance[] leftHandInstances;

        [Header("Armature")]
        [SerializeField] public ModelArmatures armatureRoot;
        [SerializeField] public Transform leftHandInstanceRoot;
        [SerializeField] public Transform rightHandInstanceRoot;

        private void Awake()
        {
            for(int i = 0; i < leftHandInstances.Length; i++)
            {
                var instance = leftHandInstances[i];
                instance.gameObject.SetActive(false);
            }
            for(int i = 0; i < rightHandInstances.Length; i++)
            {
                var instance = rightHandInstances[i];
                instance.gameObject.SetActive(false);
            }
        }

        private void OnEnable()
        {
            armatureRoot.OnAnimatorChanged += OnAnimatonChanged;
            OnAnimatonChanged(armatureRoot.Animator);
        }

        private void OnAnimatonChanged(Animator animator)
        {
            Reposition(true);
            for (int i = 0; i < leftHandInstances.Length; i++)
            {
                var instance = leftHandInstances[i];
                instance.gameObject.SetActive(i == activeLeftWeapon);
            }

            for (int i = 0; i < rightHandInstances.Length; i++)
            {
                var instance = rightHandInstances[i];
                instance.gameObject.SetActive(i == activeRightWeapon);
            }
        }

        public void Reposition(bool reparent)
        {
            armatureRoot.ActivePrefabInstance = armatureRoot.transform.GetChild(0).gameObject;

            Reposition(leftHandInstanceRoot, armatureRoot.LeftHandSlot, reparent);
            Reposition(rightHandInstanceRoot, armatureRoot.RightHandSlot, reparent);
        }

        internal void Reposition(Transform instanceRootTransform, Transform slotTransform, bool reparent)
        {
            var parent = instanceRootTransform.parent;
            instanceRootTransform.parent = slotTransform;
            instanceRootTransform.localPosition = Vector3.zero;
            instanceRootTransform.localEulerAngles = Vector3.zero;
            if (!reparent)
            {
                instanceRootTransform.parent = parent;
            }
        }
    }

    #if UNITY_EDITOR
    [CustomEditor(typeof(WeaponInstances))]
    public class WeaponInstancesEditor : Editor
    {
        [SerializeField] private ModelArmatures armatures;

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (target is WeaponInstances instances && instances.armatureRoot)
            {
                if (GUILayout.Button("Position") && instances.armatureRoot.transform.childCount > 0)
                {
                    instances.Reposition(false);
                }
            }
        }
    }
    #endif
}
