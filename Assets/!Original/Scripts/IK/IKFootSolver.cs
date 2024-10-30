using UnityEngine;

namespace Main.IK
{
    public class IKFootSolver : MonoBehaviour
    {
        public bool isMovingForward;

        [SerializeField] LayerMask terrainLayer = default;
        [SerializeField] Transform body = default;
        [SerializeField] IKFootSolver otherFoot = default;
        [SerializeField] float speed = 4;
        [SerializeField] float stepDistance = .2f;
        [SerializeField] float stepLength = .2f;
        [SerializeField] float sideStepLength = .1f;

        [SerializeField] float stepHeight = .3f;
        [SerializeField] Vector3 footOffset = default;

        public Vector3 footRotOffset;
        public float footYPosOffset = 0.1f;

        public float rayStartYOffset = 0;
        public float rayLength = 1.5f;

        float footSpacing;
        Vector3 oldPosition, currentPosition, newPosition;
        Vector3 oldNormal, currentNormal, newNormal;
        float lerp;

        private void Start()
        {
            footSpacing = transform.localPosition.x;
            currentPosition = newPosition = oldPosition = transform.position;
            currentNormal = newNormal = oldNormal = transform.up;
            lerp = 1;
        }

        // Update is called once per frame

        void Update()
        {
            transform.position = currentPosition + Vector3.up * footYPosOffset;
            transform.localRotation = Quaternion.Euler(footRotOffset);

            Ray ray = new Ray(body.position + (body.right * footSpacing) + Vector3.up * rayStartYOffset, Vector3.down);

            Debug.DrawRay(body.position + (body.right * footSpacing) + Vector3.up * rayStartYOffset, Vector3.down);
            RaycastHit[] info = Physics.RaycastAll(body.position + (body.right * footSpacing) + Vector3.up * rayStartYOffset, Vector3.down, rayLength, terrainLayer.value);

            if (info.Length > 0)
            {
                if (Vector3.Distance(newPosition, info[0].point) > stepDistance && !otherFoot.IsMoving() && lerp >= 1)
                {
                    lerp = 0;
                    Vector3 direction = Vector3.ProjectOnPlane(info[0].point - currentPosition, Vector3.up).normalized;

                    float angle = Vector3.Angle(body.forward, body.InverseTransformDirection(direction));

                    isMovingForward = angle < 50 || angle > 130;

                    if (isMovingForward)
                    {
                        newPosition = info[0].point + direction * stepLength + footOffset;
                        newNormal = info[0].normal;
                    }
                    else
                    {
                        newPosition = info[0].point + direction * sideStepLength + footOffset;
                        newNormal = info[0].normal;
                    }
                }
            }

            if (lerp < 1)
            {
                Vector3 tempPosition = Vector3.Lerp(oldPosition, newPosition, lerp);
                tempPosition.y += Mathf.Sin(lerp * Mathf.PI) * stepHeight;

                currentPosition = tempPosition;
                currentNormal = Vector3.Lerp(oldNormal, newNormal, lerp);
                lerp += Time.deltaTime * speed;
            }
            else
            {
                oldPosition = newPosition;
                oldNormal = newNormal;
            }
        }

        public bool IsMoving()
        {
            return lerp < 1;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(newPosition, 0.1f);
        }
    }
}
