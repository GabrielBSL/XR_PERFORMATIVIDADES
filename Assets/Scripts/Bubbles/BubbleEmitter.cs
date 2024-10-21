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
    [SerializeField] private Transform handle;
    [SerializeField] private Vector3 drift { get { return handle.position - transform.position;}}
    [SerializeField] private Vector3 spawnOffset;

    private bool emitting = true;

    private void OnTriggerEnter(Collider other)
    {
        emitting = true;
        StartCoroutine(EmitBubbles(other.gameObject.transform));
    }

    private void OnTriggerExit()
    {
        emitting = false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawLine(transform.position, handle.position);
    }

    private IEnumerator EmitBubbles(Transform playerTranform)
    {   
        while(emitting)
        {
            GameObject bubble = Instantiate(bubblePrefab, playerTranform.position + spawnOffset + Random.insideUnitSphere * spawnRadius, Quaternion.identity);
            yield return new WaitForSeconds(cooldown);
        }
    }
}
