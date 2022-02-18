using DoubTech.MCC.Input;
using UnityEngine;

namespace DoubTech.MCC.Input
{
    public class BaseInputSync : MonoBehaviour, IPlayerInputSync
    {
        public virtual float CameraAngle { get; set; }
        public virtual bool Jump { get; set; }
        public virtual bool AnalogMovement { get; set; }
        public virtual Vector2 Move { get; set; }
        public virtual bool Sprint { get; set; }
        public virtual Vector2 Look { get; set; }
        public bool Aim { get; set; }
        public bool Crouch { get; set; }
        public bool FirePrimary { get; set; }
        public bool FireSecondary { get; set; }
        public bool ReloadPrimary { get; set; }
        public bool ReloadSecondary { get; set; }
        public bool NextWeapon { get; set; }
        public bool PreviousWeapon { get; set; }
        public bool Weapon0 { get; set; }
        public bool Weapon1 { get; set; }
        public bool Weapon2 { get; set; }
        public bool Weapon3 { get; set; }
        public bool Weapon4 { get; set; }
        public bool Weapon5 { get; set; }
        public bool Weapon6 { get; set; }
        public bool Weapon7 { get; set; }
        public bool Weapon8 { get; set; }
        public bool Weapon9 { get; set; }
        public bool Use { get; set; }
    }
}
