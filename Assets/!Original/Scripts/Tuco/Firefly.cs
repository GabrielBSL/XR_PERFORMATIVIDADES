using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireflyRandomWalk : MonoBehaviour
{
    public Transform anchorPoint;  // The point around which the firefly will roam
    public float speed = 2f;       // Movement speed of the firefly
    public float roamRadius = 5f;  // Maximum distance from the anchor point the firefly can roam
    public float noiseScale = 1f;  // Scale of the Perlin noise used to affect the movement
    public float changeDirectionInterval = 2f;  // How often the firefly changes direction
    public float noiseStrength = 0.5f; // Strength of the noise effect on the movement

    private Vector3 randomDirection;
    private float timeSinceDirectionChange = 0f;
    private float noiseOffsetX, noiseOffsetY, noiseOffsetZ;

    void Start()
    {
        GetComponent<Renderer>().material.SetFloat("_blinkOffset", Random.Range(-Mathf.PI, Mathf.PI));

        ChooseNewRandomDirection();
        noiseOffsetX = Random.Range(0f, 100f);
        noiseOffsetY = Random.Range(0f, 100f);
        noiseOffsetZ = Random.Range(0f, 100f);
    }

    void Update()
    {
        // Apply noise to the direction to make the movement more curvy
        float noiseX = Mathf.PerlinNoise(Time.time * noiseScale + noiseOffsetX, 0f) * 2 - 1;
        float noiseY = Mathf.PerlinNoise(Time.time * noiseScale + noiseOffsetY, 0f) * 2 - 1;
        float noiseZ = Mathf.PerlinNoise(Time.time * noiseScale + noiseOffsetZ, 0f) * 2 - 1;
        Vector3 noise = new Vector3(noiseX, noiseY, noiseZ) * noiseStrength;

        // Move the firefly with added noise
        transform.position += (randomDirection + noise) * speed * Time.deltaTime;

        // Check if the firefly has wandered too far from the anchor point
        if (Vector3.Distance(transform.position, anchorPoint.position) > roamRadius)
        {
            // Bring it back within the roam radius by steering towards the anchor point
            Vector3 directionBackToAnchor = (anchorPoint.position - transform.position).normalized;
            randomDirection = directionBackToAnchor;
        }

        // Change direction after a certain interval
        timeSinceDirectionChange += Time.deltaTime;
        if (timeSinceDirectionChange > changeDirectionInterval)
        {
            ChooseNewRandomDirection();
            timeSinceDirectionChange = 0f;
        }
    }

    void ChooseNewRandomDirection()
    {
        // Pick a random direction
        randomDirection = Random.insideUnitSphere.normalized;
        randomDirection.y = Mathf.Clamp(randomDirection.y, -0.1f, 0.1f);  // Optionally limit vertical movement
    }
}
