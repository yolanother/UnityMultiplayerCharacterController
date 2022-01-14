using System;
using UnityEngine;

namespace DoubTech.Networking
{
    public class TransformTracker : MonoBehaviour
    {
        [SerializeField] private bool trackPosition = true;
        [SerializeField] private bool trackRotation = true;
        
        [SerializeField] public Transform targetTransform;

        private void Update()
        {
            if (targetTransform)
            {
                if(trackPosition) transform.position = targetTransform.position;
                if(trackRotation) transform.rotation = targetTransform.rotation;
            }
        }

        public void Track(Transform transform) => targetTransform = transform;
    }
}