using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformSampler : MonoBehaviour
{
    //================ VARIABLE DECLARATIONS ================

    [SerializeField] private Transform referenceTransform;
    [SerializeField] [Range(2, 16)] private int sampleCount = 16;

    public Queue<Tuple<Vector3, Quaternion, float>> sampleQueue = new Queue<Tuple<Vector3, Quaternion, float>>();
    public Tuple<Vector3, Quaternion, float> currentSample;
    public Tuple<Vector3, Quaternion, float> oldestSample;
    
    public Queue<Tuple<Vector3, float>> derivativeSampleQueue = new Queue<Tuple<Vector3, float>>();
    public Tuple<Vector3, float> derivativeCurrentSample;
    public Tuple<Vector3, float> derivativeOldestSample;

    /*
    enum AccelerationAlgorithm {TripleSample, Estimator};
    [SerializeField] AccelerationAlgorithm accelerationAlgorithm;
    */

    //================ MONOBEHAVIOUR FUNCTIONS ================

    private void Awake()
    {
        currentSample = new Tuple<Vector3, Quaternion, float>
        (
            this.transform.position - referenceTransform.position,
            this.transform.rotation,
            Time.time
        );

        oldestSample = currentSample;
    }

    void Update()
    {
        currentSample = new Tuple<Vector3, Quaternion, float>
        (
            this.transform.position - referenceTransform.position,
            this.transform.rotation,
            Time.time
        );
        sampleQueue.Enqueue(currentSample);
        while (sampleQueue.Count > sampleCount) oldestSample= sampleQueue.Dequeue();

        //================ ACCELERATION ================
        derivativeCurrentSample = new Tuple<Vector3, float>
        (
            GetVelocity(),
            Time.time
        );
        derivativeSampleQueue.Enqueue(derivativeCurrentSample);
        while (derivativeSampleQueue.Count > sampleCount) derivativeOldestSample = derivativeSampleQueue.Dequeue();

    }

    //================ 1 PREVIOUS FRAME COMPLEXITY ================

    public Tuple<Vector3, Quaternion, float> GetCurrentSample() {return currentSample;}

    public float GetDeltaTime() {return currentSample.Item3 - oldestSample.Item3;}

    private Vector3 GetDisplacement() {return currentSample.Item1 - oldestSample.Item1;}
    public Vector3 GetVelocity() {return GetDisplacement() / GetDeltaTime();}
    public float GetSpeed() {return GetDisplacement().magnitude / GetDeltaTime();}
    
    public Quaternion GetRotation() {return currentSample.Item2 * Quaternion.Inverse(oldestSample.Item2);}
    public Quaternion GetAngularVelocity() {return GetRotation();}// * GetDeltaTime();}
    public float GetAngularSpeed() {return (Quaternion.Angle(currentSample.Item2, oldestSample.Item2) / 180) / GetDeltaTime();}

    public Vector3 GetDeltaVelocity() {return (derivativeCurrentSample.Item1 - derivativeOldestSample.Item1);}
    
    public Vector3 GetAcceleration() {return (derivativeCurrentSample.Item1 - derivativeOldestSample.Item1) /
                                             (derivativeCurrentSample.Item2 - derivativeOldestSample.Item2);}
}