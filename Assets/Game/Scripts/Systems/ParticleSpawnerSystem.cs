

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

        public void SpawnWooCreatedParticle(Vector3 position, Color color)
        {
            var particle = _resolver.Instantiate(_gameConfiguration.WooCreatedParticlePrefab, position, Quaternion.identity);
            var main = particle.main;
            main.startColor = color;
        }
        
        public void SpawnWooDestroyedParticle(Woo woo, Vector3 position, int size, bool fromPressure)
        {
            if (!fromPressure)
            {
                SpawnWooCreatedParticle(position, woo.Color);
                return;
            }
            
            var explosionColor = fromPressure ? Color.black : Color.red;
            if (fromPressure && _playerState.IsPressDown)
            {
                _resolver.Instantiate(_gameConfiguration.PressSqueezeParticlePrefab, _playerState.PressPosition, Quaternion.identity);
            }
            
            var ps = _resolver.Instantiate(_gameConfiguration.WooDestroyedParticlePrefab, position, Quaternion.identity);
            var main = ps.main;
            main.startColor = explosionColor;
        }
        
        public void SpawnBigExplosion(Vector3 position, Quaternion rotation)
        {
            var ps = _resolver.Instantiate(_gameConfiguration.BigExplosionParticlePrefab, position, rotation);
        }
    }
}