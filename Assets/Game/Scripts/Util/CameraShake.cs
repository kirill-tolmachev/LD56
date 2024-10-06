using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Game.Scripts.Util
{
    public class CameraShake : MonoBehaviour
    {
        // Intensity of the shake effect
        public float shakeAmount = 0.7f;

        // Speed at which the shake effect diminishes
        public float decreaseFactor = 1.0f;

        // Duration for blending back to the original position
        public float blendDuration = 0.2f;

        private Vector3 originalPos;

        void OnEnable()
        {
            // Store the original position of the camera
            originalPos = transform.position;
        }

        // Call this method to start the shake effect
        public async UniTask TriggerShakeAsync(float duration)
        {
            float shakeDuration = duration;
            Vector3 currentPos = originalPos;

            while (shakeDuration > 0)
            {
                // Generate a random offset within a circle
                Vector2 shakePos = Random.insideUnitCircle * shakeAmount * (shakeDuration / duration);

                // Apply the offset to the camera's position
                transform.position = new Vector3(
                    originalPos.x + shakePos.x,
                    originalPos.y + shakePos.y,
                    originalPos.z
                );

                // Decrease the shake duration over time
                shakeDuration -= Time.deltaTime * decreaseFactor;

                // Wait until the next frame
                await UniTask.Yield();
            }

            // Smoothly blend back to the original position
            float elapsed = 0f;
            currentPos = transform.position;

            while (elapsed < blendDuration)
            {
                transform.position = Vector3.Lerp(currentPos, originalPos, elapsed / blendDuration);
                elapsed += Time.deltaTime;

                // Wait until the next frame
                await UniTask.Yield();
            }

            // Ensure the camera is exactly at the original position
            transform.position = originalPos;
        }
    }
}
