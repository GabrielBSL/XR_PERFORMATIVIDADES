using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RainStick : MonoBehaviour
{
    private float value;

    [SerializeField] private float scale = 1f;
    [SerializeField] private TransformSampler headset;


    void Update()
    {
        value = headset.GetSpeed() * scale;
    }

    public float GetValue()
    {
        return value;
    }
}