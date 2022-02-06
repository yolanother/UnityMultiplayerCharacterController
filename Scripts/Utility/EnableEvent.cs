using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnableEvent : MonoBehaviour
{
    [SerializeField] private UnityEvent onEnable = new UnityEvent();
    [SerializeField] private UnityEvent onDisable = new UnityEvent();

    private void OnEnable()
    {
        onEnable.Invoke();
    }

    private void OnDisable()
    {
        onDisable.Invoke();
    }
}
