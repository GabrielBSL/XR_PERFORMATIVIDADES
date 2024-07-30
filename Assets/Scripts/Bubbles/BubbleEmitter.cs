using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class BubbleEmitter : MonoBehaviour
{
    [SerializeField] private GameObject bubblePrefab;
    [SerializeField] private float cooldown;
    [SerializeField] private float spawnRadius;
    private float lastTime;
    private float currentTime;

    void Start()
    {
        lastTime = Time.time;
    }

    void Update()
    {
        currentTime = Time.time;
        if(currentTime - lastTime >= cooldown)
        {
            GameObject bubble = Instantiate(bubblePrefab, this.transform.position + Random.insideUnitSphere * spawnRadius, Quaternion.identity);
            lastTime = currentTime;
        }
    }
}
