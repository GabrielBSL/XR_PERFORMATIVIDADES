using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MotionSpeed : MonoBehaviour
{
    private float value;

    [SerializeField] private float scale = 1f;
    [SerializeField] private float threshold = 1f;
    [SerializeField] private TransformSampler hand;

    public float GetValue()
    {
        value = (hand.GetSpeed() + hand.GetAngularSpeed())* scale - threshold;
        return value;
    }
}