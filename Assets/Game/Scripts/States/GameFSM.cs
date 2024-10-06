using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using VContainer;
using VContainer.Unity;

namespace Game.Scripts.States
{
    public class GameFSM : IStartable
    {
        private readonly IObjectResolver _objectResolver;
        public GameState CurrentState { get; private set; }

        private Type _nextState;
        private object _nextStateContext;

        public GameFSM(IObjectResolver objectResolver)
        {
            _objectResolver = objectResolver;
        }
        
        public void ChangeState<T>(object context = null) where T : GameState
        {
            _nextState = typeof(T);
            _nextStateContext = context;
        }

        public void ChangeState(Type type, object context = null)
        {
            _nextState = type;
            _nextStateContext = context;
        }

        private async UniTask RunAsync<T>(CancellationToken cancellationToken = default) where T : GameState
        {
            ChangeState<T>();

            while (_nextState != null)
            {
                var state = (GameState)_objectResolver.Resolve(_nextState);
                state.context = _nextStateContext;
                
                CurrentState = state;
                await state.OnRun(cancellationToken);
            }
        }

        public void Start()
        {
            RunAsync<SplashState>().Forget();
        }
    }
}