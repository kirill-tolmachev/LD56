using Game.Scripts.Config;
using Game.Scripts.Entities;

namespace Game.Scripts.Systems
{
    public class ScoreSystem
    {
        private readonly LevelState _levelState;
        private readonly GameConfiguration _gameConfiguration;

        public ScoreSystem(LevelState levelState, GameConfiguration gameConfiguration)
        {
            _levelState = levelState;
            _gameConfiguration = gameConfiguration;
        }
        
        public void AddScore(Capsule capsule, Woo woo)
        {
            _levelState.CurrentScore += woo.Size * _gameConfiguration.WooSizeScoreMultiplier;
            capsule.Fill.SetValue01(_levelState.CurrentScore / _levelState.MaxScore);
        }
    }
}