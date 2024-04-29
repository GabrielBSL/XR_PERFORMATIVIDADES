using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadTurn : MonoBehaviour
{
    private float value;

    [SerializeField] private TransformSampler head;

    void Update()
    {
        value = Vector3.Dot(head.currentSample.Item2 * Vector3.right, Vector3.forward);
    }

    public float GetValue()
    {
        return value;
    }
}