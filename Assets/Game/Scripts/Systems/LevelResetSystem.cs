namespace Game.Scripts.Systems
{
    public class LevelResetSystem
    {
        private readonly WooLifetimeSystem _wooLifetimeSystem;
        private readonly LevelResultSystem _levelResultSystem;

        public LevelResetSystem(WooLifetimeSystem wooLifetimeSystem, LevelResultSystem levelResultSystem)
        {
            _wooLifetimeSystem = wooLifetimeSystem;
            _levelResultSystem = levelResultSystem;
        }
        
        public void Reset()
        {
            _wooLifetimeSystem.Reset();
            _levelResultSystem.Reset();
        }
    }
}