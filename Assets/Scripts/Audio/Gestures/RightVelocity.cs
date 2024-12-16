using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RightVelocity : MonoBehaviour
{
    public float value;
    private TransformSampler rightTransformSampler;
    
    [SerializeField] private float scale = 1f;
    [SerializeField] private float threshold = 0f;

    void GetRightTransformSampler(TransformSampler _rightTransformSampler){rightTransformSampler = _rightTransformSampler;}

    void OnEnable()
    {
        GestureReferenceEvents.rightTransformSampler += GetRightTransformSampler;
    }
    void OnDisable()
    {
        GestureReferenceEvents.rightTransformSampler -= GetRightTransformSampler;
    }

    void Update()
    {
        value = (rightTransformSampler.GetSpeed() + rightTransformSampler.GetAngularSpeed())* scale - threshold;
    }
}