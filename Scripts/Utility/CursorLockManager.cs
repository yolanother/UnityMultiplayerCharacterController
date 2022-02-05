using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DoubTech.MCC.Utilities
{
    public class CursorLockManager : MonoBehaviour
    {
        [SerializeField] private bool lockOnEnable = true;
        [SerializeField] private Canvas detectionCanvas;

        private HashSet<string> cameraUnlocks = new HashSet<string>();

        private void OnEnable()
        {
            if(lockOnEnable) Lock();
        }

        public void Lock(string lockName)
        {
            cameraUnlocks.Remove(lockName);
            Lock();
        }

        public void Lock()
        {
            if (cameraUnlocks.Count == 0)
            {
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
                detectionCanvas.gameObject.SetActive(false);
            }
        }

        public void Unlock(string lockName)
        {
            detectionCanvas.gameObject.SetActive(true);
            cameraUnlocks.Add(lockName);
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
    }
}
