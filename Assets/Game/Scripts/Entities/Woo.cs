using Cysharp.Threading.Tasks;
using DG.Tweening;
using Game.Scripts.Systems;
using UnityEngine;
using UnityEngine.EventSystems;
using VContainer;

namespace Game.Scripts.Entities
{
    public class Woo : MonoBehaviour //, IPointerClickHandler
    {
        [SerializeField] private int _size;
        [SerializeField] private float _pressureThreshold = 10f; // Adjust this value as needed
        [SerializeField] private Transform _center;
        [SerializeField] private float _lifetime = 2f;

        [SerializeField] private SpriteRenderer _spriteRenderer;
        [SerializeField] private Material _normalMaterial;
        [SerializeField] private Material _highlightedMaterial;

        public Conveyor Origin;
        
        public Rigidbody2D CenterRigidbody2D;
        
        public WooType Type;
        
        private bool _canDrag = true;
        private bool _isDragging;
        
        public float Lifetime => _lifetime;
        private Animator _animator;
        public Transform Center => _center;
        
        public float PressureThreshold => _pressureThreshold;
        public int Size => _size;
        public Color Color = UnityEngine.Color.black;

        [SerializeField] private float _explosionK = 1.5f;
        private bool _isExploding;

        public CircleCollider2D[] Colliders;
        private SpringJoint2D[] _joints;
        private Rigidbody2D[] _rigidbodies;

        private void Awake()
        {
            Colliders = GetComponentsInChildren<CircleCollider2D>();
            _joints = GetComponentsInChildren<SpringJoint2D>();
            _rigidbodies = GetComponentsInChildren<Rigidbody2D>();
            _animator = GetComponent<Animator>();
            
            ToggleHighlight(false);
        }

        private void Start()
        {
            // ToggleGravity(false);
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
            _wooLifetimeSystem.Destroy(this, true);

            // if (Capsule != null)
            // {
            //     _scoreSystem.AddScore(Capsule, this);    
            // }
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            _wooLifetimeSystem.Alter(this);
        }

        public void ToggleHighlight(bool isHighlighted)
        {
            _spriteRenderer.material = isHighlighted ? _highlightedMaterial : _normalMaterial;
        }

        public void ToggleGravity(bool isGravity)
        {
            CenterRigidbody2D.constraints = isGravity ? RigidbodyConstraints2D.None : RigidbodyConstraints2D.FreezePosition;
        }
    }
}