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
            _wooLifetimeSystem.Create(1, position, _capsule);
        }
    }
}