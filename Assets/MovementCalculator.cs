using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementCalculator : MonoBehaviour
{
    [SerializeField] private TransformSampler leftTransformSampler;
    [SerializeField] private TransformSampler rightTransformSampler;
    [SerializeField] private TransformSampler headTransformSampler;
    [SerializeField] private CenteredProgressBar progressBar;
    [SerializeField] private float requiredMovement = 20000f;
    private float totalMovement = 0f;
    void Update()
    {
        totalMovement += leftTransformSampler.GetSpeed();
        totalMovement += rightTransformSampler.GetSpeed();
        totalMovement += headTransformSampler.GetSpeed();
        progressBar.value = totalMovement / requiredMovement;
        if(totalMovement >= requiredMovement)
        {
            progressBar.gameObject.SetActive(false);
        }
    }
}
