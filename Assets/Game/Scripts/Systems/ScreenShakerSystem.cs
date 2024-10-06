using Cysharp.Threading.Tasks;
using Game.Scripts.Config;
using Game.Scripts.Entities;
using Game.Scripts.Util;
using UnityEngine;
using VContainer.Unity;

namespace Game.Scripts.Systems
{
    public class ScreenShakerSystem : ITickable
    {
        private readonly LevelState _levelState;
        private readonly CameraShake _cameraShake;
        private readonly AudioManager _audioManager;
        private float _lastShakeTime;

        public bool IsActive { get; set; } = true;

        public ScreenShakerSystem(LevelState levelState, CameraShake cameraShake, AudioManager audioManager)
        {
            _levelState = levelState;
            _cameraShake = cameraShake;
            _audioManager = audioManager;
        }
        
        
        public void Tick()
        {
            if (!IsActive)
                return;
            
            if (_levelState.ScreenShakeInterval <= 0f)
                return;
            
            if (Time.time - _lastShakeTime < 0.5f)
                return;
            
            _lastShakeTime = Time.time;
            
            _cameraShake.TriggerShakeAsync(0.5f).Forget();
            _audioManager.PlayExplosionAudio();
        }
    }
}