using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VelocityTracker : MonoBehaviour
{
    [SerializeField] private double scale = 10;
    private Vector3 lastPosition;
    private Vector3 currentPosition;
    private double deltaPosition;
    private double parameter;

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
        parameter = Math.Round(deltaPosition * scale, 3);

        Debug.Log($"deltaPosition = {parameter}");
        FMODAudioManager.SetWind(parameter);

        
    }
}