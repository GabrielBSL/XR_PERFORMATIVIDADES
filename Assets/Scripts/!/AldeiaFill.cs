using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AldeiaFill : MonoBehaviour
{
    [SerializeField] private float fill;
    [SerializeField] private GameObject[] avatars;

    // GET PLAYER MOVEMENT
    [SerializeField] private OldTransformSampler leftTransformSampler;
    [SerializeField] private OldTransformSampler rightTransformSampler;
    [SerializeField] private OldTransformSampler headTransformSampler;
    [SerializeField] private float requiredMovement;
    private float totalMovement = 0f;

    void Update()
    {
        totalMovement += leftTransformSampler.GetDisplacement().magnitude;
        totalMovement += rightTransformSampler.GetDisplacement().magnitude;
        totalMovement += headTransformSampler.GetDisplacement().magnitude;

        fill = totalMovement / requiredMovement;

        for(int i = 0; i < avatars.Length; i += 1)
        {
            avatars[i].GetComponent<Renderer>().material.SetFloat("_completion", Mathf.Clamp(fill - i, 0, 1));
        }
    }
}
