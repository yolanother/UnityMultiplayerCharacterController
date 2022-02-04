using DoubTech.MCC.Input;
using UnityEngine;

namespace DoubTech.MCC.CharacterController
{
    [RequireComponent(typeof(UnityEngine.CharacterController))]
    public class UnityCharacterController : MonoBehaviour, ICharacterController
    {
        private UnityEngine.CharacterController controller;

        private void Awake()
        {
            controller = GetComponent<UnityEngine.CharacterController>();
        }

        public Vector3 Position
        {
            get => controller.transform.position;
            set => controller.transform.position = value;
        }
        public Quaternion Rotation
        {
            get => controller.transform.rotation;
            set => controller.transform.rotation = value;
        }
        public Vector3 Velocity
        {
            get => controller.velocity;
            set { }
        }
        public void Move(Vector3 motion)
        {
            controller.Move(motion);
        }
    }
}
