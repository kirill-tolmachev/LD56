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
    public class Level6_Corp : GameState
    {
        private readonly LevelUI _levelUI;
        private readonly LevelState _levelState;
        private readonly AudioManager _audioManager;
        private readonly LevelResetSystem _levelResetSystem;
        private readonly NarrationUI _narrationUI;
        private readonly Narrators _narrators;
        private readonly Environment _environment;

        public Level6_Corp(GameFSM gameFsm, LevelUI levelUI, LevelState levelState, AudioManager audioManager, LevelResetSystem levelResetSystem, NarrationUI narrationUI, Narrators narrators, Environment environment) : base(gameFsm)
        {
            _levelUI = levelUI;
            _levelState = levelState;
            _audioManager = audioManager;
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
            await _narrationUI.ShowText("Just make sure there are <b>NO CIRCLES</b> in the can.", _narrators.Rebel);
            
            await _narrationUI.ShowText("Good luck!", _narrators.Rebel);
            await _narrationUI.HideAsync();
            
            
            _levelUI.gameObject.SetActive(true);
            _levelState.IsPaused = false;
            _levelState.InvalidRedChance = 0.5f;
            _levelState.InvalidNormalChance = 0.1f;

            int timerSeconds = 30;
            
            _levelState.TimerStartTime = Time.time;
            _levelState.TimerMaxTime = timerSeconds;
            _levelUI.ToggleTimer(true);
            
            var waitForWin = UniTask.Delay(timerSeconds * 1000, cancellationToken: cancellationToken);
            var waitForLose = UniTask.WaitUntil(() => _levelState.TotalCircles > 0, cancellationToken: cancellationToken);

            int result = await UniTask.WhenAny(waitForWin, waitForLose);

            _levelState.IsPaused = true;
            
            _levelUI.gameObject.SetActive(false);
            
            if (result == 0)
            {
                CompleteAndGoToLevel<Level1>(_levelState);
                return;
            }
            
            if (result == 1)
            {
                FailThisLevel();
                return;
            }
            
            _levelUI.ToggleTimer(false);
        }
    }
}