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

        public void Lock(string lockName)
        {
            cameraUnlocks.Remove(lockName);
            Lock();
        }

        public void Lock()
        {
            if (cameraUnlocks.Count == 0)
            {
                if (!locked)
                {
                    locked = true;
                    onCursorLocked.Invoke();
                    Cursor.visible = false;
                    //Cursor.lockState = CursorLockMode.Locked;
                    detectionCanvas.gameObject.SetActive(false);
                }
            }
        }

        public void Unlock(string lockName)
        {
            if (locked)
            {
                locked = false;
                onCursorUnlocked.Invoke();
                detectionCanvas.gameObject.SetActive(true);
                cameraUnlocks.Add(lockName);
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
            }
        }
    }
}
