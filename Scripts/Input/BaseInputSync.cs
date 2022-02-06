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
    }
}
