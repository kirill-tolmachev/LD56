using System.Threading;
using Cysharp.Threading.Tasks;
using Game.Scripts.Config;
using Game.Scripts.Entities;
using Game.Scripts.Systems;
using Game.Scripts.UI;
using Game.Scripts.UI.Progress;
using Game.Scripts.Util;

namespace Game.Scripts.States.Levels
{
    public class Level3 : GameState
    {
        private readonly Level3UI _levelUI;
        private readonly LevelState _levelState;
        private readonly LevelResetSystem _levelResetSystem;
        private readonly NarrationUI _narrationUI;
        private readonly Narrators _narrators;
        private readonly Environment _environment;

        public Level3(GameFSM gameFsm, Level3UI levelUI, LevelState levelState, LevelResetSystem levelResetSystem, NarrationUI narrationUI, Narrators narrators, Environment environment) : base(gameFsm)
        {
            _levelUI = levelUI;
            _levelState = levelState;
            _levelResetSystem = levelResetSystem;
            _narrationUI = narrationUI;
            _narrators = narrators;
            _environment = environment;
        }

        public override async UniTask OnRun(CancellationToken cancellationToken = default)
        {
            _levelResetSystem.Reset();
            
            _narrationUI.Show();
            await _narrationUI.ShowText("Excellent work, <b>WORKER</b>!", _narrators.Triangle);
            await _narrationUI.ShowText("Management has decided that we can now use both <b>SQUARES</b> and <b>CIRCLES</b>.", _narrators.Triangle);

            await _environment.ToggleRightConveyorAsync(true);
            await _environment.ToggleLeftConveyorAsync(true);

            await _narrationUI.ShowText("However, we only need <b>SQUARES</b> from the left conveyor...", _narrators.Triangle);
            await _narrationUI.ShowText("...and only <b>CIRCLES</b> from the right conveyor.", _narrators.Triangle);
            await _narrationUI.ShowText("If you see a Woo on the wrong side, click it quickly...", _narrators.Triangle);
            await _narrationUI.ShowText("...to transform it into the <b>PROPER WHATEVER IT NEEDS TO BE</b>.", _narrators.Triangle);

            await _narrationUI.ShowText("Let's keep things running smoothly. Good luck!", _narrators.Triangle);
            
            await _narrationUI.HideAsync();
            
            
            _levelUI.gameObject.SetActive(true);
            
            _levelState.InvalidRedChance = 0f;
            _levelState.InvalidNormalChance = 0.3f;
            _levelState.IsPaused = false;
            
            var waitForWin = UniTask.WaitUntil(() => _levelState.CorrectWoos == 50, cancellationToken: cancellationToken);
            var waitForLose = UniTask.WaitUntil(() => _levelState.WrongWoos == 10, cancellationToken: cancellationToken);

            int result = await UniTask.WhenAny(waitForWin, waitForLose);

            _levelState.IsPaused = true;
            
            _levelUI.gameObject.SetActive(false);
            
            if (result == 0)
            {
                CompleteAndGoToLevel<Level4>(_levelState);
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