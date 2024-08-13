using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(Collider))]
public class BubbleEmitter : MonoBehaviour
{
    [SerializeField] private GameObject bubblePrefab;
    [SerializeField] private float cooldown;
    [SerializeField] private float spawnRadius;

    private bool emitting = true;

    private void OnTriggerEnter()
    {
        emitting = true;
        StartCoroutine(EmitBubbles());
    }

    private void OnTriggerExit()
    {
        emitting = false;
    }

    private IEnumerator EmitBubbles()
    {   
        while(emitting)
        {
            GameObject bubble = Instantiate(bubblePrefab, this.transform.position + Random.insideUnitSphere * spawnRadius, Quaternion.identity);
            yield return new WaitForSeconds(cooldown);
        }
    }
}
