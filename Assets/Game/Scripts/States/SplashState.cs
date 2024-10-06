using System.Threading;
using Cysharp.Threading.Tasks;
using Game.Scripts.States.Levels;
using Game.Scripts.Util;

namespace Game.Scripts.States
{
    public class SplashState : GameState
    {
        private readonly SplashScreen _splashScreen;

        public SplashState(GameFSM gameFsm, SplashScreen splashScreen) : base(gameFsm)
        {
            _splashScreen = splashScreen;
        }

        public override UniTask OnRun(CancellationToken cancellationToken = default)
        {
            GameFsm.ChangeState<Level5>();
            return _splashScreen.ShowAsync();
        }
    }
}