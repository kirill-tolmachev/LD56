using System.Threading;
using Cysharp.Threading.Tasks;
using Game.Scripts.Config;
using Game.Scripts.Entities;
using Game.Scripts.Systems;
using Game.Scripts.UI;
using Game.Scripts.Util;
using UnityEngine;

namespace Game.Scripts.States.Levels
{
    public class Level6_Rebel : GameState
    {
        private readonly LevelUI _levelUI;
        private readonly LevelState _levelState;
        private readonly AudioManager _audioManager;
        private readonly CameraShake _cameraShaker;
        private readonly ParticleSpawnerSystem _particleSpawnerSystem;
        private readonly LevelResetSystem _levelResetSystem;
        private readonly NarrationUI _narrationUI;
        private readonly Narrators _narrators;
        private readonly Environment _environment;

        public Level6_Rebel(GameFSM gameFsm, LevelUI levelUI, LevelState levelState, AudioManager audioManager, CameraShake cameraShaker, ParticleSpawnerSystem particleSpawnerSystem, LevelResetSystem levelResetSystem, NarrationUI narrationUI, Narrators narrators, Environment environment) : base(gameFsm)
        {
            _levelUI = levelUI;
            _levelState = levelState;
            _audioManager = audioManager;
            _cameraShaker = cameraShaker;
            _particleSpawnerSystem = particleSpawnerSystem;
            _levelResetSystem = levelResetSystem;
            _narrationUI = narrationUI;
            _narrators = narrators;
            _environment = environment;
        }

        public override async UniTask OnRun(CancellationToken cancellationToken = default)
        {
            _levelResetSystem.Reset();
            _audioManager.PlayReversedBackgroundMusic();
            
            _narrationUI.Show();
            _environment.ToggleRightConveyor(true);
            _environment.ToggleLeftConveyor(true);
            
            await _narrationUI.ShowText("Great job!", _narrators.Rebel);
            await _narrationUI.ShowText("Now we can finally destroy this hellish machine!", _narrators.Rebel);
            await _narrationUI.ShowText("All you need to do is make sure <b>NO CIRCLES</b> get into the can from both ends.", _narrators.Rebel);
            await _narrationUI.ShowText("Let's fill this thing with <b>SQUARES</b> for the final time!", _narrators.Rebel);
            await _narrationUI.ShowText("Both <b>BLACK SQUARES</b> and <color=red><b>RED SQUARES</b></color> will do the job.", _narrators.Rebel);
            await _narrationUI.ShowText("Just make sure that <b>NO MORE THAN 10 CIRCLES</b> get in the can.", _narrators.Rebel);
            await _narrationUI.ShowText("You need to hold up for the last 30 seconds.", _narrators.Rebel);
            
            await _narrationUI.ShowText("Good luck!", _narrators.Rebel);
            await _narrationUI.HideAsync();
            
            
            _levelUI.gameObject.SetActive(true);
            _levelState.IsPaused = false;
            _levelState.InvalidRedChance = 0.5f;
            _levelState.InvalidNormalChance = 0.1f;

            int timer = 30;
            
            _levelState.TimerStartTime = Time.time;
            _levelState.TimerMaxTime = timer;
            _levelUI.ToggleTimer(true);
            
            var waitForWin = UniTask.Delay(timer * 1000, cancellationToken: cancellationToken);
            var waitForLose = UniTask.WaitUntil(() => _levelState.TotalCircles >= 10, cancellationToken: cancellationToken);

            int result = await UniTask.WhenAny(waitForWin, waitForLose);
            _levelUI.ToggleTimer(false);
            _levelState.IsPaused = true;
            
            _levelUI.gameObject.SetActive(false);
            
            if (result == 0)
            {
                _particleSpawnerSystem.SpawnBigExplosion(Vector3.zero, Quaternion.identity);
                _audioManager.PlayExplosionAudio();
                
                await _cameraShaker.TriggerShakeAsync(3f);
                
                
                CompleteAndGoToLevel<Level1>(_levelState);
                return;
            }
            
            if (result == 1)
            {
                FailThisLevel();
                return;
            }
        }
    }
}