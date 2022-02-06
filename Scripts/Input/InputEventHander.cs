using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace MessyJammersADF.Com.Doubtech.Unity.Mirrorcharactercontroller.Input
{
    public class InputEventHander : MonoBehaviour
    {
        [Header("Movement")]
        [SerializeField] private UnityEvent<Vector2> onMove = new UnityEvent<Vector2>();
        [SerializeField] private UnityEvent<Vector2> onLook = new UnityEvent<Vector2>();
        [SerializeField] private UnityEvent<bool> onSprint = new UnityEvent<bool>();
        [SerializeField] private UnityEvent onJump = new UnityEvent();

        [Header("Camera")]
        [SerializeField] private UnityEvent onChangeCamera = new UnityEvent();

        [Header("Menu")]
        [SerializeField] private UnityEvent onMenu = new UnityEvent();

        private bool jump = false;
        private bool changeCamera = false;
        private bool menu;

        private void ButtonPressHandler(InputValue value, ref bool state, UnityEvent e)
        {
            var newState = value.isPressed;
            if (newState != state)
            {
                state = newState;
                if(state) e.Invoke();
            }
        }

        void OnMove(InputValue value) => onMove.Invoke(value.Get<Vector2>());
        void OnLook(InputValue value) => onLook.Invoke(value.Get<Vector2>());
        void OnJump(InputValue value) => ButtonPressHandler(value, ref jump, onJump);
        void OnSprint(InputValue value) => onSprint.Invoke(value.isPressed);

        void OnChangeCamera(InputValue value) => ButtonPressHandler(value, ref changeCamera, onChangeCamera);
        
        void OnMenu(InputValue value) => ButtonPressHandler(value, ref menu, onMenu);
    }
}
