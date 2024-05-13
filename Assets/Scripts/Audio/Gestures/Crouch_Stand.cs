using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crouch_Stand : MonoBehaviour
{
    public float value;
    private Transform headTransform;
    private Transform jangadaTransform;

    [SerializeField] private float userHeight;
    [SerializeField] private float crouchHeight = 1f;

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
        value = (headTransform.position.y - jangadaTransform.position.y - crouchHeight)/
                (userHeight - crouchHeight);
    }
}

