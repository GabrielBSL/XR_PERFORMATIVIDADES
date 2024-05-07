using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VelocityTracker : MonoBehaviour
{
    /*
    [SerializeField] private Transform referenceTransform;
    [SerializeField] private int positionSampleCount = 2;
    //[SerializeField] private int velocitySampleCount = 2;
    [SerializeField] private float velocityScale = 1.0f;
    [SerializeField] private float velocityThreshold = 1.0f;

    private Queue<Tuple<Vector3, float>> positionQueue = new Queue<Tuple<Vector3, float>>();
    private Tuple<Vector3, float> currentPosition;
    private Tuple<Vector3, float> oldestPosition;

    private float[] velocityQueue = new float[3];
    private Tuple<float, float> currentVelocity;

    /*
    private Queue<Tuple<float, float>> velocityQueue = new Queue<Tuple<float, float>>();
    private Tuple<float, float> currentVelocity;
    private Tuple<float, float> oldestVelocity;
    //private float acceleration;

    public void Update()
    {   
        currentPosition = new Tuple<Vector3, float>
        (
            this.transform.position,
            Time.time
        );
        positionQueue.Enqueue(currentPosition);
        while (positionQueue.Count > positionSampleCount) oldestPosition = positionQueue.Dequeue();

        velocityQueue[2] = velocityQueue[1];
        velocityQueue[1] = velocityQueue[0];
        currentVelocity = new Tuple<float, float>
        (
            velocityScale *
            Vector3.Dot(currentPosition.Item1 - oldestPosition.Item1, referenceTransform.forward) /
                       (currentPosition.Item2 - oldestPosition.Item2), //deltaTime
            Time.time
        );
        velocityQueue[0] = Math.Abs(currentVelocity.Item1);
        /*
        velocityQueue.Enqueue(currentVelocity);
        while (velocityQueue.Count > velocitySampleCount) oldestVelocity = velocityQueue.Dequeue();
        Debug.Log($"currentVelocity    = {Math.Round(currentVelocity.Item1, 2)}");

        /*
        acceleration = (currentVelocity.Item1 - oldestVelocity.Item1) /
                       (currentVelocity.Item2 - oldestVelocity.Item2); //deltaTime;
        //Debug.Log($"acceleration    = {Math.Round(acceleration, 2)}");

        if (velocityQueue[1] > velocityThreshold &&
            velocityQueue[0] < velocityQueue[1] &&
            velocityQueue[1] > velocityQueue[2]) FMODAudioManager.Impulse();

        FMODAudioManager.SetAcceleration(currentVelocity.Item1);
    }
    */
}