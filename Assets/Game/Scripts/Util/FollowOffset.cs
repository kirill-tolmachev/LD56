using UnityEngine;

namespace Game.Scripts.Util
{
    public class FollowOffset : MonoBehaviour
    {
        [SerializeField] private BoxCollider2D _targetCollider;

        private float _yOffset;
        
        private void Start()
        {
            // Calculate the initial offset between this object and the top of the target collider
            _yOffset = transform.position.y - _targetCollider.bounds.max.y;
        }

        private void LateUpdate()
        {
            // Update position to maintain the same offset relative to the top of the target collider
            float targetTopY = _targetCollider.bounds.max.y;
            transform.position = new Vector3(transform.position.x, targetTopY + _yOffset, transform.position.z);
        }
    }
}