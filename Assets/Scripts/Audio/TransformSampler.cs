using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformSampler : MonoBehaviour
{   
    //================ VARIABLE DECLARATIONS ================

    [SerializeField] [Range(2, 16)] private int sampleCount = 16;
    public Queue<Tuple<Vector3, Quaternion, float>> sampleQueue = new Queue<Tuple<Vector3, Quaternion, float>>();
    public Tuple<Vector3, Quaternion, float> currentSample;
    public Tuple<Vector3, Quaternion, float> oldestSample;

    /*
    enum AccelerationAlgorithm {TripleSample, Estimator};
    [SerializeField] AccelerationAlgorithm accelerationAlgorithm;
    */

    //================ MONOBEHAVIOUR FUNCTIONS ================

    void Update()
    {
        currentSample = new Tuple<Vector3, Quaternion, float>
        (
            this.transform.position,
            this.transform.rotation,
            Time.time
        );

        // Update Queue
        sampleQueue.Enqueue(currentSample);
        while (sampleQueue.Count > sampleCount) oldestSample= sampleQueue.Dequeue();

    }

    //================ 1 PREVIOUS FRAME COMPLEXITY ================

    public float GetDeltaTime() {return currentSample.Item3 - oldestSample.Item3;}

    public Vector3 GetDisplacement() {return currentSample.Item1 - oldestSample.Item1;} //currentPosition - oldestPosition
    public Vector3 GetVelocity() {return GetDisplacement() / GetDeltaTime();} //displacement / deltaTime
    public float GetSpeed() {return GetDisplacement().magnitude / GetDeltaTime();} //displacement magnitude / deltaTime
    
    public float GetRotation() {return Quaternion.Angle(currentSample.Item2, oldestSample.Item2);} //currentPosition - oldestPosition    
    public float GetAngularSpeed() {return GetRotation() / GetDeltaTime();} //rotation / deltaTime
}