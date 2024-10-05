using Game.Scripts.Systems;
using UnityEngine;
using VContainer;

namespace Game.Scripts.Entities
{
    public class Woo : MonoBehaviour
    {
        [SerializeField] private int _size;
        [SerializeField] private float _pressureThreshold = 10f; // Adjust this value as needed
        [SerializeField] private Transform _center;
        [SerializeField] private float _lifetime = 2f;
        
        public float Lifetime => _lifetime;
        
        public Transform Center => _center;
        
        public float PressureThreshold => _pressureThreshold;
        public int Size => _size;
        
        [SerializeField] private float _explosionK = 1.5f;
        private bool _isExploding;

        public CircleCollider2D[] Colliders;
        private SpringJoint2D[] _joints;

        private void Awake()
        {
            Colliders = GetComponentsInChildren<CircleCollider2D>();
            _joints = GetComponentsInChildren<SpringJoint2D>();
        }
        
        [Inject] private WooLifetimeSystem _wooLifetimeSystem;
        [Inject] private ScoreSystem _scoreSystem;

        private void Update()
        {
            foreach (var joint in _joints)
            {
                var distance = Vector2.Distance(joint.transform.position, joint.connectedBody.transform.position);
                if (distance < joint.distance * _explosionK)
                {
                    Debug.Log(distance + " < " + joint.distance * _explosionK);
                    Explode();
                    break;
                }
            }
        }
        
        public void Explode()
        {
            if (_isExploding)
                return;
            
            _isExploding = true;
            _wooLifetimeSystem.Destroy(this, Color.black);
            _scoreSystem.AddScore(this);
        }
    }
}