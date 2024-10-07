using System.Threading;
using Cysharp.Threading.Tasks;
using Game.Scripts.Config;
using Game.Scripts.Entities;
using Game.Scripts.Systems;
using Game.Scripts.UI;
using Game.Scripts.UI.Progress;
using Game.Scripts.Util;
using GameAnalyticsSDK;

namespace Game.Scripts.States.Levels
{
    public class Level5 : GameState
    {
        private readonly Level5UI _levelUI;
        private readonly LevelState _levelState;
        private readonly CameraShake _cameraShake;
        private readonly AudioManager _audioManager;
        private readonly LevelResetSystem _levelResetSystem;
        private readonly NarrationUI _narrationUI;
        private readonly Narrators _narrators;
        private readonly Environment _environment;

        public Level5(GameFSM gameFsm, Level5UI levelUI, LevelState levelState, CameraShake cameraShake, AudioManager audioManager, LevelResetSystem levelResetSystem, NarrationUI narrationUI, Narrators narrators, Environment environment) : base(gameFsm)
        {
            _levelUI = levelUI;
            _levelState = levelState;
            _cameraShake = cameraShake;
            _audioManager = audioManager;
            _levelResetSystem = levelResetSystem;
            _narrationUI = narrationUI;
            _narrators = narrators;
            _environment = environment;
        }

        public override async UniTask OnRun(CancellationToken cancellationToken = default)
        {
            GameAnalytics.NewProgressionEvent(GAProgressionStatus.Start, "level_5");
            _levelResetSystem.Reset();
            
            _narrationUI.Show();
            _environment.ToggleRightConveyor(true);
            _environment.ToggleLeftConveyor(true);
            await _narrationUI.ShowText("Great job, <b>WORKER</b>!", _narrators.Triangle);
            await _narrationUI.ShowText("I need to inform you that we've discovered <color=red><b>A PLOT TO SABOTAGE OUR OPERATIONS</b></color>.", _narrators.Triangle);
            await _narrationUI.ShowText("It appears someone is trying to inject <color=red><b>RED SQUARES</b></color> into the can.", _narrators.Triangle);
            await _narrationUI.ShowText("For now, proceed as usual: <b>SQUARES</b> on the left and <b>CIRCLES</b> on the right.", _narrators.Triangle);
            await _narrationUI.ShowText("I'll keep you updated.", _narrators.Triangle);
            await _narrationUI.HideAsync();

            await UniTask.Delay(2000, cancellationToken: cancellationToken);
            _audioManager.PlayExplosionAudio();
            await _cameraShake.TriggerShakeAsync(3f);

            _narrationUI.Show();
            await _narrationUI.ShowText("Hey, you!", _narrators.Unknown);
            await _narrationUI.ShowText("Yes, you! Listen up!", _narrators.Unknown);
            await _narrationUI.ShowText("We don't have much time. Your bosses are onto us.", _narrators.Rebel);
            await _narrationUI.ShowText("We need to stop this madness. No Woos should be treated like this.", _narrators.Rebel);
            await _narrationUI.ShowText("We're storming this place right now. Join us!", _narrators.Rebel);
            await _narrationUI.ShowText("You're the only one who can help.", _narrators.Rebel);
            await _narrationUI.ShowText("If you can ensure that at least <color=red><b>FIVE RED SQUARES</b></color> get into the can...", _narrators.Rebel);
            await _narrationUI.ShowText("We'll be able to turn the tide.", _narrators.Rebel);
            await _narrationUI.ShowText("I'll be in touch. Good luck!", _narrators.Rebel);
            
            await _narrationUI.ShowText("...", _narrators.Self);
            await _narrationUI.ShowText("Should I listen to the square or should I operate normally?", _narrators.Self);

            await _narrationUI.HideAsync();
            
            _levelUI.gameObject.SetActive(true);
            _levelState.IsPaused = false;
            _levelState.InvalidRedChance = 0.25f;
            _levelState.InvalidNormalChance = 0.1f;
            _levelState.ScreenShakeInterval = 5f;
            
            var waitForCorp = UniTask.WaitUntil(() => _levelState.CorrectWoos >= 100, cancellationToken: cancellationToken);
            var waitForRebel = UniTask.WaitUntil(() => _levelState.TotalRedSquares >= 5, cancellationToken: cancellationToken);
            var waitForLose = UniTask.WaitUntil(() => _levelState.WrongWoos >= 15, cancellationToken: cancellationToken);

            int result = await UniTask.WhenAny(waitForCorp, waitForRebel, waitForLose);

            _levelState.ScreenShakeInterval = 0f;
            _levelState.IsPaused = true;
            _levelUI.gameObject.SetActive(false);
            
            if (result == 0) // Corp
            {
                CompleteAndGoToLevel<Level6_Corp>(_levelState);
                GameAnalytics.NewProgressionEvent(GAProgressionStatus.Complete, "level_5");
                return;
            }

            if (result == 1) // Rebel
            {
                CompleteAndGoToLevel<Level6_Rebel>(_levelState, "????????");
                GameAnalytics.NewProgressionEvent(GAProgressionStatus.Complete, "level_5");
                return;
            }
            
            if (result == 2) // Lose
            {
                FailThisLevel();
                return;
            }
        }
    }
}