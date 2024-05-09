using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crouch_Stand : MonoBehaviour
{
    public float value;
    private Transform headTransform;
    private Transform jangadaTransform;

    [SerializeField] private float threshold = -1;

    void GetHeadTransform(Transform _headTransform){headTransform = _headTransform;}
    void GetJangadaTransform(Transform _jangadaTransform){jangadaTransform = _jangadaTransform;}

    void OnEnable()
    {
        GestureReferenceEvents.headTransform += GetHeadTransform;
        GestureReferenceEvents.jangadaTransform += GetJangadaTransform;
    }
    void OnDisable()
    {
        GestureReferenceEvents.headTransform -= GetHeadTransform;
        GestureReferenceEvents.jangadaTransform -= GetJangadaTransform;
    }

    void Update()
    {
        value = (headTransform.position.y - jangadaTransform.position.y) - threshold;
    }
}

