using DG.Tweening;
using Game.Scripts.Config;
using Game.Scripts.Entities;
using Game.Scripts.States;
using Game.Scripts.States.Levels;
using Game.Scripts.Systems;
using Game.Scripts.UI;
using Game.Scripts.UI.Progress;
using Game.Scripts.Util;
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
            builder.RegisterInstance(_gameConfiguration.Narrators);
            builder.RegisterComponentInHierarchy<SplashScreen>().AsSelf();
            builder.RegisterComponentInHierarchy<AudioManager>().AsSelf();
            builder.RegisterComponentInHierarchy<ResultDetection>().AsSelf();
            builder.RegisterComponentInHierarchy<NarrationUI>().AsSelf();
            builder.RegisterComponentInHierarchy<Environment>().AsSelf();
            builder.RegisterComponentInHierarchy<CameraShake>().AsSelf();
            builder.RegisterComponentInHierarchy<CreditsUI>().AsSelf();
            
            builder.RegisterComponentInHierarchy<LevelCompletedUI>().AsSelf();
            builder.RegisterComponentInHierarchy<LevelFailedUI>().AsSelf();
            
            builder.RegisterInstance<PlayerState>(new PlayerState());
            builder.RegisterInstance<Camera>(Camera.main);
            
            builder.RegisterEntryPoint<WooLifetimeSystem>().AsSelf();
            builder.RegisterEntryPoint<ParticleSpawnerSystem>().AsSelf();
            builder.RegisterEntryPoint<LevelResultSystem>().AsSelf();
            
            builder.Register<LevelState>(Lifetime.Singleton).AsSelf();
            builder.Register<ScoreSystem>(Lifetime.Singleton).AsSelf();
            builder.Register<LevelResetSystem>(Lifetime.Singleton).AsSelf();
            
            builder.RegisterEntryPoint<GameFSM>().AsSelf();
            builder.Register<SplashState>(Lifetime.Transient).AsSelf();
            builder.Register<Level1>(Lifetime.Transient).AsSelf();
            builder.Register<Level2>(Lifetime.Transient).AsSelf();
            builder.Register<Level3>(Lifetime.Transient).AsSelf();
            builder.Register<Level4>(Lifetime.Transient).AsSelf();
            builder.Register<Level5>(Lifetime.Transient).AsSelf();
            builder.Register<Level6_Corp>(Lifetime.Transient).AsSelf();
            builder.Register<Level6_Rebel>(Lifetime.Transient).AsSelf();
            builder.Register<CreditsState>(Lifetime.Transient).AsSelf();
            builder.Register<LevelCompletedState>(Lifetime.Transient).AsSelf();
            builder.Register<LevelFailedState>(Lifetime.Transient).AsSelf();
            
            
            builder.RegisterComponentInHierarchy<Level1UI>().AsSelf();
            builder.RegisterComponentInHierarchy<Level2UI>().AsSelf();
            builder.RegisterComponentInHierarchy<Level3UI>().AsSelf();
            builder.RegisterComponentInHierarchy<Level4UI>().AsSelf();
            builder.RegisterComponentInHierarchy<Level5UI>().AsSelf();
            builder.RegisterComponentInHierarchy<Level6UI>().AsSelf();
        }

        private void Start()
        {
            DOTween.Init();
        }
    }
}