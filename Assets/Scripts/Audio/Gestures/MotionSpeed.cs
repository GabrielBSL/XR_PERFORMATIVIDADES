using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MotionSpeed : MonoBehaviour
{
    private float value;

    [SerializeField] private float scale = 1f;
    [SerializeField] private TransformSampler hand;

    void Update()
    {
        value = (hand.GetSpeed() + hand.GetAngularSpeed())* scale;
    }

    public float GetValue()
    {
        return value;
    }
}