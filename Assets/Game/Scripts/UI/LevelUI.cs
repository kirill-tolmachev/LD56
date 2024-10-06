using System.Globalization;
using Game.Scripts.Entities;
using TMPro;
using UnityEngine;
using VContainer;

namespace Game.Scripts.UI
{
    public class LevelUI : MonoBehaviour
    {
        [SerializeField] private TMP_Text _totalWoosText;
        [SerializeField] private TMP_Text _totalSquaresText;
        [SerializeField] private TMP_Text _totalCirclesText;
        [SerializeField] private TMP_Text _totalRedSquaresText;
        [SerializeField] private TMP_Text _correctWoosText;
        [SerializeField] private TMP_Text _wrongWoosText;
        
        [SerializeField] private TMP_Text _timerText;
        [SerializeField] private RectTransform _timerParent;
        
        [Inject] private LevelState LevelState;
        
        public void StateChanged(LevelState levelState)
        {
            if (_totalWoosText != null)
            {
                _totalWoosText.text = levelState.TotalWoos.ToString();
            }
            
            if (_totalSquaresText != null)
            {
                _totalSquaresText.text = levelState.TotalSquares.ToString();
            }
            
            if (_totalCirclesText != null)
            {
                _totalCirclesText.text = levelState.TotalCircles.ToString();
            }
            
            if (_totalRedSquaresText != null)
            {
                _totalRedSquaresText.text = levelState.TotalRedSquares.ToString();
            }
            
            if (_correctWoosText != null)
            {
                _correctWoosText.text = levelState.CorrectWoos.ToString();
            }
            
            if (_wrongWoosText != null)
            {
                _wrongWoosText.text = levelState.WrongWoos.ToString();
            }
        }

        private void Update()
        {
            if (_timerText != null)
            {
                _timerText.text = LevelState.CurrentTimer.ToString("0.00");
            }
        }

        public void ToggleTimer(bool value)
        {
            if (_timerParent != null)
            {
                _timerParent.gameObject.SetActive(value);
            }
        }
    }
}