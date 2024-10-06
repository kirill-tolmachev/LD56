using System.Collections.Generic;
using Game.Scripts.Entities;
using Game.Scripts.UI;

namespace Game.Scripts.Systems
{
    public class LevelResultSystem
    {
        private HashSet<Woo> _woos = new();
        
        private readonly LevelState _levelState;
        private readonly LevelUI _levelUI;

        public LevelResultSystem(LevelState levelState, LevelUI levelUI)
        {
            _levelState = levelState;
            _levelUI = levelUI;
        }

        public void Reset()
        {
            _woos.Clear();
            _levelState.Reset();
            _levelUI.StateChanged(_levelState);
        }

        public void AddWoo(Woo woo)
        {
            if (!_woos.Add(woo))
                return;
            
            _levelState.TotalWoos++;

            if (woo.Origin == null)
            {
                _levelUI.StateChanged(_levelState);
                return;
            }

            if (woo.Type == woo.Origin.ExpectedWooType)
            {
                _levelState.CorrectWoos++;
            }
            else
            {
                _levelState.WrongWoos++;
            }

            if (woo.Type == WooType.Square)
            {
                _levelState.TotalSquares++;
            }
            else if (woo.Type == WooType.Circle)
            {
                _levelState.TotalCircles++;
            }
            else if (woo.Type == WooType.SquareRed)
            {
                _levelState.TotalRedSquares++;
            }
            
            _levelUI.StateChanged(_levelState);
        }
    }
}