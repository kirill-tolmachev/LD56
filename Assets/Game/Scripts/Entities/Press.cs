using System.Linq;
using DG.Tweening;
using Game.Scripts.Systems;
using UnityEngine;
using VContainer;

namespace Game.Scripts.Entities
{
    public class Press : MonoBehaviour
    {
        [SerializeField] private BoxCollider2D _boxCollider;

        private WooLifetimeSystem _wooLifetimeSystem;
        private bool _isPressing;
        private Vector3 _initialPosition;
        [SerializeField] private Transform _endPoint;
        
        [Inject]
        public void Construct(WooLifetimeSystem wooLifetimeSystem)
        {
            _wooLifetimeSystem = wooLifetimeSystem;
        }

        private void Awake()
        {
            _initialPosition = transform.position;
        }

        public void DoPress()
        {
            if (_isPressing)
                return;
            
            _isPressing = true;
            var seq = DOTween.Sequence();
            seq.Append(transform.DOMoveY(_endPoint.position.y, 0.2f).SetEase(Ease.InOutSine))
                .AppendInterval(0.5f)
                .Append(transform.DOMoveY(_initialPosition.y, 0.2f).SetEase(Ease.InOutSine))
                .OnComplete(() => _isPressing = false); // Reset pressing state after the sequence finishes
        }
    }
}