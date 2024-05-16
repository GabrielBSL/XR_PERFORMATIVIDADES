using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Aldeia2Angle : MonoBehaviour
{
    public float value;
    private Transform headTransform;
    private Transform aldeia2Transform;

    void GetHeadTransform(Transform _headTransform){headTransform = _headTransform;}
    void GetAldeia2Transform(Transform _aldeia2Transform){aldeia2Transform = _aldeia2Transform;}

    void OnEnable()
    {
        GestureReferenceEvents.headTransform += GetHeadTransform;
        GestureReferenceEvents.aldeia2Transform += GetAldeia2Transform;
    }
    void OnDisable()
    {
        GestureReferenceEvents.headTransform -= GetHeadTransform;
        GestureReferenceEvents.aldeia2Transform -= GetAldeia2Transform;
    }

    void Update()
    {
        value = Vector3.Dot
        (
            headTransform.forward, 
            aldeia2Transform.position - headTransform.position
        );
    }
}

