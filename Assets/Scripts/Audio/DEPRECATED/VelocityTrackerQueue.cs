using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VelocityTrackerQueue : MonoBehaviour
{

    [SerializeField] private int sampleSize = 10;
    [SerializeField] private double scale = 10;
    private Queue<Vector3> positionQueue = new Queue<Vector3>();
    private Vector3 oldestPosition;
    private Vector3 currentPosition;
    private double deltaPosition;
    private double parameter;

    void Update()
    {
        currentPosition = transform.position;
        positionQueue.Enqueue(currentPosition);
        while (positionQueue.Count > sampleSize) oldestPosition = positionQueue.Dequeue();
        deltaPosition = Vector3.Magnitude(currentPosition - oldestPosition);
        parameter = Math.Round(deltaPosition * scale, 3);
        
        Debug.Log($"deltaPosition = {parameter}");
        FMODAudioManager.SetWind(parameter);
    }
}