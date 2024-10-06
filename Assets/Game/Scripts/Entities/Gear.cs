using UnityEngine;

namespace Game.Scripts.Entities
{
    public class Gear : MonoBehaviour
    {
        public bool IsRotating;
        public float RotationSpeed = 100f;

        private void Update()
        {
            if (!IsRotating)
                return;
            
            transform.Rotate(Vector3.forward * RotationSpeed * Time.deltaTime);
        }
    }
}