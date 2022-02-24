using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace DoubTech.MCC.Utilities
{
    public class CursorLockManager : MonoBehaviour
    {
        [SerializeField] private bool lockOnEnable = true;
        [SerializeField] private Canvas detectionCanvas;

        [Header("Events")]
        [SerializeField] private UnityEvent onCursorLocked = new UnityEvent();
        [SerializeField] private UnityEvent onCursorUnlocked = new UnityEvent();

        private bool locked;

        private HashSet<string> cameraUnlocks = new HashSet<string>();

        public bool Locked => locked;

        private void OnEnable()
        {
            if(lockOnEnable) Lock();
        }

        private void OnDisable()
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }

        public void Lock(string lockName)
        {
            Debug.Log("Locked by " + lockName);
            cameraUnlocks.Remove(lockName);
            Lock();
        }

        public void Lock()
        {
            if (cameraUnlocks.Count == 0)
            {
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
                if(detectionCanvas) detectionCanvas.gameObject.SetActive(false);
                if (!locked)
                {
                    locked = true;
                    onCursorLocked.Invoke();
                }
            }
        }

        public void Unlock(string lockName)
        {
            Debug.Log("Unlocked by " + lockName);
            cameraUnlocks.Add(lockName);
            if(detectionCanvas) detectionCanvas.gameObject.SetActive(true);
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            if (locked)
            {
                locked = false;
                onCursorUnlocked.Invoke();
            }
        }
    }
}
