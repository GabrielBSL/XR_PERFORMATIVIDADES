using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadTurn2 : MonoBehaviour
{
    private float value;

    [SerializeField] private Transform head;
    
    public float GetValue()
    {
        value = Vector3.Dot(head.rotation * Vector3.forward, Vector3.down);
        return value;
    }
}