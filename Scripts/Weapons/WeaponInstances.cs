#if UNITY_EDITOR
using UnityEditor;
#endif
using System;
using DoubTech.MCC.CharacterSelection;
using UnityEngine;

namespace DoubTech.MCC.Weapons
{
    public class WeaponInstances : MonoBehaviour
    {
        [Header("Active Weapon")]
        [SerializeField] private int activeLeftWeapon = -1;
        [SerializeField] private int activeRightWeapon = -1;
        [SerializeField] private Transform activeMuzzleTransform;

        [Header("Instances")]
        [SerializeField] private WeaponInstance[] rightHandInstances;
        [SerializeField] private WeaponInstance[] leftHandInstances;

        [Header("Armature")]
        [SerializeField] public ArmatureSet armatureRoot;
        [SerializeField] public Transform leftHandInstanceRoot;
        [SerializeField] public Transform rightHandInstanceRoot;

        public WeaponInstance LeftWeapon => activeLeftWeapon >= 0 && activeLeftWeapon < leftHandInstances.Length ? leftHandInstances[activeLeftWeapon] : null;
        public WeaponInstance RightWeapon => activeRightWeapon >= 0 && activeRightWeapon < rightHandInstances.Length ? rightHandInstances[activeRightWeapon] : null;

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

        private void LateUpdate()
        {
            if (activeMuzzleTransform && RightWeapon)
            {
                activeMuzzleTransform.position = RightWeapon.MuzzleTransform.position;
            }
        }

        public void FirePrimary()
        {
            RightWeapon.FirePrimary();
        }

        public void FireSecondary()
        {
            RightWeapon.FireSecondary();
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
