using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Game.Scripts.Entities;
using Game.Scripts.UI;

namespace Game.Scripts.States
{
    public class LevelCompletedState : GameState
    {
        private readonly LevelCompletedUI _levelCompletedUI;

        public LevelCompletedState(GameFSM gameFsm, LevelCompletedUI levelCompletedUI) : base(gameFsm)
        {
            _levelCompletedUI = levelCompletedUI;
        }

        public override async UniTask OnRun(CancellationToken cancellationToken = default)
        {
            var ctx = (Context)context;

            await _levelCompletedUI.Show(ctx.LevelState, ctx.OverrideText);
            GameFsm.ChangeState(ctx.NextState);
        }

        public class Context
        {
            public LevelState LevelState { get; }
            public Type NextState { get; }
            public string OverrideText { get; }

            public Context(LevelState levelState, Type nextState, string overrideText = null)
            {
                LevelState = levelState;
                NextState = nextState;
                OverrideText = overrideText;
            }
        }
    }
}