using System;
using Game.Scripts.Config;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Game.Scripts.Systems
{
    public class ProjectileSpawnerSystem : IInitializable, IDisposable, ITickable
    {
        private readonly PlayerState _playerState;
        private readonly GameConfiguration _gameConfiguration;
        private readonly IObjectResolver _resolver;
        private readonly ProjectileLifetimeSystem _projectileLifetimeSystem;

        private float _lastShotTime;

        public ProjectileSpawnerSystem(PlayerState playerState, GameConfiguration gameConfiguration, IObjectResolver resolver, ProjectileLifetimeSystem projectileLifetimeSystem)
        {
            _playerState = playerState;
            _gameConfiguration = gameConfiguration;
            _resolver = resolver;
            _projectileLifetimeSystem = projectileLifetimeSystem;
        }
        
        public void Initialize()
        {
        }
        
        public void Dispose()
        {
        }

        public void Tick()
        {
            if (Time.time - _lastShotTime < _playerState.ShotInterval)
                return;
            
            var projectile = _resolver.Instantiate(_gameConfiguration.ProjectilePrefab);
            projectile.transform.position = _playerState.AimPosition;
            projectile.transform.rotation = Quaternion.LookRotation(_playerState.AimPosition - _playerState.Position);
            _projectileLifetimeSystem.Add(projectile);
            
            _lastShotTime = Time.time;
        }
    }
}