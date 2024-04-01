using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VelocityTrackerAcceleration : MonoBehaviour
{
    [SerializeField] private int sampleSize = 10;
    [SerializeField] private double scale = 10;

    private Queue<double> speedQueue = new Queue<double>();
    private Vector3 lastPosition;
    private Vector3 currentPosition;
    private double deltaPosition;

    private double currentSpeed;
    private double oldestSpeed;
    private double deltaSpeed;

    private double acceleration;
    private double parameter;
    
    void Start()
    {
        currentPosition = transform.position;
    }

    void Update()
    {
        lastPosition = currentPosition;
        currentPosition = transform.position;
        deltaPosition = Vector3.Magnitude(currentPosition - lastPosition);

        currentSpeed = deltaPosition / Time.deltaTime;
        speedQueue.Enqueue(currentSpeed);
        while (speedQueue.Count > sampleSize) oldestSpeed = speedQueue.Dequeue();
        deltaSpeed = currentSpeed - oldestSpeed;
        acceleration = deltaSpeed / (speedQueue.Count * Time.deltaTime);
        
        parameter = Math.Round(acceleration * scale, 3);
        
        Debug.Log($"deltaPosition = {parameter}");
        FMODAudioManager.SetWind(parameter);
    }
}