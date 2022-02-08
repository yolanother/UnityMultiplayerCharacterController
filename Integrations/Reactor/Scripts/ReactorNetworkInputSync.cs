#if REACTOR
using DoubTech.MCC.Input;
using KS.Reactor;
using KS.Reactor.Client.Unity;
using UnityEngine;

namespace DoubTech.MCC.Integrations.Reactor
{
    public class ReactorNetworkInputSync : ksEntityScript, IPlayerInputSync, IPlayerAnimSync, ICharacterController
    {
        [ksEditable] private uint playerType = 1;
        [ksEditable] private uint propertyStart = 1000;

        public float CameraAngle
        {
            get => Properties[propertyStart + 1].Float;
            set => Properties[propertyStart + 1].Float = value;
        }

        public bool Jump 
        {
            get => Properties[propertyStart + 2].Bool;
            set => Properties[propertyStart + 2].Bool = value;
        }
        
        public bool AnalogMovement 
        {
            get => Properties[propertyStart + 3].Bool;
            set => Properties[propertyStart + 3].Bool = value;
        }
        
        public Vector2 Move 
        {
            get => Properties[propertyStart + 4].Vector2;
            set => Properties[propertyStart + 4].Vector2 = value;
        }
        
        public bool Sprint
        {
            get => Properties[propertyStart + 5].Bool;
            set => Properties[propertyStart + 5].Bool = value;
        }
        
        public Vector2 Look 
        {
            get => Properties[propertyStart + 6].Vector2;
            set => Properties[propertyStart + 6].Vector2 = value;
        }

        public bool Aim
        {
            get;
            set;
        }
        public bool Crouch { get; set; }
        public bool FirePrimary { get; set; }
        public bool FireSecondary { get; set; }
        public bool Use { get; set; }

        public bool AnimSyncJump 
        {
            get => Properties[propertyStart + 7].Bool;
            set => Properties[propertyStart + 7].Bool = value;
        }
        
        public bool AnimSyncFreeFall 
        {
            get => Properties[propertyStart + 8].Bool;
            set => Properties[propertyStart + 8].Bool = value;
        }
        
        public float AnimSyncSpeed 
        {
            get => Properties[propertyStart + 9].Float;
            set => Properties[propertyStart + 9].Float = value;
        }
        
        public float AnimSyncMotionSpeed 
        {
            get => Properties[propertyStart + 10].Float;
            set => Properties[propertyStart + 10].Float = value;
        }
        
        public bool AnimSyncIsGrounded 
        {
            get => Properties[propertyStart + 11].Bool;
            set => Properties[propertyStart + 11].Bool = value;
        }

        public Vector3 Position { 
            get => Properties[propertyStart + 12].Vector3;
            set => Properties[propertyStart + 12].Vector3 = value;
        }
        public Quaternion Rotation
        {
            get => Properties[propertyStart + 13].Quaternion;
            set => Properties[propertyStart + 13].Quaternion = value;
        }

        public Vector3 Velocity
        {
            get => Properties[propertyStart + 14].Vector3; 
            set => Properties[propertyStart + 14].Vector3 = value;
        }
        void ICharacterController.Move(Vector3 target)
        {
            //CharacterController.Move(target);
        }
    }
}
#endif