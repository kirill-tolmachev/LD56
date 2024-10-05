using System;
using System.Collections.Generic;
using Game.Scripts.Config;
using Game.Scripts.Entities;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Game.Scripts.Systems
{
    public class WooLifetimeSystem : IInitializable, IDisposable, ITickable
    {
        private Dictionary<Woo, float> _woos = new Dictionary<Woo, float>();
        private readonly GameConfiguration _gameConfiguration;
        private readonly ParticleSpawnerSystem _particleSpawnerSystem;
        private readonly IObjectResolver _objectResolver;

        public WooLifetimeSystem(GameConfiguration gameConfiguration, ParticleSpawnerSystem particleSpawnerSystem, IObjectResolver objectResolver)
        {
            _gameConfiguration = gameConfiguration;
            _particleSpawnerSystem = particleSpawnerSystem;
            _objectResolver = objectResolver;
        }
        
        public void Initialize()
        {
            _woos = new Dictionary<Woo, float>();
        }

        public void Dispose()
        {
            foreach (var wood in _woos)
            {
                UnityEngine.Object.Destroy(wood.Key.gameObject);
            }
            
            _woos.Clear();
        }
        
        public Woo Create(int size, Vector3 position, Capsule capsule)
        {
            var prefab = GetPrefab(size);
            var woo = _objectResolver.Instantiate(prefab, position, Quaternion.identity);
            woo.Capsule = capsule;
            _woos.Add(woo, Time.time);
            
            
            return woo;
        }

        public void Destroy(Woo wood, bool fromPressure)
        {
            _particleSpawnerSystem.SpawnWooDestroyedParticle(wood.Center.position, wood.Size, fromPressure);
            _woos.Remove(wood);
            UnityEngine.Object.Destroy(wood.gameObject);
        }
        
        public IEnumerable<Woo> GetWoos()
        {
            return _woos.Keys;
        }
        
        private Woo GetPrefab(int size)
        {
            switch (size)
            {
                case 1:
                    return _gameConfiguration.Woo1;
                default:
                    throw new System.NotImplementedException();
            }
        }

        public void Tick()
        {
            var expiredWoos = new List<Woo>();
            foreach (var (woo, lifetime) in _woos)
            {
                if (Time.time - lifetime > woo.Lifetime)
                {
                    expiredWoos.Add(woo);
                }
            }
            
            foreach (var woo in expiredWoos)
            {
                Destroy(woo, false);
            }
        }
    }
}