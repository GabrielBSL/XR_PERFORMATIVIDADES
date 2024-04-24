using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestGesture : MonoBehaviour
{
    [SerializeField] private TransformSampler target;
    [SerializeField] private float scale = 1f;

    public float GetValue()
    {
        return target.GetSpeed() * scale;
    }
}