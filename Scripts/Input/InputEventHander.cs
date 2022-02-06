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
        [SerializeField] private UnityEvent<bool> onAim = new UnityEvent<bool>();

        [Header("Menu")]
        [SerializeField] private UnityEvent onMenu = new UnityEvent();

        private bool jump = false;
        private bool changeCamera = false;
        private bool menu;
        private bool aim;
        private Vector2 move;
        private Vector2 look;
        private bool sprint;

        private void ButtonPressHandler(InputValue value, ref bool state, UnityEvent e)
        {
            var newState = value.isPressed;
            if (newState != state)
            {
                state = newState;
                if(state) e.Invoke();
            }
        }

        private void ButtonStateHandler(InputValue value, ref bool state, UnityEvent<bool> e)
        {
            var newState = value.isPressed;
            if (newState != state)
            {
                state = newState;
                e.Invoke(state);
            }
        }

        private void ButtonStateHandler(InputValue value, ref Vector2 state, UnityEvent<Vector2> e)
        {
            var newState = value.Get<Vector2>();
            if (newState != state)
            {
                state = newState;
                e.Invoke(state);
            }
        }

        void OnMove(InputValue value) => ButtonStateHandler(value, ref move, onMove);
        void OnLook(InputValue value) => ButtonStateHandler(value, ref look, onLook);
        void OnJump(InputValue value) => ButtonPressHandler(value, ref jump, onJump);
        void OnSprint(InputValue value) => ButtonStateHandler(value, ref sprint, onSprint);

        void OnChangeCamera(InputValue value) => ButtonPressHandler(value, ref changeCamera, onChangeCamera);
        
        void OnMenu(InputValue value) => ButtonPressHandler(value, ref menu, onMenu);
        void OnAim(InputValue value) => ButtonStateHandler(value, ref aim, onAim);
    }
}
