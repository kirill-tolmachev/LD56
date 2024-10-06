namespace Game.Scripts.Util
{
    using UnityEngine;

    public class CameraEdgePan : MonoBehaviour
    {
        // Parameters to control the panning behavior
        public float panAmount = 0.5f; // Maximum amount to pan the camera (adjust for subtlety)
        public float panSpeed = 5f; // Speed at which the camera pans
        public float edgeThreshold = 50f; // Distance from the edge to start panning (in pixels)

        private Vector3 originalPosition;

        void Start()
        {
            // Store the camera's initial position
            originalPosition = transform.position;
        }

        void Update()
        {
            // Get the current mouse position in screen coordinates
            Vector3 mousePos = Input.mousePosition;

            // Initialize movement variables
            float moveX = 0f;
            float moveY = 0f;

            // Check if the mouse is near the left or right edge
            if (mousePos.x <= edgeThreshold)
            {
                // Near the left edge
                float t = 1f - (mousePos.x / edgeThreshold); // Normalized value between 0 and 1
                moveX = -t * panAmount;
            }
            else if (mousePos.x >= Screen.width - edgeThreshold)
            {
                // Near the right edge
                float t = (mousePos.x - (Screen.width - edgeThreshold)) / edgeThreshold;
                moveX = t * panAmount;
            }

            // Check if the mouse is near the top or bottom edge
            if (mousePos.y <= edgeThreshold)
            {
                // Near the bottom edge
                float t = 1f - (mousePos.y / edgeThreshold);
                moveY = -t * panAmount;
            }
            else if (mousePos.y >= Screen.height - edgeThreshold)
            {
                // Near the top edge
                float t = (mousePos.y - (Screen.height - edgeThreshold)) / edgeThreshold;
                moveY = t * panAmount;
            }

            // Calculate the target position based on the mouse position
            Vector3 targetPosition = originalPosition + new Vector3(moveX, moveY, 0f);

            // Smoothly move the camera towards the target position
            transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * panSpeed);
        }
    }
}