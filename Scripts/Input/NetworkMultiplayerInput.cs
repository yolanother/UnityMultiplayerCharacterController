using UnityEngine;
using UnityEngine.Events;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

namespace DoubTech.Networking
{
	public class NetworkMultiplayerInput : MonoBehaviour
	{
		[Header("Character Input Values")]
		public Vector2 move;
		public Vector2 look;
		public bool jump;
		public bool sprint;

		[Header("Movement Settings")]
		public bool analogMovement;

        [Header("Events")]
        public UnityEvent onChangeCamera = new UnityEvent();

#if !UNITY_IOS || !UNITY_ANDROID
		[Header("Mouse Cursor Settings")]
		public bool cursorInputForLook = true;
#endif

		public bool inputEnabled = true;
		public bool InputEnabled
		{
			get => inputEnabled;
			set => inputEnabled = value;
		}

#if ENABLE_INPUT_SYSTEM
		public void OnMove(InputValue value)
		{
			if (!InputEnabled) return;
			
			MoveInput(value.Get<Vector2>());
		}

		public void OnLook(InputValue value)
		{
			if (!InputEnabled) return;
			
			if (cursorInputForLook)
			{
				LookInput(value.Get<Vector2>());
			}
		}

		public void OnJump(InputValue value)
		{
			if (!InputEnabled) return;

			JumpInput(value.isPressed);
		}

		public void OnSprint(InputValue value)
		{
			if (!InputEnabled) return;
			
			SprintInput(value.isPressed);
		}

        public void OnChangeCamera(InputValue value)
        {
	        if (!InputEnabled) return;
	        
            if(value.isPressed) ChangeCamera();
        }
#else
	// old input sys if we do decide to have it (most likely wont)...
#endif

        private void ChangeCamera()
        {
            onChangeCamera.Invoke();
        }

		public void MoveInput(Vector2 newMoveDirection)
		{
			move = newMoveDirection;
		}

		public void LookInput(Vector2 newLookDirection)
		{
			look = newLookDirection;
		}

		public void JumpInput(bool newJumpState)
		{
			jump = newJumpState;
		}

		public void SprintInput(bool newSprintState)
		{
			sprint = newSprintState;
		}
	}
}
