using Game.Scripts.Entities;
using TMPro;
using UnityEngine;

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
    }
}