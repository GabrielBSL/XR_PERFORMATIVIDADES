using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VelocityTracker : MonoBehaviour
{
    [SerializeField] private Transform referenceTransform;
    [SerializeField] private int positionSampleCount = 2;
    [SerializeField] private int velocitySampleCount = 2;

    private Queue<Tuple<Vector3, float>> positionQueue = new Queue<Tuple<Vector3, float>>();
    private Tuple<Vector3, float> currentPosition;
    private Tuple<Vector3, float> oldestPosition;

    private Queue<Tuple<float, float>> velocityQueue = new Queue<Tuple<float, float>>();
    private Tuple<float, float> currentVelocity;
    private Tuple<float, float> oldestVelocity;

    private float acceleration;

    public void Update()
    {   
        currentPosition = new Tuple<Vector3, float>
        (
            this.transform.position,
            Time.time
        );
        positionQueue.Enqueue(currentPosition);
        while (positionQueue.Count > positionSampleCount) oldestPosition = positionQueue.Dequeue();

        currentVelocity = new Tuple<float, float>
        (
            Vector3.Dot(currentPosition.Item1 - oldestPosition.Item1, referenceTransform.forward) /
                       (currentPosition.Item2 - oldestPosition.Item2), //deltaTime
            Time.time
        );
        velocityQueue.Enqueue(currentVelocity);
        while (velocityQueue.Count > velocitySampleCount) oldestVelocity = velocityQueue.Dequeue();
        Debug.Log($"currentVelocity    = {Math.Round(currentVelocity.Item1, 2)}");

        acceleration = (currentVelocity.Item1 - oldestVelocity.Item1) /
                       (currentVelocity.Item2 - oldestVelocity.Item2); //deltaTime;
        //Debug.Log($"acceleration    = {Math.Round(acceleration, 2)}");
        FMODAudioManager.SetAcceleration(currentVelocity.Item1);
    }
}