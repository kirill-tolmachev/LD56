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
        
        public Woo Create(WooType type, int size, Vector3 position, Capsule capsule = null, Slot slot = null)
        {
            var prefab = GetPrefab(type, size);
            var woo = _objectResolver.Instantiate(prefab, position, Quaternion.identity);
            woo.Capsule = capsule;
            woo.Slot = slot;
            
            if (slot != null)
                slot.Woo = woo;
            
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
        
        private Woo GetPrefab(WooType type, int size)
        {
            switch (type)
            {
                case WooType.Circle:
                    return _gameConfiguration.WooCircle;
                case WooType.Square:
                    return _gameConfiguration.WooSquare;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }

        public void Tick()
        {
            // var expiredWoos = new List<Woo>();
            // foreach (var (woo, lifetime) in _woos)
            // {
            //     if (Time.time - lifetime > woo.Lifetime)
            //     {
            //         expiredWoos.Add(woo);
            //     }
            // }
            //
            // foreach (var woo in expiredWoos)
            // {
            //     Destroy(woo, false);
            // }
        }
    }
}