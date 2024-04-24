using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fauna : MonoBehaviour
{
    private float value;

    [SerializeField] private TransformSampler left;
    [SerializeField] private TransformSampler right;

    void Update()
    {
        value = Vector3.Dot(left.currentSample.Item2 * Vector3.left,
                            right.currentSample.Item2 * Vector3.right);
    }

    public float GetValue()
    {
        return value;
    }
}