using UnityEngine;

/* Note: animations are called via the controller for both the character and capsule using animator null checks
 */

namespace DoubTech.MCC.Input
{
    public class NetworkInputSync : MonoBehaviour
    {
        [SerializeField] private InputSystemInput _input;
        [SerializeField] private Camera _mainCamera;

        private IPlayerInfoProvider playerInfo;
        private IPlayerInputSync inputSync;
        private double TOLERANCE = .0001f;

        private void OnEnable()
        {
            // get a reference to our main camera
            if (_mainCamera == null)
            {
                _mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
            }
            _input = FindObjectOfType<InputSystemInput>();
            playerInfo = GetComponentInParent<IPlayerInfoProvider>();
            inputSync = GetComponentInParent<IPlayerInputSync>();
        }

        private void Update()
        {
            if (!playerInfo.IsLocalPlayer) return;
            if (null == inputSync) return;
            if (null == _input) return;
            
            if (inputSync.Look != _input.look)
            {
                inputSync.Look = _input.look;
            }

            if (inputSync.Sprint != _input.sprint)
            {
                inputSync.Sprint = _input.sprint;
            }

            if (inputSync.Jump != _input.jump)
            {
                inputSync.Jump = _input.jump;
            }

            if (inputSync.Move != _input.move)
            {
                inputSync.Move = _input.move;
            }

            if (inputSync.Aim != _input.aim)
            {
                inputSync.Aim = _input.aim;
            }

            if (inputSync.Crouch != _input.crouch)
            {
                inputSync.Crouch = _input.crouch;
            }

            if (inputSync.FirePrimary != _input.firePrimary)
            {
                inputSync.FirePrimary = _input.firePrimary;
            }

            if (inputSync.FireSecondary != _input.fireSecondary)
            {
                inputSync.FireSecondary = _input.fireSecondary;
            }

            if (inputSync.AnalogMovement != _input.analogMovement)
            {
                inputSync.AnalogMovement = _input.analogMovement;
            }
            inputSync.CameraAngle = _mainCamera.transform.eulerAngles.y;

            if (inputSync.ReloadPrimary != _input.reloadPrimary)
            {
                inputSync.ReloadPrimary = _input.reloadPrimary;
            }

            if (inputSync.ReloadSecondary != _input.reloadSecondary)
            {
                inputSync.ReloadSecondary = _input.reloadSecondary;
            }

            if (inputSync.NextWeapon != _input.nextWeapon)
            {
                inputSync.NextWeapon = _input.nextWeapon;
            }

            if (inputSync.PreviousWeapon != _input.previousWeapon)
            {
                inputSync.PreviousWeapon = _input.previousWeapon;
            }

            if (inputSync.Weapon0 != _input.weapon0) inputSync.Weapon0 = _input.weapon0;
            if (inputSync.Weapon1 != _input.weapon1) inputSync.Weapon1 = _input.weapon1;
            if (inputSync.Weapon2 != _input.weapon2) inputSync.Weapon2 = _input.weapon2;
            if (inputSync.Weapon3 != _input.weapon3) inputSync.Weapon3 = _input.weapon3;
            if (inputSync.Weapon4 != _input.weapon4) inputSync.Weapon4 = _input.weapon4;
            if (inputSync.Weapon5 != _input.weapon5) inputSync.Weapon5 = _input.weapon5;
            if (inputSync.Weapon6 != _input.weapon6) inputSync.Weapon6 = _input.weapon6;
            if (inputSync.Weapon7 != _input.weapon7) inputSync.Weapon7 = _input.weapon7;
            if (inputSync.Weapon8 != _input.weapon8) inputSync.Weapon8 = _input.weapon8;
            if (inputSync.Weapon9 != _input.weapon9) inputSync.Weapon9 = _input.weapon9;
        }

        private static float ClampAngle(float lfAngle, float lfMin, float lfMax)
        {
            if (lfAngle < -360f) lfAngle += 360f;
            if (lfAngle > 360f) lfAngle -= 360f;
            return Mathf.Clamp(lfAngle, lfMin, lfMax);
        }
    }

    public interface IPlayerInputSync
    {
        float CameraAngle { get; set; }
        bool Jump { get; set; }
        bool AnalogMovement { get; set; }
        Vector2 Move { get; set; }
        bool Sprint { get; set; }
        Vector2 Look { get; set; }
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