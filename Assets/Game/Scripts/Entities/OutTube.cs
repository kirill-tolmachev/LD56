using Game.Scripts.Config;
using Game.Scripts.Systems;
using UnityEngine;
using VContainer;

namespace Game.Scripts.Entities
{
    public class OutTube : MonoBehaviour
    {
        public float ShotInterval => 1.333f * _levelState.ConveyorInterval;
        public Transform GunPoint;
        
        private float _lastShotTime;
        private WooLifetimeSystem _wooLifetimeSystem;
        private GameConfiguration _gameConfiguration;
        private Capsule _capsule;
        private LevelState _levelState;
        
        [SerializeField] private WooType _expectedWooType;
        
        [Inject]
        public void Construct(WooLifetimeSystem wooLifetimeSystem, GameConfiguration gameConfiguration, LevelState levelState)
        {
            _wooLifetimeSystem = wooLifetimeSystem;
            _gameConfiguration = gameConfiguration;
            _levelState = levelState;
        }

        private void Awake()
        {
            _capsule = GetComponentInParent<Capsule>();
        }
        
        private void Update()
        {
            if (_levelState.IsPaused)
                return;
            
            if (Time.time - _lastShotTime < ShotInterval)
                return;
            
            _lastShotTime = Time.time;
            
            var position = GunPoint.position;
            var randomType = GetWooType();
            
            _wooLifetimeSystem.Create(randomType, 1, position);
        }

        private WooType GetWooType()
        {
            var v = Random.Range(0f, 1f);
            var invalidNormal = _expectedWooType == WooType.Square ? WooType.Circle : WooType.Square;
            
            if (v < _levelState.InvalidRedChance)
                return WooType.SquareRed;
            
            if (v < _levelState.InvalidNormalChance)
                return invalidNormal;
            
            return _expectedWooType;
        }
    }
}