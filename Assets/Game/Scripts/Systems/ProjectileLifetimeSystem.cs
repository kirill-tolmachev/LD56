using System;
using System.Collections.Generic;
using System.Linq;
using Game.Scripts.Config;
using Game.Scripts.Entities;
using UnityEngine;
using VContainer.Unity;

namespace Game.Scripts.Systems
{
    public class ProjectileLifetimeSystem : ITickable, IInitializable, IDisposable
    {
        private readonly GameConfiguration _gameConfiguration;
        private HashSet<Projectile> _projectiles;
        
        public ProjectileLifetimeSystem(GameConfiguration gameConfiguration)
        {
            _gameConfiguration = gameConfiguration;
        }
        
        public void Initialize()
        {
            _projectiles = new HashSet<Projectile>();
        }
        
        public void Dispose()
        {
            var projectiles = _projectiles.ToList();
            foreach (var projectile in projectiles)
            {
                if (projectile == null)
                    continue;
                
                UnityEngine.Object.Destroy(projectile.gameObject);
            }
            
            _projectiles.Clear();
        }
        
        public void Add(Projectile projectile)
        {
            _projectiles.Add(projectile);
        }
        
        public void Destroy(Projectile projectile)
        {
            _projectiles.Remove(projectile);
            UnityEngine.Object.Destroy(projectile.gameObject);
        }
        
        public void Tick()
        {
            foreach (var projectile in _projectiles)
            {
                projectile.transform.position += projectile.transform.forward * _gameConfiguration.ProjectileSpeed * Time.deltaTime;
            }
        }
    }
}