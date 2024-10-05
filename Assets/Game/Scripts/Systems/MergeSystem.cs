using Game.Scripts.Entities;

namespace Game.Scripts.Systems
{
    public class MergeSystem
    {
        private readonly WooLifetimeSystem _wooLifetimeSystem;

        public MergeSystem(WooLifetimeSystem wooLifetimeSystem)
        {
            _wooLifetimeSystem = wooLifetimeSystem;
        }
        
        public void Merge(Slot self, Slot other)
        {
            if (self.Woo == null || other.Woo == null)
                return;
            
            _wooLifetimeSystem.Destroy(self.Woo, false);
            _wooLifetimeSystem.Destroy(other.Woo, false);
            
            var mergedType = GetMergedType(self.Woo.Type, other.Woo.Type);
            _wooLifetimeSystem.Create(mergedType, 1, self.transform.position, null, self);
        }
        
        public WooType GetMergedType(WooType type1, WooType type2)
        {
            if (type1 == WooType.Circle && type2 == WooType.Circle)
                return WooType.Circle;
            
            if (type1 == WooType.Square && type2 == WooType.Square)
                return WooType.Square;
            
            return WooType.Circle;
        }
    }
}