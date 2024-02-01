using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmsAutoScaler : MonoBehaviour
{
    [Header("Shouders")]
    [SerializeField] private Transform leftShouderTransform;
    [SerializeField] private Transform rightShouderTransform;

    [Header("Hand Transforms")]
    [SerializeField] private Transform leftHandTransform;
    [SerializeField] private Transform rightHandTransform;

    [Header("Hand Targets")]
    [SerializeField] private Transform leftHandTarget;
    [SerializeField] private Transform rightHandTarget;

    void Update()
    {
        
    }
}
