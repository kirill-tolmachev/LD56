using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Game.Scripts.Entities;
using Game.Scripts.States.Levels;
using Game.Scripts.UI;

namespace Game.Scripts.States
{
    public class LevelFailedState : GameState
    {
        public class Context
        {
            public Type NextState { get; }

            public Context(Type nextState)
            {
                NextState = nextState;
            }
        }
        
        private readonly LevelFailedUI _levelFailedUI;

        public LevelFailedState(GameFSM gameFsm, LevelFailedUI levelFailedUI) : base(gameFsm)
        {
            _levelFailedUI = levelFailedUI;
        }

        public override async UniTask OnRun(CancellationToken cancellationToken = default)
        {
            var result =await _levelFailedUI.Show();
            
            var ctx = (Context)context;
            
            if (result)
            {
                GameFsm.ChangeState(ctx.NextState);
                return;
            }
            
            GameFsm.ChangeState<SplashState>();
        }
    }
}