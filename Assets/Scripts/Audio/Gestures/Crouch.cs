using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crouch : MonoBehaviour
{
    private float value;

    [SerializeField] private Transform headset;
    [SerializeField] private Transform ground;
    [SerializeField] private float userHeight = 1f;
    [SerializeField] private float crouchHeight = 1f;
    [SerializeField] private bool calibrating = true;


    void Update()
    {
        if(calibrating) userHeight = Vector3.Distance(headset.position, ground.position);
        value = (Vector3.Distance(headset.position, ground.position) - crouchHeight) / (userHeight - crouchHeight);
    }

    public float GetValue()
    {
        return value;
    }
}