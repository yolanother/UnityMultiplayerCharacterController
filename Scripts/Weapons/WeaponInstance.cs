using System;
using System.Collections;
using DoubTech.MCC.IK;
using UnityEngine;
using UnityEngine.Events;

namespace DoubTech.MCC.Weapons
{
    public class WeaponInstance : MonoBehaviour
    {
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
        [SerializeField] private UnityEvent OnWeaponPrimaryFired = new UnityEvent();
        [SerializeField] private UnityEvent OnWeaponSecondaryFired = new UnityEvent();
        
        [SerializeField] private WeaponAnimationLayer[] animationLayers;
        private IAnimatorProvider animator;

        public Transform MuzzleTransform => muzzleTransform;

        private void OnEnable()
        {
            animator = GetComponentInParent<IAnimatorProvider>();
            Equip();
        }

        public void FirePrimary()
        {
            OnWeaponPrimaryFired.Invoke();
        }

        public void FireSecondary()
        {
            OnWeaponSecondaryFired.Invoke();
        }

        public void Equip()
        {
            if (!animator?.Animator) return;
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
