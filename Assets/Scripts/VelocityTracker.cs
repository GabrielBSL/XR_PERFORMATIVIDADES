using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VelocityTracker : MonoBehaviour
{

    private Vector3 lastPosition;
    private Vector3 currentPosition;
    private float deltaPosition;

    // Start is called before the first frame update
    void Start()
    {
        currentPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        lastPosition = currentPosition;
        currentPosition = transform.position;
        deltaPosition = Vector3.Magnitude(currentPosition - lastPosition);

        Debug.Log($"deltaPosition = {Math.Round((decimal)deltaPosition * 10, 2)}");
    }
}