using UnityEngine;

public class Bubble : MonoBehaviour
{
    [SerializeField] private bool isTutorial;
    [SerializeField] private float lifetime;

    [Header("Movement")]
    public Vector3 drift;
    [SerializeField] private float bounce;

    [Header("Scale")]
    [SerializeField] private Vector3 baseScale = Vector3.one;
    [SerializeField] private float minScaleMultiplier;
    [SerializeField] private float maxScaleMultiplier;
    private float spawnTime;
    private float timeOffset;

    public Bubble(Vector3 drift)
    {
        this.drift = drift;
    }

    private void Start()
    {
        spawnTime = Time.time;
        timeOffset = Random.Range(-Mathf.PI, Mathf.PI);
        timeOffset = Random.Range(-Mathf.PI, Mathf.PI);
        this.transform.localScale = baseScale * Random.Range(minScaleMultiplier, maxScaleMultiplier);
    }
    
    private void Update()
    {
        if(!isTutorial)
        {
            if(Time.time - spawnTime >= lifetime) Destroy(this.gameObject);
            Vector3 bounceVector = new Vector3(0, bounce * Mathf.Sin(Time.time - spawnTime + timeOffset), 0);
            this.transform.position += Time.deltaTime * (bounceVector + drift);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("BubblePopper"))
        {            
            other.gameObject.GetComponent<BubblePopper>().Pop(isTutorial);
            FMODAudioManager.instance.BubblePop();
            Destroy(this.gameObject);
        }
    }
}