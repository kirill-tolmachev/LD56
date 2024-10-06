using System.Collections.Generic;
using Game.Scripts.Entities;
using Game.Scripts.UI;
using Game.Scripts.UI.Progress;

namespace Game.Scripts.Systems
{
    public class LevelResultSystem
    {
        private HashSet<Woo> _woos = new();
        
        private readonly LevelState _levelState;
        private readonly LevelUI[] _levelUIs;

        public LevelResultSystem(LevelState levelState, Level1UI levelUI, Level2UI level2UI, Level3UI level3UI, Level4UI level4UI, Level5UI level5UI, Level6UI level6UI)
        {
            _levelState = levelState;
            _levelUIs = new LevelUI[] {levelUI, level2UI, level3UI, level4UI, level5UI, level6UI};
        }

        public void Reset()
        {
            _woos.Clear();
            _levelState.Reset();
            
            foreach (var levelUI in _levelUIs)
            {
                levelUI.StateChanged(_levelState);
            }
        }

        public void AddWoo(Woo woo)
        {
            if (!_woos.Add(woo))
                return;
            
            _levelState.TotalWoos++;

            if (woo.Origin == null)
            {
                StateChanged(_levelState);
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
            
            StateChanged(_levelState);
        }
        
        private void StateChanged(LevelState levelState)
        {
            foreach (var levelUI in _levelUIs)
            {
                levelUI.StateChanged(levelState);
            }
        }
    }
}