using UnityEngine;

namespace DoubTech.MCC.Weapons
{
    [CreateAssetMenu(fileName = "WeaponConfig", menuName = "DoubTech/MCC/Weapon Configuration", order = 0)]
    public class WeaponConfiguration : ScriptableObject
    {
        [SerializeField] public AnimationClip[] deathAnimations;
        [SerializeField] public AnimationClip[] hitAnimations;
        [SerializeField] public AnimationClip equip;
        [SerializeField] public AnimationClip unequip;
        [SerializeField] public AnimatorOverrideController controller;
        [SerializeField] public AnimationClip reload;
    }
}