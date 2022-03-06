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
        [SerializeField] private IKAttachmentManager ikManager;
        [SerializeField] private Transform aimTarget;
        [SerializeField] public IKAttachmentPoint leftHandAttachmentPoint;
        [SerializeField] public IKAttachmentPoint rightHandAttachmentPoint;

        [Header("Model")]
        [SerializeField] private GameObject model;

        [Header("Equip Events")]
        [SerializeField] public UnityEvent OnWeaponEquipStarted = new UnityEvent();
        [SerializeField] public UnityEvent OnWeaponEquipped = new UnityEvent();

        [SerializeField] public UnityEvent OnWeaponUnequipStarted = new UnityEvent();
        [SerializeField] public UnityEvent OnWeaponUnequipped = new UnityEvent();

        [Header("Fire Events")]
        [SerializeField] public UnityEvent<Transform> OnWeaponPrimaryFired = new UnityEvent<Transform>();
        [SerializeField] public UnityEvent<Transform> OnWeaponSecondaryFired = new UnityEvent<Transform>();
        [SerializeField] public UnityEvent<Transform> OnWeaponPrimaryFiredWithNoAmmo = new UnityEvent<Transform>();
        [SerializeField] public UnityEvent<Transform> OnWeaponSecondaryFiredWithNoAmmo = new UnityEvent<Transform>();
        [SerializeField] public UnityEvent<Transform> OnWeaponReloading = new UnityEvent<Transform>();
        [SerializeField] public UnityEvent<Transform> OnWeaponReloaded = new UnityEvent<Transform>();

        [Header("Animations")]
        [SerializeField] private WeaponAnimationLayer[] animationLayers;
        private IAnimationController animController;
        private IAnimatorProvider animator;

        public Transform MuzzleTransform => muzzleTransform;
        public WeaponConfiguration WeaponConfiguration => weaponConfiguration;
        public bool IsEquipped => equipped;
        public bool IsEquipping => equipping;

        private bool equipping = true;

        private Transform rightHandAttachmentPivot;
        private Transform leftHandAttachmentPivot;
        private Transform rightHandHint;
        private Transform leftHandHint;
        private bool equipped;
        private MaterialSet materialSet;

        private IWeaponEventHandler[] weaponEventhandlers;

        private Transform equippedParent;
        private void Awake()
        {
            equippedParent = transform.parent;
            weaponEventhandlers = GetComponents<IWeaponEventHandler>();
        }

        private void OnEnable()
        {
            animator = GetComponentInParent<IAnimatorProvider>();
            animator.OnAnimatorChanged += OnAnimatorChanged;
            OnAnimatorChanged(animator.Animator);
        }

        private void TriggerEvent(Action<IWeaponEventHandler> handler)
        {
            for (int i = 0; i < weaponEventhandlers.Length; i++)
            {
                if(null != handler) handler.Invoke(weaponEventhandlers[i]);
            }
        }

        private void OnAnimatorChanged(Animator animator)
        {
            animController = animator.GetComponentInParent<IAnimationController>();
            materialSet = animator.GetComponent<MaterialSet>();
            if(equipped) AttachWeapon();
            else
            {
                ParentToUnequipped();
            }
        }

        public void FirePrimaryNoAmmo()
        {
            OnWeaponPrimaryFiredWithNoAmmo.Invoke(transform);
            TriggerEvent(e => e.OnWeaponFiredWithNoAmmo());
        }

        public void FireSecondaryNoAmmo()
        {
            OnWeaponSecondaryFiredWithNoAmmo.Invoke(transform);
            TriggerEvent(e => e.OnWeaponSecondaryFiredWithNoAmmo());
        }

        public void FirePrimary()
        {
            OnWeaponPrimaryFired.Invoke(transform);
            TriggerEvent(e => e.OnWeaponPrimaryFired());
        }

        public void FireSecondary()
        {
            OnWeaponSecondaryFired.Invoke(transform);
            TriggerEvent(e => e.OnWeaponSecondaryFired());
        }

        public void Reloading()
        {
            OnWeaponReloading.Invoke(transform);
            TriggerEvent(e => e.OnWeaponReloading());
        }

        public void Reloaded()
        {
            OnWeaponReloaded.Invoke(transform);
            TriggerEvent(e => e.OnWeaponReloaded());
        }

        private void FixedUpdate()
        {

            if (rightHandAttachmentPivot)
            {
                rightHandAttachmentPivot.LookAt(aimTarget);
            }
        }

        public void Equip()
        {
            StopAllCoroutines();
            StartCoroutine(EquipAsync());
        }
        
        public IEnumerator EquipAsync()
        {
            if (equipped) yield break;
            if (!animator?.Animator) yield break;

            equipping = true;
            equipped = true;
            
            OnWeaponEquipStarted.Invoke();
            TriggerEvent(e => e.OnWeaponEquipStarted());

            ikManager.Detach();

            var rig = animator.Animator.GetComponentInChildren<Rig>();
            if(rig) yield return Tween.Fade(rig.weight, false, .2f, w => rig.weight = w);
            animController?.PlayAction(weaponConfiguration.equip);
            yield return new WaitForSeconds(weaponConfiguration.equipGrabTime);
            transform.parent = equippedParent;
            transform.localPosition = Vector3.zero;
            transform.localEulerAngles = Vector3.zero;
            yield return new WaitForSeconds(weaponConfiguration.equip.length - weaponConfiguration.equipGrabTime);

            foreach (var layer in animationLayers)
            {
                StartCoroutine(LerpLayer(layer));
            }
            OnWeaponEquipped.Invoke();
            TriggerEvent(e => e.OnWeaponEquipped());

            AttachWeapon();
            if(rig) yield return Tween.Fade(rig.weight, true, .2f, w => rig.weight = w);
            equipping = false;
        }

        private void AttachWeapon()
        {
            var rightHandAttachment =
                weaponConfiguration.GetRightHandAttachment(materialSet.FPSMode, false);
            if (rightHandAttachment.attachToBodyAttachment)
            {
                if (!rightHandAttachmentPivot)
                {
                    rightHandAttachmentPivot = new GameObject("Right Hand Attachment Pivot").transform;
                    rightHandAttachmentPoint =
                        new GameObject("Right Hand Attachment Point").AddComponent<IKAttachmentPoint>();
                    rightHandHint = new GameObject("Right Hand Attachment Hint").transform;
                    rightHandHint.parent = rightHandAttachmentPivot;
                    rightHandAttachmentPoint.AttachmentType = IKAttachmentType.RightHand;
                }

                Attach(rightHandAttachmentPivot, rightHandAttachmentPoint, rightHandHint, rightHandAttachment);
            }
            ikManager.RightHandAttachmentPoint = rightHandAttachmentPoint;


            var leftHandAttachment =
                weaponConfiguration.GetLeftHandAttachment(materialSet.FPSMode, false);
            if (leftHandAttachment.attachToBodyAttachment)
            {
                if (!leftHandAttachmentPivot)
                {
                    leftHandAttachmentPivot = new GameObject("Left Hand Attachment Pivot").transform;
                    leftHandHint = new GameObject("Left Hand Attachment Hint").transform;
                    leftHandHint.parent = leftHandAttachmentPivot;
                    leftHandAttachmentPoint =
                        new GameObject("Left Hand Attachment Point").AddComponent<IKAttachmentPoint>();
                    leftHandAttachmentPoint.AttachmentType = IKAttachmentType.LeftHand;
                }

                Attach(leftHandAttachmentPivot, leftHandAttachmentPoint, leftHandHint,
                    leftHandAttachment);
            }
            ikManager.LeftHandAttachmentPoint = leftHandAttachmentPoint;
        }

        private void Attach(Transform pivot, IKAttachmentPoint ikAttachmentPoint, Transform hint, BodyAttachment attachment)
        {
            var attachmentPoint = ikAttachmentPoint.transform;
            if (attachment.attachToBonePosition)
            {
                pivot.parent =
                    animator.Animator.GetBoneTransform(attachment.ikBone);
            }
            else
            {
                pivot.parent = animator.Animator.transform;
            }

            pivot.localPosition = attachment.ikPosition;
            attachmentPoint.transform.parent = pivot;
            ikAttachmentPoint.TargetTransform = attachmentPoint;
            ikAttachmentPoint.TargetHintTransform = hint;
            attachmentPoint.parent = pivot;
            attachmentPoint.localPosition = Vector3.zero;
            attachmentPoint.localEulerAngles = attachment.ikRotation;
            hint.localPosition = attachment.ikHintPosition;
            hint.localEulerAngles = attachment.ikHintRotation;
        }

        public void Unequip(Action onUnequipComplete)
        {
            StopAllCoroutines();
            StartCoroutine(UnequipAsync(onUnequipComplete));
        }
        
        private IEnumerator UnequipAsync(Action onUnequipComplete) 
        {
            if (!equipped) yield break;
            equipping = true;
            equipped = false;
            var rig = animator.Animator.GetComponentInChildren<Rig>();
            if(rig) yield return Tween.Fade(rig.weight, false, .2f, w => rig.weight = w);
            ikManager.Detach();
            animController?.PlayAction(weaponConfiguration.unequip);
            OnWeaponUnequipStarted.Invoke();
            TriggerEvent(e => e.OnWeaponUnequipStarted());

            yield return new WaitForSeconds(weaponConfiguration.unequpReleaseTime);
            ParentToUnequipped();
            yield return new WaitForSeconds(weaponConfiguration.unequip.length - weaponConfiguration.unequpReleaseTime);
            
            OnWeaponUnequipped.Invoke();
            TriggerEvent(e => e.OnWeaponUnequipped());
            if(rig) yield return Tween.Fade(rig.weight, true, .2f, w => rig.weight = w);
            equipping = false;
            onUnequipComplete.Invoke();
        }

        private void ParentToUnequipped()
        {
            transform.parent = animator.Animator.GetBoneTransform(weaponConfiguration.unequippedBone);
            transform.localPosition = weaponConfiguration.unequippedPosition;
            transform.localEulerAngles = weaponConfiguration.unequippedRotation;
        }

        private void OnDestroy()
        {
            if(leftHandAttachmentPoint) Destroy(leftHandAttachmentPoint);
            if(rightHandAttachmentPoint) Destroy(rightHandAttachmentPoint.gameObject);
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

    public interface IWeaponEventHandler
    {
        void OnWeaponEquipStarted();
        void OnWeaponEquipped();
        void OnWeaponUnequipStarted();
        void OnWeaponUnequipped();
        void OnWeaponPrimaryFired();
        void OnWeaponSecondaryFired();
        void OnWeaponReloading();
        void OnWeaponReloaded();
        void OnWeaponFiredWithNoAmmo();
        void OnWeaponSecondaryFiredWithNoAmmo();
    }

    public class Tween
    {
        public static IEnumerator Fade(float value, bool fadeIn, float duration, Action<float> valueChanged)
        {
            while (fadeIn && value < 1 || !fadeIn && value > 0)
            {
                value += (fadeIn ? 1 : -1) * Time.deltaTime / duration;
                valueChanged?.Invoke(value);
                yield return null;
            }

            value = Mathf.Clamp01(value);
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
