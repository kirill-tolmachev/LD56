using Game.Scripts.Config;
using Game.Scripts.Systems;
using UnityEngine;
using VContainer;

namespace Game.Scripts.Entities
{
    public class OutTube : MonoBehaviour
    {
        public Transform GunPoint;
        
        private float _lastShotTime;
        private WooLifetimeSystem _wooLifetimeSystem;
        private GameConfiguration _gameConfiguration;
        private Capsule _capsule;
        
        [Inject]
        public void Construct(WooLifetimeSystem wooLifetimeSystem, GameConfiguration gameConfiguration)
        {
            _wooLifetimeSystem = wooLifetimeSystem;
            _gameConfiguration = gameConfiguration;
        }

        private void Awake()
        {
            _capsule = GetComponentInParent<Capsule>();
        }
        
        private void Update()
        {
            if (Time.time - _lastShotTime < _gameConfiguration.OutTubeShotInterval)
                return;
            
            _lastShotTime = Time.time;
            
            var position = GunPoint.position;
            var randomType = GetRandomWooType();
            _wooLifetimeSystem.Create(randomType, 1, position, _capsule);
        }

        private WooType GetRandomWooType()
        {
            var v = Random.Range(0f, 3f);
            if (v < 1)
                return WooType.Circle;
            
            if (v < 2)
                return WooType.Square;
            
            return WooType.SquareRed;
        }
    }
}