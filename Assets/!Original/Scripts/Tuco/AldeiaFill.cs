using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AldeiaFill : MonoBehaviour
{
    [SerializeField] private float fill;
    [SerializeField] private Renderer[] avatarRenderers;
    private MaterialPropertyBlock propertyBlock;

    // GET PLAYER MOVEMENT
    [SerializeField] private bool debug;
    [SerializeField] private OldTransformSampler leftTransformSampler;
    [SerializeField] private OldTransformSampler rightTransformSampler;
    [SerializeField] private OldTransformSampler headTransformSampler;
    [SerializeField] private float requiredMovement;
    private float totalMovement = 0f;

    void Start()
    {
        propertyBlock = new MaterialPropertyBlock();
    }

    void Update()
    {
        totalMovement += leftTransformSampler.GetDisplacement().magnitude;
        totalMovement += rightTransformSampler.GetDisplacement().magnitude;
        totalMovement += headTransformSampler.GetDisplacement().magnitude;

        if(!debug) fill = totalMovement / requiredMovement;

        for(int i = 0; i < avatarRenderers.Length; i += 1)
        {
            propertyBlock.Clear();
            propertyBlock.SetFloat("_completion", Mathf.Clamp(fill - i, 0, 1));
            avatarRenderers[i].SetPropertyBlock(propertyBlock);
        }
    }
}