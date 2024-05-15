using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookUpDown : MonoBehaviour
{
    public float value;
    [SerializeField] private float maxAngle = Mathf.PI/4;
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
        value = Vector3.Dot(headTransform.forward, Vector3.up) / Mathf.Sin(maxAngle);
        //Debug.Log($"lookUpDown = {value}");
    }
}