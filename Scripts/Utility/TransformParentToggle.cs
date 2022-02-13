using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TransformParentToggle : MonoBehaviour
{
    [SerializeField] private bool isTransformA;
    [SerializeField] private Transform targetTransformA;
    [SerializeField] private Transform targetTransformB;

    private void Awake()
    {
        IsTransformA = isTransformA;
    }

    public bool IsTransformA
    {
        get => isTransformA;
        set
        {
            if (value) SwitchToTransformA();
            else SwitchToTransformB();
        }
    }

    public bool IsTransformB
    {
        get => !IsTransformA;
        set => IsTransformA = value;
    }

    public void SwitchToTransformA()
    {
        isTransformA = true;
        transform.parent = targetTransformA;
        transform.localPosition = Vector3.zero;
        transform.localEulerAngles = Vector3.zero;
    }

    public void SwitchToTransformB()
    {
        isTransformA = false;
        transform.parent = targetTransformB;
        transform.localPosition = Vector3.zero;
        transform.localEulerAngles = Vector3.zero;
    }

    public void Toggle()
    {
        if(isTransformA) SwitchToTransformA();
        else SwitchToTransformB();
    }
}