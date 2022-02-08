using System;
using UnityEngine;

namespace DoubTech.MCC.IK
{
    public class IKTarget : MonoBehaviour
    {
        [SerializeField] public Transform target;

        private void LateUpdate()
        {
            if(target) transform.position = target.position;
        }
    }
}