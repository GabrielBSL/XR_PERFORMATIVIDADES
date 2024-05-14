using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookUpDown : MonoBehaviour
{
    public float value;
    private Transform headTransform;

    void GetHeadTransform(Transform _headTransform){headTransform = _headTransform;}

    void OnEnable()
    {
        GestureReferenceEvents.headTransform += GetHeadTransform;
    }
    void OnDisable()
    {
        GestureReferenceEvents.headTransform -= GetHeadTransform;
    }

    void Update()
    {
        value = Vector3.Dot(headTransform.rotation * Vector3.forward, Vector3.up) / 0.5f;  
    }
}

