using UnityEngine;
using UnityEngine.UIElements;

public class Bubble : MonoBehaviour
{
    private static Vector3 globalDrift;

    [SerializeField] private bool isStatic;
    [SerializeField] private float lifetime;

    [Header("Movement")]
    [SerializeField] private Vector3 drift;
    [SerializeField] private float bounce;

    [Header("Scale")]
    [SerializeField] private Vector3 baseScale = Vector3.one;
    [SerializeField] private float minScaleMultiplier;
    [SerializeField] private float maxScaleMultiplier;
    private float spawnTime;
    private float timeOffset;

    private void Start()
    {
        spawnTime = Time.time;
        timeOffset = Random.Range(-Mathf.PI, Mathf.PI);
        timeOffset = Random.Range(-Mathf.PI, Mathf.PI);
        this.transform.localScale = baseScale * Random.Range(minScaleMultiplier, maxScaleMultiplier);
    }
    
    private void Update()
    {
        if(!isStatic)
        {
            if(Time.time - spawnTime >= lifetime) Pop();
            Vector3 bounceVector = new Vector3(0, bounce * Mathf.Sin(Time.time - spawnTime + timeOffset), 0);
            this.transform.position += Time.deltaTime * bounceVector + globalDrift;
            globalDrift = drift;
        }
    }

    public void Pop()
    {
        
    }
}
