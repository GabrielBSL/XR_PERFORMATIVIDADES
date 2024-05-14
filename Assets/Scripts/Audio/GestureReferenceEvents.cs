using System;
using UnityEngine;

public static class GestureReferenceEvents
{
    public static Action<bool> calibrating;
    public static Action<float> userHeight;
    public static Action<float> userWingspan;
    public static Action<float> aldeiaBlend;
    public static Action<bool> jangadaMoving;

    public static Action<Transform> rightTransform;
    public static Action<TransformSampler> rightTransformSampler;

    public static Action<Transform> leftTransform;
    public static Action<TransformSampler> leftTransformSampler;

    public static Action<Transform> headTransform;
    public static Action<TransformSampler> headTransformSampler;
    
    public static Action<Transform> jangadaTransform;
}