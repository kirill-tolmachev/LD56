using Game.Scripts.Entities;

namespace Game.Scripts.Systems
{
    public class ScoreSystem
    {
        private readonly LevelState _levelState;
        private readonly Fill _fill;

        public ScoreSystem(LevelState levelState, Fill fill)
        {
            _levelState = levelState;
            _fill = fill;
        }
        
        public void AddScore(Woo woo)
        {
            _levelState.CurrentScore += woo.Size;
            _fill.SetValue01(_levelState.CurrentScore / _levelState.MaxScore);
        }
    }
}