using UnityEngine;

public class Bubble : MonoBehaviour
{
    private static Vector3 globalDrift;

    [SerializeField] private bool isStatic;
    [SerializeField] private Vector3 drift;
    [SerializeField] private float bounce;
    private float _spawnTime;

    void Start()
    {
        _spawnTime = Time.time;
    }
    
    void Update()
    {
        if(isStatic) return;
        this.transform.position += Time.deltaTime * (new Vector3(0, bounce * Mathf.Sin(Time.time - _spawnTime), 0) + globalDrift);
        globalDrift = drift;
    }
}
