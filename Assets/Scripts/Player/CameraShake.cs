namespace Player
{
    using UnityEngine;
    using System.Collections;

    public class CameraShake : MonoBehaviour
    {
        [SerializeField] private Transform target = null;
        
        [SerializeField] private float decreaseFactor = 1.0f;
        [SerializeField] private Vector3 offset = Vector3.zero;

        private float shakeDuration = 0f;
        private float shakeAmount = 0.7f;
        
        private Vector3 originalPos;

        void Start()
        {
            originalPos = target.transform.localPosition;
        }

        void Update()
        {
            if (shakeDuration > 0)
            {
                target.transform.localPosition = originalPos + Random.insideUnitSphere * shakeAmount + offset;
                shakeDuration -= Time.deltaTime * decreaseFactor;
            }
            else
            {
                shakeDuration = 0f;
                target.transform.localPosition = originalPos;
            }
        }

        public void ShakeCamera(float duration, float amount)
        {
            shakeDuration = duration;
            shakeAmount = amount;
            originalPos = target.transform.localPosition;
        }
    }
}