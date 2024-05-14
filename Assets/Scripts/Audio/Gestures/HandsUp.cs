using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandsUp : MonoBehaviour
{
    public float value;
    private Transform leftTransform;
    private Transform rightTransform;
    private Transform headTransform;
    
    [SerializeField] private float scale = 1f;
    [SerializeField] private float threshold = 0f;

    void GetLeftTransform(Transform _leftTransform){leftTransform = _leftTransform;}
    void GetRightTransform(Transform _rightTransform){rightTransform = _rightTransform;}
    void GetHeadTransform(Transform _headTransform){headTransform = _headTransform;}

    void OnEnable()
    {
        GestureReferenceEvents.leftTransform += GetLeftTransform;
        GestureReferenceEvents.rightTransform += GetRightTransform;
        GestureReferenceEvents.headTransform += GetHeadTransform;
    }
    void OnDisable()
    {
        GestureReferenceEvents.leftTransform -= GetLeftTransform;
        GestureReferenceEvents.rightTransform -= GetRightTransform;
        GestureReferenceEvents.headTransform -= GetHeadTransform;
    }

    void Update()
    {
        value = ((leftTransform.position.y + rightTransform.position.y) / 2 - headTransform.position.y) * scale - threshold;
    }
}