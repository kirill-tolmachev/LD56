

using Game.Scripts.Config;
using Game.Scripts.Entities;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Game.Scripts.Systems
{
    public class ParticleSpawnerSystem
    {
        private readonly GameConfiguration _gameConfiguration;
        private readonly IObjectResolver _resolver;
        private readonly PlayerState _playerState;

        public ParticleSpawnerSystem(GameConfiguration gameConfiguration, IObjectResolver resolver, PlayerState playerState)
        {
            _gameConfiguration = gameConfiguration;
            _resolver = resolver;
            _playerState = playerState;
        }
        
        public void SpawnWooDestroyedParticle(Vector3 position, int size, bool fromPressure)
        {
            var explosionColor = fromPressure ? Color.black : Color.red;
            if (fromPressure && _playerState.IsPressDown)
            {
                _resolver.Instantiate(_gameConfiguration.PressSqueezeParticlePrefab, _playerState.PressPosition, Quaternion.identity);
            }
            
            var ps = _resolver.Instantiate(_gameConfiguration.WooDestroyedParticlePrefab, position, Quaternion.identity);
            var main = ps.main;
            main.startColor = explosionColor;
        }
    }
}