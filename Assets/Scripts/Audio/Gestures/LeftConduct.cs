using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeftConduct : MonoBehaviour
{
    public float value;
    private Transform leftTransform;
    private Transform headTransform;
    public float userWingspan;

    void GetLeftTransform(Transform _leftTransform){leftTransform = _leftTransform;}
    void GetHeadTransform(Transform _headTransform){headTransform = _headTransform;}
    void GetUserWingspan(float _userWingspan){userWingspan = _userWingspan;}

    void OnEnable()
    {
        GestureReferenceEvents.leftTransform += GetLeftTransform;
        GestureReferenceEvents.headTransform += GetHeadTransform;
    }
    void OnDisable()
    {
        GestureReferenceEvents.leftTransform -= GetLeftTransform;
        GestureReferenceEvents.headTransform -= GetHeadTransform;
    }

    void Update()
    {
        value = Vector3.Dot(leftTransform.forward, Vector3.up) * 
                Mathf.Abs(leftTransform.position.y - headTransform.position.y) / (userWingspan / 2);

        Debug.Log($"maestro left = {value}");

    }
}