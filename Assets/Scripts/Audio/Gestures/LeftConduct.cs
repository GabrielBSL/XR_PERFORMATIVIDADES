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
    //void GetUserWingspan(float _userWingspan){userWingspan = _userWingspan;}

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
        value = Mathf.Clamp01(Vector3.Dot(leftTransform.forward, Vector3.up)) * 
                Mathf.Max(leftTransform.position.y - headTransform.position.y, 0)
                +
                Mathf.Clamp01(Vector3.Dot(leftTransform.forward, Vector3.down)) * 
                Mathf.Min(leftTransform.position.y - headTransform.position.y, 0);

        //Debug.Log($"maestro left = {value}");
    }
    void OnDrawGizmos()
    {
        // Draw a yellow sphere at the transform's position
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(headTransform.position, 0.05f);
    }
}