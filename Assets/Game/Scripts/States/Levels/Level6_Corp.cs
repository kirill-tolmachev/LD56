using System.Threading;
using Cysharp.Threading.Tasks;
using Game.Scripts.Config;
using Game.Scripts.Entities;
using Game.Scripts.Systems;
using Game.Scripts.UI;
using Game.Scripts.Util;

namespace Game.Scripts.States.Levels
{
    public class Level6_Corp : GameState
    {
        private readonly LevelUI _levelUI;
        private readonly LevelState _levelState;
        private readonly LevelResetSystem _levelResetSystem;
        private readonly NarrationUI _narrationUI;
        private readonly Narrators _narrators;
        private readonly Environment _environment;

        public Level6_Corp(GameFSM gameFsm, LevelUI levelUI, LevelState levelState, LevelResetSystem levelResetSystem, NarrationUI narrationUI, Narrators narrators, Environment environment) : base(gameFsm)
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
            _environment.ToggleRightConveyor(true);
            _environment.ToggleLeftConveyor(true);
            
            await _narrationUI.ShowText("Great job <b>WORKER</b>!", _narrators.Triangle);
            await _narrationUI.ShowText("We have discovered unusual activity in the <b>SQUARES</b> operations team", _narrators.Triangle);
            await _narrationUI.ShowText("Apparently some <b>SQUARES</b> are not behaving as expected", _narrators.Triangle);
            await _narrationUI.ShowText("They are... <color=red>RED</color>", _narrators.Triangle);
            await _narrationUI.ShowText("While we are trying to figure out what is going on - please make sure that no <color=red><b>RED SQUARES</b></color> get into the can.", _narrators.Triangle);
            await _narrationUI.ShowText("Of course the old requirements remain the same - <b>SQUARES</b> on the left and <b>CIRCLES</b> on the right", _narrators.Triangle);
            
            await _narrationUI.ShowText("Good luck!", _narrators.Triangle);
            
            await _narrationUI.HideAsync();
            
            
            _levelUI.gameObject.SetActive(true);
            _levelState.IsPaused = false;
            _levelState.InvalidRedChance = 0.5f;
            _levelState.InvalidNormalChance = 0.1f;
            
            var waitForWin = UniTask.WaitUntil(() => _levelState.CorrectWoos == 5, cancellationToken: cancellationToken);
            var waitForLose = UniTask.WaitUntil(() => _levelState.WrongWoos == 10, cancellationToken: cancellationToken);

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
        }
    }
}