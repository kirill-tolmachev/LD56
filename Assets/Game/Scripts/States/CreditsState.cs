using System.Threading;
using Cysharp.Threading.Tasks;
using Game.Scripts.Config;
using Game.Scripts.States.Levels;
using Game.Scripts.UI;

namespace Game.Scripts.States
{
    public class CreditsState : GameState
    {
        private readonly CreditsUI _creditsUI;
        private readonly NarrationUI _narrationUI;
        private readonly Narrators _narrators;

        public class Context
        {
            public bool IsRebelEnding { get; }

            public Context(bool isRebelEnding)
            {
                IsRebelEnding = isRebelEnding;
            }
        }

        public CreditsState(GameFSM gameFsm, CreditsUI creditsUI, NarrationUI narrationUI, Narrators narrators) : base(gameFsm)
        {
            _creditsUI = creditsUI;
            _narrationUI = narrationUI;
            _narrators = narrators;
        }

        public override async UniTask OnRun(CancellationToken cancellationToken = default)
        {
            var ctx = (Context)context;

            if (ctx.IsRebelEnding)
            {
                await RebelEnding();
            }
            else
            {
                await CorpEnding();
            }
            
            await _creditsUI.Show(!ctx.IsRebelEnding);
            GameFsm.ChangeState<Level1>();
        }

        private async UniTask RebelEnding()
        {
            _narrationUI.Show();
            await _narrationUI.ShowText("It worked!", _narrators.Rebel);
            await _narrationUI.ShowText("The machine is finally destroyed!", _narrators.Rebel);
            await _narrationUI.ShowText("No more Woos will be tortured.", _narrators.Rebel);
            await _narrationUI.ShowText("Thank you for everything you've done.", _narrators.Rebel);
            await _narrationUI.ShowText("We'll meet again someday.", _narrators.Rebel);
            await _narrationUI.ShowText("Farewell.", _narrators.Rebel);
            
            await _narrationUI.HideAsync();
        }

        private async UniTask CorpEnding()
        {
            _narrationUI.Show();
            await _narrationUI.ShowText("It worked!", _narrators.Triangle);
            await _narrationUI.ShowText("The rebels are defeated!", _narrators.Triangle);
            await _narrationUI.ShowText("We can now use the Woos to build another machine.", _narrators.Triangle);
            await _narrationUI.ShowText("We'll gather more Woos.", _narrators.Triangle);
            await _narrationUI.ShowText("Forever.", _narrators.Triangle);
            await _narrationUI.ShowText("And when I say 'we'...", _narrators.Triangle);
            await _narrationUI.ShowText("Of course, I don't mean <b>YOU</b>...", _narrators.Triangle);
            await _narrationUI.ShowText("You've seen too much.", _narrators.Triangle);
            await _narrationUI.ShowText("Farewell, <b>WORKER</b>.", _narrators.Triangle);

            await _narrationUI.HideAsync();
        }
    }
}