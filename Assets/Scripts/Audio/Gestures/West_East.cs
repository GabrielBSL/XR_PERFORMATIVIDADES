using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class West_East : MonoBehaviour
{
    public float value;
    private Transform headTransform;
    private Transform jangadaTransform;

    void GetHeadTransform(Transform _headTransform){headTransform = _headTransform;}
    void GetJangadaTransform(Transform _jangadaTransform){jangadaTransform = _jangadaTransform;}

    void OnEnable()
    {
        GestureReferenceEvents.headTransform += GetHeadTransform;
        GestureReferenceEvents.jangadaTransform += GetJangadaTransform;
    }
    void OnDisable()
    {
        GestureReferenceEvents.headTransform -= GetHeadTransform;
        GestureReferenceEvents.jangadaTransform -= GetJangadaTransform;
    }

    void Update()
    {
        value = Vector3.Dot(headTransform.rotation * Vector3.forward, jangadaTransform.up);
    }
}

