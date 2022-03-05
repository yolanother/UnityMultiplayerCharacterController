#if UNITY_EDITOR
using UnityEditor;
#endif
using System;
using System.Collections;
using DoubTech.MCC.CharacterSelection;
using UnityEngine;
using UnityEngine.Events;

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

        [Header("Events")]
        [SerializeField] private UnityEvent onWeaponChanged = new UnityEvent();
        [SerializeField] public UnityEvent<WeaponConfiguration> onWeaponConfigChanged = new UnityEvent<WeaponConfiguration>();

        public int LeftWeaponIndex
        {
            get => activeLeftWeapon;
            set
            {
                if (activeLeftWeapon != value)
                {
                    activeLeftWeapon = value;
                    for (int i = 0; i < leftHandInstances.Length; i++)
                    {
                        var instance = leftHandInstances[i];
                        instance.gameObject.SetActive(i == activeLeftWeapon);
                    }

                    onWeaponChanged.Invoke();
                }
            }
        }

        public WeaponInstance RightWeapon => activeRightWeapon >= 0 && activeRightWeapon < rightHandInstances.Length ? rightHandInstances[activeRightWeapon] : null;
        public WeaponInstance LeftWeapon => activeLeftWeapon >= 0 && activeLeftWeapon < leftHandInstances.Length ? leftHandInstances[activeLeftWeapon] : null;
        
        public int RightWeaponIndex
        {
            get => activeRightWeapon;
            set
            {
                if (activeRightWeapon != value && value >= 0 && value < rightHandInstances.Length)
                {
                    SetWeaponIndex(value);
                }
            }
        }

        private void SetWeaponIndex(int value)
        {
            var activeWeapon = RightWeapon;
            if (activeWeapon && activeWeapon.IsEquipping) return;
            if (activeWeapon && activeWeapon.IsEquipped)
            {
                rightHandInstances[activeRightWeapon].Unequip(() => SetWeaponIndex(value));
                return;
            }
                    
            activeRightWeapon = value;
            onWeaponChanged.Invoke();
            var weapon = RightWeapon;
            if(null != weapon)
            {
                onWeaponConfigChanged.Invoke(weapon.WeaponConfiguration);
                weapon.Equip();
            }
        }

        private void OnEnable()
        {
            armatureRoot.OnAnimatorChanged += OnAnimationChanged;
            
            Reposition(true);
            StartCoroutine(Init());
        }

        private IEnumerator Init()
        {
            yield return new WaitForEndOfFrame();
            Reequip(activeLeftWeapon, activeRightWeapon);
        }

        public void Reequip(int leftWeapon, int rightWeapon)
        {
            activeLeftWeapon = -1;
            activeRightWeapon = -1;
            LeftWeaponIndex = leftWeapon;
            RightWeaponIndex = rightWeapon;
        }

        private void OnAnimationChanged(Animator animator)
        {
            Reposition(true);

            RightWeaponIndex = activeRightWeapon;
            LeftWeaponIndex = activeLeftWeapon;
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

        public void FirePrimaryNoAmmo()
        {
            RightWeapon.FirePrimaryNoAmmo();
        }

        public void FireSecondaryNoAmmo()
        {
            RightWeapon.FireSecondaryNoAmmo();
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
