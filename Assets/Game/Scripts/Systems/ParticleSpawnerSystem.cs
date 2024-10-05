

using Game.Scripts.Config;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Game.Scripts.Systems
{
    public class ParticleSpawnerSystem
    {
        private readonly GameConfiguration _gameConfiguration;
        private readonly IObjectResolver _resolver;

        public ParticleSpawnerSystem(GameConfiguration gameConfiguration, IObjectResolver resolver)
        {
            _gameConfiguration = gameConfiguration;
            _resolver = resolver;
        }
        
        public void SpawnWooDestroyedParticle(Vector3 position, int size, Color explosionColor)
        {
            var ps = _resolver.Instantiate(_gameConfiguration.WooDestroyedParticlePrefab, position, Quaternion.identity);
            var main = ps.main;
            main.startColor = explosionColor;
        }
    }
}