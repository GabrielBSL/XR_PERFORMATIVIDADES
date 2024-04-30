using UnityEngine;

namespace Main.Scenario
{
    public class JangadaRotation : MonoBehaviour
    {
        [SerializeField] private float zRotation = 1;
        [SerializeField] private float timeToSwift = 2f;
        [SerializeField] private float lerpIntensity = .025f;

        private float initialRotation;
        private float rotationSign = 1;
        private float rotationAngle = 0;

        // Start is called before the first frame update
        void Start()
        {
            initialRotation = transform.eulerAngles.z;
            Debug.Log(rotationAngle);
        }

        // Update is called once per frame
        void Update()
        {
            rotationAngle += (zRotation * 2 * rotationSign / timeToSwift) * Time.deltaTime;

            if((rotationAngle > zRotation && rotationSign > 0) || (rotationAngle < zRotation * -1 && rotationSign < 0))
            {
                rotationSign *= -1;
            }

            transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, Mathf.Lerp(transform.eulerAngles.z, initialRotation + rotationAngle, lerpIntensity));
        }
    }
}
