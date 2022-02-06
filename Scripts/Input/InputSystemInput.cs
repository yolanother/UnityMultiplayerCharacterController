using UnityEngine;
using UnityEngine.Events;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

namespace DoubTech.Networking
{
	public class InputSystemInput : MonoBehaviour
	{
		[Header("Input Configuration")]
		[SerializeField] public float sensitivity = 1;
		[SerializeField] public bool invertY = false;
	
	
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
			if (!InputEnabled)
			{
				MoveInput(Vector2.zero);
				return;
			}
			
			MoveInput(value.Get<Vector2>());
		}

		public void OnLook(InputValue value)
		{
			if (!InputEnabled)
			{
				LookInput(Vector2.zero);
				return;
			}
			
			if (cursorInputForLook)
			{
				var delta = value.Get<Vector2>() * sensitivity;
				if (invertY) delta.y *= -1;
				LookInput(delta);
			}
		}

		public void OnJump(InputValue value)
		{
			JumpInput(InputEnabled && value.isPressed);
		}

		public void OnSprint(InputValue value)
		{
			SprintInput(InputEnabled && value.isPressed);
		}

        public void OnChangeCamera(InputValue value)
        {
	        if(InputEnabled && value.isPressed) ChangeCamera();
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
			Debug.Log(newLookDirection);
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
