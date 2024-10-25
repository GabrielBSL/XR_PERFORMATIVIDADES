using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AldeiaFill : MonoBehaviour
{
    [SerializeField] private float fill;
    [SerializeField] private GameObject[] avatars;

    void Update()
    {
        for(int i = 0; i < avatars.Length; i += 1)
        {
            avatars[i].GetComponent<Renderer>().material.SetFloat("_completion", Mathf.Clamp(fill - i, 0, 1));
        }
    }
}