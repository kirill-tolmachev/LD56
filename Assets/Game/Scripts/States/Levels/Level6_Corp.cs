using System.Threading;
using Cysharp.Threading.Tasks;
using Game.Scripts.Config;
using Game.Scripts.Entities;
using Game.Scripts.Systems;
using Game.Scripts.UI;
using Game.Scripts.UI.Progress;
using Game.Scripts.Util;
using GameAnalyticsSDK;
using UnityEngine;

namespace Game.Scripts.States.Levels
{
    public class Level6_Corp : GameState
    {
        private readonly Level6UI _levelUI;
        private readonly LevelState _levelState;
        private readonly AudioManager _audioManager;
        private readonly LevelResetSystem _levelResetSystem;
        private readonly NarrationUI _narrationUI;
        private readonly Narrators _narrators;
        private readonly Environment _environment;

        public Level6_Corp(GameFSM gameFsm, Level6UI levelUI, LevelState levelState, AudioManager audioManager, LevelResetSystem levelResetSystem, NarrationUI narrationUI, Narrators narrators, Environment environment) : base(gameFsm)
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
            GameAnalytics.NewProgressionEvent(GAProgressionStatus.Start, "level_6_corp");
            _levelResetSystem.Reset();
            
            _narrationUI.Show();
            
            _environment.ToggleRightConveyor(true);
            _environment.ToggleLeftConveyor(true);
            
            await _narrationUI.ShowText("<b>WORKER!</b>", _narrators.Triangle);
            await _narrationUI.ShowText("We are under attack by these treacherous rebels!", _narrators.Triangle);
            await _narrationUI.ShowText("But we will prevail if you follow my instructions exactly!", _narrators.Triangle);
            await _narrationUI.ShowText("Ensure that <b>NO MORE THAN 20 SQUARES</b> get into the can.", _narrators.Triangle);
            await _narrationUI.ShowText("That means no squares, red or black—<b>regardless of color</b>.", _narrators.Triangle);
            await _narrationUI.ShowText("You only need to hold them off for 30 seconds.", _narrators.Triangle);

            await _narrationUI.ShowText("Good luck!", _narrators.Triangle);
            await _narrationUI.HideAsync();
            
            
            _levelUI.gameObject.SetActive(true);
            _levelState.IsPaused = false;
            _levelState.InvalidRedChance = 0.3f;
            _levelState.InvalidNormalChance = 0.1f;

            int timerSeconds = 30;
            
            _levelState.TimerStartTime = Time.time;
            _levelState.TimerMaxTime = timerSeconds;
            _levelUI.ToggleTimer(true);
            
            var waitForWin = UniTask.Delay(timerSeconds * 1000, cancellationToken: cancellationToken);
            var waitForLose = UniTask.WaitUntil(() => _levelState.TotalRedSquares + _levelState.TotalSquares >= 20, cancellationToken: cancellationToken);

            int result = await UniTask.WhenAny(waitForWin, waitForLose);

            _levelState.IsPaused = true;
            
            _levelUI.gameObject.SetActive(false);
            
            if (result == 0)
            {
                GameFsm.ChangeState<CreditsState>(new CreditsState.Context(false));
                GameAnalytics.NewProgressionEvent(GAProgressionStatus.Complete, "level_6_corp");
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