using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crouch : MonoBehaviour
{
    public float value;
    private Transform headTransform;
    private Transform jangadaTransform;

    [SerializeField] private float crouchRatio = 0.5f;
    private float userHeight;    
    private float crouchHeight;

    void GetHeadTransform(Transform _headTransform){headTransform = _headTransform;}
    void GetJangadaTransform(Transform _jangadaTransform){jangadaTransform = _jangadaTransform;}
    void GetUserHeight(float _userHeight)
    {
        userHeight = _userHeight;
        crouchHeight = crouchRatio * userHeight; 
    }

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
        value = (headTransform.position.y - jangadaTransform.position.y - crouchHeight) / (userHeight - crouchHeight);
    }
}