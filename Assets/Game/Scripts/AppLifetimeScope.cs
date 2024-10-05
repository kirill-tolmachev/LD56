using Game.Scripts.Config;
using Game.Scripts.Systems;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Game.Scripts
{
    public class AppLifetimeScope : LifetimeScope
    {
        [SerializeField] private GameConfiguration _gameConfiguration;
        
        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterInstance(_gameConfiguration);
            builder.RegisterComponentInHierarchy<PlayerController>().AsSelf();
            builder.RegisterComponentInHierarchy<WorldBounds>();
            builder.RegisterInstance<PlayerState>(new PlayerState());
            builder.RegisterInstance<Camera>(Camera.main);

            builder.RegisterEntryPoint<ProjectileLifetimeSystem>().AsSelf();
            builder.RegisterEntryPoint<ProjectileSpawnerSystem>().AsSelf();
        }

        private void Start()
        {
            Container.Instantiate(_gameConfiguration.ProjectileBBoxPrefab);
        }
    }
}