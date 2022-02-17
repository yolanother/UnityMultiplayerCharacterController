using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnableEvent : MonoBehaviour
{
    [SerializeField] private UnityEvent onEnable = new UnityEvent();
    [SerializeField] private UnityEvent onDisable = new UnityEvent();

    [SerializeField] private float delay = 0;

    private void OnEnable()
    {
        if (delay > 0) StartCoroutine(ExecuteEnable());
        else onEnable.Invoke();
    }

    private IEnumerator ExecuteEnable()
    {
        yield return new WaitForSeconds(delay);
        onEnable.Invoke();
    }

    private void OnDisable()
    {
        StopAllCoroutines();
        onDisable.Invoke();
    }
}
