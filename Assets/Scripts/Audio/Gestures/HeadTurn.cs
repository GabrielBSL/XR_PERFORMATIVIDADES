using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadTurn : MonoBehaviour
{
    private float value;

    [SerializeField] private Transform head;
    [SerializeField] private Transform referenceTransform;
    
    public float GetValue()
    {
        value = Vector3.Dot(head.rotation * Vector3.forward, referenceTransform.right);
        return value;
    }
}