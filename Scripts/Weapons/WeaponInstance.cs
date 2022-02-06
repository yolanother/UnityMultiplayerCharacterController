using System;
using System.Collections;
using UnityEngine;

namespace DoubTech.MCC.Weapons
{
    public class WeaponInstance : MonoBehaviour
    {
        [SerializeField] private WeaponAnimationLayer[] animationLayers;
        private Animator animator;

        private void OnEnable()
        {
            animator = GetComponentInParent<Animator>();
            Equip();
        }

        public void Equip()
        {
            if (!animator) return;
            
            StopAllCoroutines();
            foreach (var layer in animationLayers)
            {
                StartCoroutine(LerpLayer(layer));
            }
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
                    var weight = animator.GetLayerWeight(i);
                    weight += Time.deltaTime / layer.transitionSpeed;
                    done &= weight >= 1;
                    animator.SetLayerWeight(i, Mathf.Clamp01(weight));
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
