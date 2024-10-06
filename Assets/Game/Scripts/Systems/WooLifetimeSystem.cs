using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Game.Scripts.Config;
using Game.Scripts.Entities;
using Game.Scripts.Util;
using UnityEngine;
using UnityEngine.EventSystems;
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
        private readonly AudioManager _audioManager;

        public WooLifetimeSystem(GameConfiguration gameConfiguration, ParticleSpawnerSystem particleSpawnerSystem, IObjectResolver objectResolver, AudioManager audioManager)
        {
            _gameConfiguration = gameConfiguration;
            _particleSpawnerSystem = particleSpawnerSystem;
            _objectResolver = objectResolver;
            _audioManager = audioManager;
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
        
        public Woo Create(WooType type, int size, Vector3 position)
        {
            var prefab = GetPrefab(type, size);
            var woo = _objectResolver.Instantiate(prefab, position, Quaternion.identity);
            var scale = woo.transform.localScale;
            woo.transform.localScale = scale * 0.5f;
            woo.transform.DOScale(scale, 0.2f).SetEase(Ease.OutBack).SetId(woo.transform).SetAutoKill(true);
            
            _woos.Add(woo, Time.time);
            
            
            return woo;
        }

        public void Reset()
        {
            foreach (var (woo, _) in _woos.ToList())
            {
                Destroy(woo, false);
            }
            
            _woos.Clear();
        }

        public void Destroy(Woo woo, bool fromPressure)
        {
            _particleSpawnerSystem.SpawnWooDestroyedParticle(woo, woo.Center.position, woo.Size, fromPressure);
            _woos.Remove(woo);
            
            if (EventSystem.current != null && EventSystem.current.currentSelectedGameObject == woo.gameObject)
            {
                EventSystem.current.SetSelectedGameObject(null);
            }
            
            woo.gameObject.SetActive(false);
            UnityEngine.Object.Destroy(woo.gameObject, 0.1f); // Then destroy it
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
                case WooType.SquareRed:
                    return _gameConfiguration.WooSquareRed;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
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

        public void Alter(Woo woo)
        {
            var newType = GetWooAlterType(woo);
            var pos = woo.Center.position;
            var size = woo.Size;
            
            Destroy(woo, false);
            Create(newType, size, pos);
            
            _audioManager.PlaySwapWooAudio();
        }

        private WooType GetWooAlterType(Woo woo)
        {
            if (woo.Type == WooType.SquareRed && woo.Origin != null)
                return woo.Origin.ExpectedWooType;
            
            return woo.Type == WooType.Circle ? WooType.Square : WooType.Circle;

        }
    }
}