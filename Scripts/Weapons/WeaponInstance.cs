using System;
using System.Collections;
using DoubTech.MCC.CharacterSelection;
using DoubTech.MCC.IK;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.Events;

namespace DoubTech.MCC.Weapons
{
    public class WeaponInstance : MonoBehaviour
    {
        [Header("Config")]
        [SerializeField] private WeaponConfiguration weaponConfiguration;
        
        [Header("Firing")]
        [SerializeField] private Transform muzzleTransform;

        [Header("IK")]
        [SerializeField] private Transform leftHandIkTarget;
        [SerializeField] private Transform rightHandIkTarget;

        [Header("Equip Events")]
        [SerializeField] private UnityEvent OnWeaponEquipStarted = new UnityEvent();
        [SerializeField] private UnityEvent OnWeaponEquipped = new UnityEvent();
        
        [SerializeField] private UnityEvent OnWeaponUnequipStarted = new UnityEvent();
        [SerializeField] private UnityEvent OnWeaponUnequipped = new UnityEvent();
        
        [Header("Fire Events")]
        [SerializeField] private UnityEvent<Transform> OnWeaponPrimaryFired = new UnityEvent<Transform>();
        [SerializeField] private UnityEvent<Transform> OnWeaponSecondaryFired = new UnityEvent<Transform>();
        [SerializeField] private UnityEvent<Transform> OnWeaponPrimaryFiredWithNoAmmo = new UnityEvent<Transform>();
        [SerializeField] private UnityEvent<Transform> OnWeaponSecondaryFiredWithNoAmmo = new UnityEvent<Transform>();
        [SerializeField] private UnityEvent<Transform> OnWeaponReloading = new UnityEvent<Transform>();
        [SerializeField] private UnityEvent<Transform> OnWeaponReloaded = new UnityEvent<Transform>();
        
        [SerializeField] private WeaponAnimationLayer[] animationLayers;
        private IAnimatorProvider animator;

        public Transform MuzzleTransform => muzzleTransform;
        public WeaponConfiguration WeaponConfiguration => weaponConfiguration;

        private bool equipping = true;

        private void OnEnable()
        {
            animator = GetComponentInParent<IAnimatorProvider>();
            equipping = true;
        }

        public void FirePrimaryNoAmmo()
        {
            OnWeaponPrimaryFiredWithNoAmmo.Invoke(transform);
        }

        public void FireSecondaryNoAmmo()
        {
            OnWeaponSecondaryFiredWithNoAmmo.Invoke(transform);
        }

        public void FirePrimary()
        {
            OnWeaponPrimaryFired.Invoke(transform);
        }

        public void FireSecondary()
        {
            OnWeaponSecondaryFired.Invoke(transform);
        }

        public void Reloading()
        {
            OnWeaponReloading.Invoke(transform);
        }
        
        public void Reloaded()
        {
            OnWeaponReloaded.Invoke(transform);
        }

        private void Update()
        {
            if(equipping) Equip();
        }

        public void Equip()
        {
            if (!animator?.Animator) return;
            equipping = false;
            OnWeaponEquipStarted.Invoke();

            StopAllCoroutines();
            foreach (var layer in animationLayers)
            {
                StartCoroutine(LerpLayer(layer));
            }
            OnWeaponEquipped.Invoke();
        }

        public void Unequip()
        {
            OnWeaponUnequipStarted.Invoke();
            OnWeaponUnequipped.Invoke();
        }

        private void OnDisable()
        {
            StopAllCoroutines();
        }

        private IEnumerator LerpLayer(WeaponAnimationLayer layer)
        {
            bool lerpComplete = false;
            while (!UpdateLerpLayer(layer)) yield return null;

            var rigBuilder = animator.Animator.GetComponentInChildren<RigBuilder>();
            if (rigBuilder)
            {
                rigBuilder.enabled = false;
            }

            yield return new WaitForSeconds(1);

            rigBuilder.enabled = true;
        }

        private bool UpdateLerpLayer(WeaponAnimationLayer layer)
        {
            byte layerValue = (byte) layer.layers;
            bool done = true;

            for (int i = 1; layerValue > 0; i++)
            {
                if ((layerValue & 0x01) == 1)
                {
                    var weight = animator.Animator.GetLayerWeight(i);
                    weight += Time.deltaTime / layer.transitionSpeed;
                    done &= weight >= 1;
                    animator.Animator.SetLayerWeight(i, Mathf.Clamp01(weight));
                }
                layerValue = (byte) (layerValue >> 1);
            }

            return done;
        }
    }

    [Serializable]
    public class WeaponAnimationLayer
    {
        public WeaponAnimationLayers layers;
        public float weight;
        public float transitionSpeed = .25f;
    }

    [Flags]
    public enum WeaponAnimationLayers : byte
    {
        None,
        UpperBody = 1,
        RightArm = 2,
        LeftArm = 4,
        Head = 8
    }
}
