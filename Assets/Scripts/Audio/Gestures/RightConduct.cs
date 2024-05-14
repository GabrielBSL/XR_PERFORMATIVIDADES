using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RightConduct : MonoBehaviour
{
    public float value;
    private TransformSampler rightTransformSampler;
    private Transform rightTransform;
    private Transform headTransform;
    public float userWingspan;

    void GetRightTransformSampler(TransformSampler _rightTransformSampler){rightTransformSampler = _rightTransformSampler;}
    void GetRightTransform(Transform _rightTransform){rightTransform = _rightTransform;}
    void GetHeadTransform(Transform _headTransform){headTransform = _headTransform;}
    void GetUserWingspan(float _userWingspan){userWingspan = _userWingspan;}

    void OnEnable()
    {
        GestureReferenceEvents.rightTransformSampler += GetRightTransformSampler;
        GestureReferenceEvents.rightTransform += GetRightTransform;
        GestureReferenceEvents.headTransform += GetHeadTransform;
    }
    void OnDisable()
    {
        GestureReferenceEvents.rightTransformSampler -= GetRightTransformSampler;
        GestureReferenceEvents.rightTransform -= GetRightTransform;
        GestureReferenceEvents.headTransform -= GetHeadTransform;
    }

    void Update()
    {
        value = Mathf.Sign(Vector3.Dot(rightTransform.forward, rightTransformSampler.GetVelocity())) * //0 if palm faces away from movement
                Mathf.Abs(rightTransform.position.y - headTransform.position.y) / (userWingspan / 2);

        Debug.Log($"maestro right = {value}");
    }
}