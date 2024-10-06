using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Game.Scripts.Config;
using Game.Scripts.Entities;
using Game.Scripts.Systems;
using Game.Scripts.UI;
using Game.Scripts.Util;

namespace Game.Scripts.States.Levels
{
    public class Level1 : GameState
    {
        private readonly AudioManager _audioManager;
        private readonly LevelUI _levelUI;
        private readonly LevelState _levelState;
        private readonly LevelResetSystem _levelResetSystem;
        private readonly NarrationUI _narrationUI;
        private readonly Narrators _narrators;
        private readonly Environment _environment;

        public Level1(GameFSM gameFsm, AudioManager audioManager, LevelUI levelUI, LevelState levelState, LevelResetSystem levelResetSystem, NarrationUI narrationUI, Narrators narrators, Environment environment) : base(gameFsm)
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
            
            _narrationUI.Show();
            await _narrationUI.ShowText("Good day <b>WORKER</b>!", _narrators.Triangle);
            await _narrationUI.ShowText("Congratulations with your promotion to the station operator", _narrators.Triangle);
            await _narrationUI.ShowText("As you know, our business here is to squeeze <color=black>WOOS</color> in the can.", _narrators.Triangle);
            await _narrationUI.ShowText("However we only need <b>CIRCLES</b>", _narrators.Triangle);
            await _narrationUI.ShowText("Please make sure that no <b>SQUARES</b> get into the can.", _narrators.Triangle);
            await _narrationUI.ShowText("If you ever see a <b>SQUARE</b> - click on him as fast as you can so that he becomes a normal fine grained <b>CIRCLE</b>", _narrators.Triangle);
            await _narrationUI.ShowText("I know you're a bit nervous, but I'm sure you'll be fine.", _narrators.Triangle);
            await _narrationUI.ShowText("Good luck!", _narrators.Triangle);
            
            await _narrationUI.HideAsync();

            _levelUI.gameObject.SetActive(true);
            
            _levelState.InvalidRedChance = 0f;
            _levelState.InvalidNormalChance = 0.5f;
            _levelState.IsPaused = false;
            
            var waitForWin = UniTask.WaitUntil(() => _levelState.CorrectWoos >= 5, cancellationToken: cancellationToken);
            var waitForLose = UniTask.WaitUntil(() => _levelState.WrongWoos == 10, cancellationToken: cancellationToken);

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