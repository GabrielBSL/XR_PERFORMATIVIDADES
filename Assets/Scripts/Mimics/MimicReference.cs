using Main.Events;
using UnityEngine;

namespace Main.Mimics
{
    public class MimicReference : MonoBehaviour
    {
        [SerializeField] private Transform rightArmTarget;
        [SerializeField] private Transform leftArmTarget;
        [SerializeField] private Transform headTarget;

        // Start is called before the first frame update
        void Start()
        {
            MainEventsManager.bodyTransformUpdate?.Invoke(transform);
            MainEventsManager.rightHandTransformUpdate?.Invoke(rightArmTarget);
            MainEventsManager.leftHandTransformUpdate?.Invoke(leftArmTarget);
            MainEventsManager.headTransformUpdate?.Invoke(headTarget);

            // Vetor original
            Vector3 originalVector = new Vector3(.71f, 0f, -.71f);

            // Ângulo de rotação em graus
            float angleDegrees = 135f;

            // Rotaciona o vetor
            Vector3 rotatedVector = RotateVector(originalVector, angleDegrees);
        }

        // Função para rotacionar um vetor em torno do eixo Y por um ângulo especificado em graus
        public Vector3 RotateVector(Vector3 originalVector, float angleDegrees)
        {
            // Converte o ângulo de graus para radianos
            float angleRadians = angleDegrees * Mathf.Deg2Rad;

            // Calcula as coordenadas x e z do vetor rotacionado usando trigonometria
            float rotatedX = originalVector.x * Mathf.Cos(angleRadians) - originalVector.z * Mathf.Sin(angleRadians);
            float rotatedZ = originalVector.x * Mathf.Sin(angleRadians) + originalVector.z * Mathf.Cos(angleRadians);

            // Retorna o vetor rotacionado
            return new Vector3(rotatedX, originalVector.y, rotatedZ);
        }
    }
}
