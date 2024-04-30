using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crouch : MonoBehaviour
{
    private float value;

    [SerializeField] private TransformSampler headset;
    [SerializeField] private Transform ground;
    [SerializeField] private float userHeight = 1f;
    [SerializeField] private float crouchHeight = 1f;
    [SerializeField] private bool calibrating = true;


    void Update()
    {
        if(calibrating) userHeight = Vector3.Distance(headset.currentSample.Item1, ground.position);
        value = (Vector3.Distance(headset.currentSample.Item1, ground.position) - crouchHeight) / (userHeight - crouchHeight);
    }

    public float GetValue()
    {
        return value;
    }
}