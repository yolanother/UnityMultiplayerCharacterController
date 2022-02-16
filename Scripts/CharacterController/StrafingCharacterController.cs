using DoubTech.MCC.IK;
using DoubTech.MCC.Input;
using DoubTech.MCC.Utilities;
using UnityEngine;
using UnityEngine.Events;

namespace DoubTech.MCC
{
    public class StrafingCharacterController : BaseNetworkCharacterController
    {
        [Header("Player")] [Tooltip("Move speed of the character in m/s")]
        public float MoveSpeed = 2.0f;

        [Tooltip("Sprint speed of the character in m/s")]
        public float SprintSpeed = 5.335f;

        [Tooltip("How fast the character turns to face movement direction")] [Range(0.0f, 0.3f)]
        public float RotationSmoothTime = 0.12f;

        [Tooltip("Acceleration and deceleration")]
        public float SpeedChangeRate = 10.0f;

        [Space(10)] [Tooltip("The height the player can jump")]
        public float JumpHeight = 1.2f;

        [Tooltip("The character uses its own gravity value. The engine default is -9.81f")]
        public float Gravity = -15.0f;

        [Space(10)]
        [Tooltip(
            "Time required to pass before being able to jump again. Set to 0f to instantly jump again")]
        public float JumpTimeout = 0.50f;

        [Tooltip(
            "Time required to pass before entering the fall state. Useful for walking down stairs")]
        public float FallTimeout = 0.15f;

        [Header("Player Grounded")]
        [Tooltip(
            "If the character is grounded or not. Not part of the CharacterController built in grounded check")]
        public bool Grounded = true;

        [Tooltip("Useful for rough ground")] public float GroundedOffset = -0.14f;

        [Tooltip(
            "The radius of the grounded check. Should match the radius of the CharacterController")]
        public float GroundedRadius = 0.28f;

        [Tooltip("What layers the character uses as ground")]
        public LayerMask GroundLayers;

        [SerializeField] private bool hasHeadTracking;
        [SerializeField] private float maxHeadRotation = 45;

        // player
        private float _speed;
        private float _animationBlend;
        private float _targetRotation = 0.0f;
        private float _rotationVelocity;
        private float _verticalVelocity;
        private float _terminalVelocity = 53.0f;

        private float lastrotation;

        // timeout deltatime
        private float _jumpTimeoutDelta;
        private float _fallTimeoutDelta;

        private Animator _animator;
        private ICharacterController _controller;

        private bool _hasAnimator;
        private IPlayerInfoProvider playerInfo;
        private IAnimationController player;

        private void Start()
        {
            _controller = GetComponentInParent<ICharacterController>();
            playerInfo = GetComponentInParent<IPlayerInfoProvider>();
            player = GetComponentInParent<IAnimationController>();

            // reset our timeouts on start
            _jumpTimeoutDelta = JumpTimeout;
            _fallTimeoutDelta = FallTimeout;
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            ResetAnimator();
        }

        public void ResetAnimator()
        {
            _animator = GetComponentInChildren<IAnimatorProvider>()?.Animator;
            _hasAnimator = _animator;
        }

        protected override void Update()
        {
            base.Update();
            if (playerInfo.IsServer)
            {
                if (!playerInfo.IsLocalPlayer)
                {
                    Debug.Log("Updating movement by " + playerInfo.PlayerId);
                }
                JumpAndGravity();
                GroundedCheck();
                Move();
            }
        }

        private void GroundedCheck()
        {
            player.Grounded = _controller.Grounded;
        }

        private void Move()
        {
            // set target speed based on move speed, sprint speed and if sprint is pressed
            float targetSpeed = playerInputSync.Sprint ? SprintSpeed : MoveSpeed;

            // a simplistic acceleration and deceleration designed to be easy to remove, replace, or iterate upon

            // note: Vector2's == operator uses approximation so is not floating point error prone, and is cheaper than magnitude
            // if there is no input, set the target speed to 0
            if (playerInputSync.Move == Vector2.zero) targetSpeed = 0.0f;

            // a reference to the players current horizontal velocity
            float currentHorizontalSpeed =
                new Vector3(_controller.Velocity.x, 0.0f, _controller.Velocity.z).magnitude;

            float speedOffset = 0.1f;
            float inputMagnitude = playerInputSync.AnalogMovement ? playerInputSync.Move.magnitude : 1f;

            // accelerate or decelerate to target speed
            if (currentHorizontalSpeed < targetSpeed - speedOffset ||
                currentHorizontalSpeed > targetSpeed + speedOffset)
            {
                // creates curved result rather than a linear one giving a more organic speed change
                // note T in Lerp is clamped, so we don't need to clamp our speed
                _speed = Mathf.Lerp(currentHorizontalSpeed, targetSpeed * inputMagnitude,
                    Time.deltaTime * SpeedChangeRate);

                // round speed to 3 decimal places
                _speed = Mathf.Round(_speed * 1000f) / 1000f;
            }
            else
            {
                _speed = targetSpeed;
            }

            _animationBlend = Mathf.Lerp(_animationBlend, targetSpeed,
                Time.deltaTime * SpeedChangeRate);

            // normalise input direction
            Vector3 inputDirection = new Vector3(playerInputSync.Move.x, 0.0f, playerInputSync.Move.y).normalized;

            // note: Vector2's != operator uses approximation so is not floating point error prone, and is cheaper than magnitude
            // if there is a move input rotate player when the player is moving
            ///if (!hasHeadTracking || playerInputSync.Move != Vector2.zero || Mathf.Abs(playerInputSync.CameraAngle - transform.eulerAngles.y) > maxHeadRotation)
            //{
                _targetRotation = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg +
                                  playerInputSync.CameraAngle;
                float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, playerInputSync.CameraAngle,
                    ref _rotationVelocity, RotationSmoothTime);

                // rotate to face input direction relative to camera position
                _controller.Rotation = Quaternion.Euler(0.0f, rotation, 0.0f);
            //}

            Vector3 targetDirection =
                Quaternion.Euler(0.0f, _targetRotation, 0.0f) * Vector3.forward;

            // move the player
            _controller.Move(targetDirection.normalized * (_speed * Time.deltaTime) +
                             new Vector3(0.0f, _verticalVelocity, 0.0f) * Time.deltaTime);

            if (_animationBlend <= MoveSpeed)
            {
                player.Speed = Mathf.Clamp01(_animationBlend / MoveSpeed * 1.02f - .01f);
            }
            else
            {
                player.Speed = Mathf.Clamp((_animationBlend - MoveSpeed) / (SprintSpeed - MoveSpeed) + 1, 1, 2);
            }

            var horizontal = player.Horizontal + inputDirection.x.Remap(-.7f, .7f, -1, 1) * Time.deltaTime;
            var vertical = player.Vertical + inputDirection.z.Remap(-.7f, .7f, -1, 1) * Time.deltaTime;
            player.Horizontal = Mathf.Clamp01(horizontal);
            player.Vertical = Mathf.Clamp01(vertical);
            player.Turn = Mathf.Clamp(rotation - lastrotation, -1, 1);
            lastrotation = rotation;
        }

        private void JumpAndGravity()
        {
            if (Grounded)
            {
                // reset the fall timeout timer
                _fallTimeoutDelta = FallTimeout;

                player.Jump = false;
                player.FreeFall = false;

                // stop our velocity dropping infinitely when grounded
                if (_verticalVelocity < 0.0f)
                {
                    _verticalVelocity = -2f;
                }

                // Jump
                if (playerInputSync.Jump && _jumpTimeoutDelta <= 0.0f)
                {
                    // the square root of H * -2 * G = how much velocity needed to reach desired height
                    _verticalVelocity = Mathf.Sqrt(JumpHeight * -2f * Gravity);

                    player.Jump = true;
                }

                // jump timeout
                if (_jumpTimeoutDelta >= 0.0f)
                {
                    _jumpTimeoutDelta -= Time.deltaTime;
                }
            }
            else
            {
                // reset the jump timeout timer
                _jumpTimeoutDelta = JumpTimeout;

                // fall timeout
                if (_fallTimeoutDelta >= 0.0f)
                {
                    _fallTimeoutDelta -= Time.deltaTime;
                }
                else
                {
                    // update animator if using character
                    if (_hasAnimator)
                    {
                        player.FreeFall = true;
                    }
                }

                // if we are not grounded, do not jump
                playerInputSync.Jump = false;
            }

            // apply gravity over time if under terminal (multiply by delta time twice to linearly speed up over time)
            if (_verticalVelocity < _terminalVelocity)
            {
                _verticalVelocity += Gravity * Time.deltaTime;
            }
        }

        private void OnDrawGizmosSelected()
        {
            Color transparentGreen = new Color(0.0f, 1.0f, 0.0f, 0.35f);
            Color transparentRed = new Color(1.0f, 0.0f, 0.0f, 0.35f);

            if (Grounded) Gizmos.color = transparentGreen;
            else Gizmos.color = transparentRed;

            // when selected, draw a gizmo in the position of, and matching radius of, the grounded collider
            if(null != _controller) {
                Gizmos.DrawSphere(
                new Vector3(_controller.Position.x, _controller.Position.y - GroundedOffset,
                    _controller.Position.z), GroundedRadius);

            }
        }
    }

    public interface IAnimationController
    {
        float Speed { get; set; }
        float Horizontal { get; set; }
        float Vertical { get; set; }
        float Turn { get; set; }
        bool Jump { get; set; }
        bool Grounded { get; set; }
        bool FreeFall { get; set; }
        bool Crouch { get; set; }

        public void PlayAction(AnimationClip clip);
    }
}