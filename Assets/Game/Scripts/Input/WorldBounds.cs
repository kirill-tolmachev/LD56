using UnityEngine;

namespace Game.Scripts
{
    public class WorldBounds : MonoBehaviour
    {
        [SerializeField] private BoxCollider2D _boxCollider;
        
        public float Right => _boxCollider.transform.TransformPoint(_boxCollider.offset + new Vector2(_boxCollider.size.x * 0.5f, 0)).x;
        public float Left => _boxCollider.transform.TransformPoint(_boxCollider.offset - new Vector2(_boxCollider.size.x * 0.5f, 0)).x;

        public float Top => _boxCollider.transform.TransformPoint(_boxCollider.offset + new Vector2(0, _boxCollider.size.y * 0.5f)).y;
        
        public float Bottom => _boxCollider.transform.TransformPoint(_boxCollider.offset - new Vector2(0, _boxCollider.size.y * 0.5f)).y;
    }
}