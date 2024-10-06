using Cysharp.Threading.Tasks;
using DG.Tweening;
using Game.Scripts.Systems;
using UnityEngine;
using UnityEngine.EventSystems;
using VContainer;

namespace Game.Scripts.Entities
{
    public class Woo : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
    {
        [SerializeField] private int _size;
        [SerializeField] private float _pressureThreshold = 10f; // Adjust this value as needed
        [SerializeField] private Transform _center;
        [SerializeField] private float _lifetime = 2f;

        [SerializeField] private SpriteRenderer _spriteRenderer;
        [SerializeField] private Material _normalMaterial;
        [SerializeField] private Material _highlightedMaterial;

        public Rigidbody2D CenterRigidbody2D;
        
        public WooType Type;
        
        public Slot Slot;
        
        private bool _canDrag = true;
        private bool _isDragging;
        
        public float Lifetime => _lifetime;
        private Animator _animator;
        public Transform Center => _center;
        
        public float PressureThreshold => _pressureThreshold;
        public int Size => _size;
        public Capsule Capsule;

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
        [Inject] private MergeSystem _mergeSystem;
        [Inject] private DragDirectionArrow _dragDirectionArrow;
        
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

            if (Capsule != null)
            {
                _scoreSystem.AddScore(Capsule, this);    
            }
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            Debug.Log("Woo clicked");
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (!_isDragging)
                return;
        }

        public void ToggleHighlight(bool isHighlighted)
        {
            _spriteRenderer.material = isHighlighted ? _highlightedMaterial : _normalMaterial;
        }

        public void PlayJump(bool gravity = false)
        {
            _animator.SetTrigger("Jump");
            if (gravity)
                ToggleGravity(true);
        }
        
        public UniTask MoveToSlot(Slot other)
        {
            Slot.Woo = null;
            Slot = other;
            other.Woo = this;
            
            transform.DOKill();
            return transform.DOMoveX(other.transform.position.x, 0.2f).SetEase(Ease.InOutBack).SetId(transform).ToUniTask();
        }

        public void ToggleGravity(bool isGravity)
        {
            CenterRigidbody2D.constraints = isGravity ? RigidbodyConstraints2D.None : RigidbodyConstraints2D.FreezePosition;
        }
        
        // private void MakeKinematic()
        // {
        //     foreach (var rb in _rigidbodies)
        //     {
        //         rb.isKinematic = true;
        //     }
        //
        //     foreach (var cld in Colliders)
        //     {
        //        cld.isTrigger = true;
        //     }
        // }
        //
        // private void MakeDynamic()
        // {
        //     foreach (var rb in _rigidbodies)
        //     {
        //         rb.isKinematic = false;
        //     }
        //     
        //     foreach (var cld in Colliders)
        //     {
        //         cld.isTrigger = false;
        //     }
        // }

        public void OnBeginDrag(PointerEventData eventData)
        {
            _isDragging = true;
            _dragDirectionArrow.Show(Slot);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            _isDragging = false;
            if (_dragDirectionArrow.SelectedSlot != null && _dragDirectionArrow.SelectedSlot.Woo != null)
            {
                _mergeSystem.Merge(this.Slot, _dragDirectionArrow.SelectedSlot);
            }
            
            _dragDirectionArrow.Hide();
        }
    }
}