using System;
using UnityEngine;

namespace DoubTech.MCC.Weapons
{
    public class ScreenAimTransform : MonoBehaviour
    {
        [SerializeField] private AimIKManager aim;
        [SerializeField] private float updateRate = .1f;

        private float lastUpdate;
        
        private void LateUpdate()
        {
            transform.position = aim.AimTarget;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawRay(aim.AimRaycast);
            Gizmos.DrawSphere(transform.position, .25f);
        }
    }
}