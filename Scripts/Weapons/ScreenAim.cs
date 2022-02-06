using System;
using UnityEngine;

namespace DoubTech.MCC.Weapons
{
    public class ScreenAim : MonoBehaviour
    {
        [SerializeField] private Transform aimSource;
        [SerializeField] private LayerMask aimMask;
        [SerializeField] private Camera camera;
        [SerializeField] private int maxDistance = 1000;
        
        private Ray screenRay;

        private int lastFrame;
        
        public Ray ScreenRay
        {
            get
            {
                if (lastFrame != Time.frameCount && camera)
                {
                    lastFrame = Time.frameCount;
                    Vector2 screenCenter = new Vector2(Screen.width / 2f, Screen.height / 2f);
                    screenRay = camera.ScreenPointToRay(screenCenter);
                }

                return screenRay;
            }
        }
        
        public Ray AimRaycast
        {
            get
            {
                if (aimSource)
                {
                    return new Ray(aimSource.position, AimTarget);
                }

                return ScreenRay;
            }
        }

        private void Awake()
        {
            if (!camera) camera = Camera.main;
        }

        public Vector3 AimCollision
        {
            get
            {
                if (Physics.Raycast(ScreenRay, out var hit, maxDistance, aimMask))
                {
                    return hit.point;
                }

                return AimTarget;
            }
        }

        public Vector3 AimTarget
        {
            get
            {
                return ScreenRay.GetPoint(10000);
            }
        }
    }
}
