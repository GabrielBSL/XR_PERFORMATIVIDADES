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
    public static Action<OldTransformSampler> rightTransformSampler;

    public static Action<Transform> leftTransform;
    public static Action<OldTransformSampler> leftTransformSampler;

    public static Action<Transform> headTransform;
    public static Action<OldTransformSampler> headTransformSampler;
    
    public static Action<Transform> jangadaTransform;
    public static Action<Transform> aldeia1Transform;
    public static Action<Transform> aldeia2Transform;
    public static Action<Transform> aldeia3Transform;
}