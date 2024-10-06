using System.Threading;
using Cysharp.Threading.Tasks;
using Game.Scripts.Entities;

namespace Game.Scripts.States
{
    public abstract class GameState
    {
        protected readonly GameFSM GameFsm;
        public object context { get; set; }

        protected GameState(GameFSM gameFsm)
        {
            GameFsm = gameFsm;
        }

        public abstract UniTask OnRun(CancellationToken cancellationToken = default);

        protected void FailThisLevel()
        {
            GameFsm.ChangeState<LevelFailedState>(new LevelFailedState.Context(GetType()));
        }
        
        protected void CompleteAndGoToLevel<TLevel>(LevelState levelState) where TLevel : GameState
        {
            GameFsm.ChangeState<LevelCompletedState>(new LevelCompletedState.Context(levelState, typeof(TLevel)));
        }
    }
}