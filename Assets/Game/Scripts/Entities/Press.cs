using System.Linq;
using DG.Tweening;
using Game.Scripts.Systems;
using Game.Scripts.Util;
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
        private PlayerState _playerState;
        
        private float _lastPressTime;
        private Capsule _capsule;
        private LevelState _levelState;
        private AudioManager _audioManager;
        
        [Inject]
        public void Construct(WooLifetimeSystem wooLifetimeSystem, AudioManager audioManager, PlayerState playerState, LevelState levelState)
        {
            _wooLifetimeSystem = wooLifetimeSystem;
            _playerState = playerState;
            _levelState = levelState;
            _audioManager = audioManager;
        }

        private void Awake()
        {
            _initialPosition = transform.position;
            _capsule = GetComponentInParent<Capsule>();
        }

        private void Update()
        {
            if (_levelState.IsPaused)
                return;
            
            if (Time.time - _lastPressTime < _capsule.PressInterval)
                return;

            _lastPressTime = Time.time;
            DoPress();
        }
        
        private void DoPress()
        {
            if (_isPressing)
                return;
            
            _isPressing = true;
            var seq = DOTween.Sequence();
            seq.Append(transform.DOMoveY(_endPoint.position.y, 0.2f).SetEase(Ease.InOutSine))
                .AppendCallback(() =>
                {
                    _playerState.IsPressDown = true;
                    _audioManager.PlaySqueeze();
                    _playerState.PressPosition = transform.position;
                })
                .AppendInterval(0.5f)
                .AppendCallback(() => _playerState.IsPressDown = false)
                .Append(transform.DOMoveY(_initialPosition.y, 0.2f).SetEase(Ease.InOutSine))
                .OnComplete(() => _isPressing = false);
        }
    }
}