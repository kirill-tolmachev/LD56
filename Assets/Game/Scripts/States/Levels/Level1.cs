using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Game.Scripts.Config;
using Game.Scripts.Entities;
using Game.Scripts.Systems;
using Game.Scripts.UI;
using Game.Scripts.UI.Progress;
using Game.Scripts.Util;

namespace Game.Scripts.States.Levels
{
    public class Level1 : GameState
    {
        private readonly AudioManager _audioManager;
        private readonly Level1UI _levelUI;
        private readonly LevelState _levelState;
        private readonly LevelResetSystem _levelResetSystem;
        private readonly NarrationUI _narrationUI;
        private readonly Narrators _narrators;
        private readonly Environment _environment;

        public Level1(GameFSM gameFsm, AudioManager audioManager, Level1UI levelUI, LevelState levelState, LevelResetSystem levelResetSystem, NarrationUI narrationUI, Narrators narrators, Environment environment) : base(gameFsm)
        {
            _audioManager = audioManager;
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
            _audioManager.PlayNormalBackgroundMusic();
            
            _environment.ToggleLeftConveyor(false);
            _environment.ToggleRightConveyor(true);
            _environment.ToggleCapsule(true);
            
            _narrationUI.Show();
            await _narrationUI.ShowText("Ah, there you are, <b>WORKER</b>!", _narrators.Triangle);
            await _narrationUI.ShowText("Welcome to your first day as a Station Operator.", _narrators.Triangle);
            await _narrationUI.ShowText("Our mission is simple: pack <color=black>WOOS</color> into cans.", _narrators.Triangle);
            await _narrationUI.ShowText("But we've been specifically informed - we only accept <b>CIRCLES</b>.", _narrators.Triangle);
            await _narrationUI.ShowText("We can't have any <b>SQUARES</b> slipping through.", _narrators.Triangle);
            await _narrationUI.ShowText("See a <b>SQUARE</b>? Click it swiftly to reshape it into a proper <b>CIRCLE</b>.", _narrators.Triangle);
            await _narrationUI.ShowText("Feeling nervous? Don't be. I've got confidence in you.", _narrators.Triangle);
            await _narrationUI.ShowText("Now, let's make some perfect cans!", _narrators.Triangle);
            
            await _narrationUI.HideAsync();

            _levelUI.gameObject.SetActive(true);
            
            _levelState.InvalidRedChance = 0f;
            _levelState.InvalidNormalChance = 0.8f;
            _levelState.IsPaused = false;
            
            var waitForWin = UniTask.WaitUntil(() => _levelState.CorrectWoos >= 15, cancellationToken: cancellationToken);
            var waitForLose = UniTask.WaitUntil(() => _levelState.WrongWoos == 5, cancellationToken: cancellationToken);

            int result = await UniTask.WhenAny(waitForWin, waitForLose);

            _levelState.IsPaused = true;
            
            _levelUI.gameObject.SetActive(false);
            
            if (result == 0)
            {
                CompleteAndGoToLevel<Level2>(_levelState);
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