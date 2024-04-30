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
    private bool onCooldown = false;

    [SerializeField] private UnityEvent oneshot;

    void Update()
    {
        reference = (referenceMode == Reference.Vector3)? referenceVector3 : referenceTransform.rotation * referenceVector3;
        
        if (Vector3.Dot(hand.GetVelocity(), reference) > 0) // Está se movendo na direção?
        {
            if
            (
                Vector3.Dot(hand.GetVelocity(), reference) > threshold / Time.deltaTime &&
                Vector3.Dot(hand.GetAcceleration(), reference) <= 0
                && !onCooldown
            )
            {
                oneshot?.Invoke();
                onCooldown = true;
            }
        }
        else onCooldown = false; // Se estiver na direção oposta, refresh cooldown

        Debug.Log($"velocity = {Vector3.Dot(hand.GetVelocity(), reference)}");
    }
}