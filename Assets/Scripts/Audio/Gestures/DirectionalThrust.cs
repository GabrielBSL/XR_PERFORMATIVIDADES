using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class DirectionalThrust : MonoBehaviour
{    
    [SerializeField] private TransformSampler hand;

    enum Reference {Vector3, Transform};
    [SerializeField] Reference referenceMode;
    [SerializeField] private Transform referenceTransform;
    [SerializeField] private Vector3 referenceVector3 = Vector3.down;
    private Vector3 reference;

    [SerializeField] private float threshold = 1f;
    [SerializeField] private float cooldown = 1f;
    private float lastActive;

    [SerializeField] private UnityEvent oneshot;

    void Update()
    {
        reference = (referenceMode == Reference.Vector3)? referenceVector3 : referenceTransform.forward;

        if
        (
            Vector3.Dot(hand.GetVelocity(), reference) > threshold &&
            hand.GetCurrentSample().Item3 - lastActive > cooldown
        )
        {
            oneshot?.Invoke();
            lastActive = hand.GetCurrentSample().Item3;
        }
    }
}