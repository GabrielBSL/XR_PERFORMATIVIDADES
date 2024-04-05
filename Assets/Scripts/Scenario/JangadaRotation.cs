using UnityEngine;

namespace Main.Scenario
{
    public class JangadaRotation : MonoBehaviour
    {
        [SerializeField] private float xRotation = 1;
        [SerializeField] private float timeToSwift = 2f;
        [SerializeField] private float lerpIntensity = .025f;

        private float initialRotation;
        private float rotationSign = 1;
        private float rotationAngle = 0;

        // Start is called before the first frame update
        void Start()
        {
            initialRotation = transform.eulerAngles.x;
            Debug.Log(rotationAngle);
        }

        // Update is called once per frame
        void Update()
        {
            rotationAngle += (xRotation * 2 * rotationSign / timeToSwift) * Time.deltaTime;

            if((rotationAngle > xRotation && rotationSign > 0) || (rotationAngle < xRotation * -1 && rotationSign < 0))
            {
                rotationSign *= -1;
            }

            transform.eulerAngles = new Vector3(Mathf.Lerp(transform.eulerAngles.x, initialRotation + rotationAngle, lerpIntensity), 
                                                transform.eulerAngles.y, transform.eulerAngles.z);
        }
    }
}
