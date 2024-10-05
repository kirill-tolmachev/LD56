using Game.Scripts.Config;
using Game.Scripts.Entities;
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
            builder.RegisterComponentInHierarchy<DragDirectionArrow>().AsSelf();
            
            builder.RegisterInstance<PlayerState>(new PlayerState());
            builder.RegisterInstance<Camera>(Camera.main);
            
            builder.RegisterEntryPoint<WooLifetimeSystem>().AsSelf();
            builder.RegisterEntryPoint<ParticleSpawnerSystem>().AsSelf();
            
            builder.Register<LevelState>(Lifetime.Singleton).AsSelf();
            builder.Register<ScoreSystem>(Lifetime.Singleton).AsSelf();
            builder.Register<MergeSystem>(Lifetime.Singleton).AsSelf();
        }
    }
}