using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeftVelocity : MonoBehaviour
{
    public float value;
    private OldTransformSampler leftTransformSampler;
    
    [SerializeField] private float scale = 1f;
    [SerializeField] private float threshold = 0f;

    void GetLeftTransformSampler(OldTransformSampler _leftTransformSampler){leftTransformSampler = _leftTransformSampler;}

    void OnEnable()
    {
        GestureReferenceEvents.leftTransformSampler += GetLeftTransformSampler;
    }
    void OnDisable()
    {
        GestureReferenceEvents.leftTransformSampler -= GetLeftTransformSampler;
    }

    void Update()
    {
        value = (leftTransformSampler.GetSpeed() + leftTransformSampler.GetAngularSpeed())* scale - threshold;
    }
}