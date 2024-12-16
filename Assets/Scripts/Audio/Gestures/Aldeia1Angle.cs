using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Aldeia1Angle : MonoBehaviour
{
    public float value;
    private Transform headTransform;
    private Transform aldeia1Transform;

    void GetHeadTransform(Transform _headTransform){headTransform = _headTransform;}
    void GetAldeia1Transform(Transform _aldeia1Transform){aldeia1Transform = _aldeia1Transform;}

    void OnEnable()
    {
        GestureReferenceEvents.headTransform += GetHeadTransform;
        GestureReferenceEvents.aldeia1Transform += GetAldeia1Transform;
    }
    void OnDisable()
    {
        GestureReferenceEvents.headTransform -= GetHeadTransform;
        GestureReferenceEvents.aldeia1Transform -= GetAldeia1Transform;
    }

    void Update()
    {
        value = Vector3.Dot
        (
            headTransform.forward, 
            aldeia1Transform.position - headTransform.position
        );
    }
}

