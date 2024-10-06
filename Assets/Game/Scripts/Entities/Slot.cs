using UnityEngine;

namespace Game.Scripts.Entities
{
    public class Slot : MonoBehaviour
    {
        public Conveyor Conveyor { get; private set; }
        
        public Woo Woo;

        public Slot Left { get; private set; }
        public Slot Right { get; private set; }
        
        public void Initialize(Conveyor conveyor, Slot left, Slot right)
        {
            Conveyor = conveyor;
            Left = left;
            Right = right;
        }

        public void ToggleHighlight(bool toggle)
        {
            if (Woo != null)
            {
                Woo.ToggleHighlight(toggle);
            }
        }
    }
}