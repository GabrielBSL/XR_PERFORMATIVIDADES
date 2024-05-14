using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crouch : MonoBehaviour
{
    public float value;
    private Transform headTransform;
    private Transform jangadaTransform;

    [SerializeField] private float userHeight;
    [SerializeField] private float crouchRatio = 0.5f;

    void GetHeadTransform(Transform _headTransform){headTransform = _headTransform;}
    void GetJangadaTransform(Transform _jangadaTransform){jangadaTransform = _jangadaTransform;}
    void GetUserHeight(float _userHeight){userHeight = _userHeight;}

    void OnEnable()
    {
        GestureReferenceEvents.headTransform += GetHeadTransform;
        GestureReferenceEvents.jangadaTransform += GetJangadaTransform;
        GestureReferenceEvents.userHeight += GetUserHeight;
    }
    void OnDisable()
    {
        GestureReferenceEvents.headTransform -= GetHeadTransform;
        GestureReferenceEvents.jangadaTransform -= GetJangadaTransform;
    }

    void Update()
    {
        Debug.Log($"_userHeight = {userHeight}, currentHeight = {headTransform.position.y - jangadaTransform.position.y}");
        value = (headTransform.position.y - jangadaTransform.position.y - (crouchRatio * userHeight))/ ((1 - crouchRatio) * userHeight);
    }
}

