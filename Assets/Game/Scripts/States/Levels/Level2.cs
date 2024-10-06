using System.Threading;
using Cysharp.Threading.Tasks;
using Game.Scripts.Config;
using Game.Scripts.Entities;
using Game.Scripts.Systems;
using Game.Scripts.UI;
using Game.Scripts.Util;

namespace Game.Scripts.States.Levels
{
    public class Level2 : GameState
    {
        private readonly LevelUI _levelUI;
        private readonly LevelState _levelState;
        private readonly LevelResetSystem _levelResetSystem;
        private readonly NarrationUI _narrationUI;
        private readonly Narrators _narrators;
        private readonly Environment _environment;

        public Level2(GameFSM gameFsm, LevelUI levelUI, LevelState levelState, LevelResetSystem levelResetSystem, NarrationUI narrationUI, Narrators narrators, Environment environment) : base(gameFsm)
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
            await _narrationUI.ShowText("Great job <b>WORKER</b>!", _narrators.Triangle);
            await _narrationUI.ShowText("As a matter of fact management decided that we can use <b>SQUARES</b> too", _narrators.Triangle);
            await _narrationUI.ShowText("That's why you are being transferred to the <b>SQUARES</b> operations team", _narrators.Triangle);
            
            await _environment.ToggleRightConveyorAsync(false);
            await UniTask.Delay(200, cancellationToken: cancellationToken);
            await _environment.ToggleLeftConveyorAsync(true);
            
            await _narrationUI.ShowText("Please make sure that no <b>CIRCLES</b> get into the can.", _narrators.Triangle);
            await _narrationUI.ShowText("If you ever see a <b>CIRCLE</b> - click on him as fast as you can so that he becomes a normal fine grained <b>SQUARE</b>", _narrators.Triangle);
            await _narrationUI.ShowText("Good luck!", _narrators.Triangle);
            
            await _narrationUI.HideAsync();
            
            
            _levelUI.gameObject.SetActive(true);
            _levelState.InvalidRedChance = 0f;
            _levelState.InvalidNormalChance = 0.5f;
            _levelState.IsPaused = false;
            
            var waitForWin = UniTask.WaitUntil(() => _levelState.CorrectWoos == 5, cancellationToken: cancellationToken);
            var waitForLose = UniTask.WaitUntil(() => _levelState.WrongWoos == 10, cancellationToken: cancellationToken);

            int result = await UniTask.WhenAny(waitForWin, waitForLose);

            _levelState.IsPaused = true;
            
            _levelUI.gameObject.SetActive(false);
            
            if (result == 0)
            {
                CompleteAndGoToLevel<Level3>(_levelState);
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