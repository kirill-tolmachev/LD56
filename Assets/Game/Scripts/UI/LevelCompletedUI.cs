using System.Linq;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Game.Scripts.Entities;
using Game.Scripts.Util;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Game.Scripts.UI
{
    public class LevelCompletedUI : MonoBehaviour
    {
        [SerializeField] private TMP_Text _scoreText;
        [SerializeField] private Button _continueButton;
     
        private ScaleChild[] _scaleChildren;
        
        private void Awake()
        {
            _scaleChildren = GetComponentsInChildren<ScaleChild>();
        }
        
        public async UniTask Show(LevelState levelState)
        {
            _continueButton.interactable = false;
            
            if (_scaleChildren == null)
                _scaleChildren = GetComponentsInChildren<ScaleChild>();
            
            foreach (var scaleChild in _scaleChildren)
            {
                scaleChild.transform.localScale = Vector3.zero;
            }
            
            gameObject.SetActive(true);
            await transform.DOScale(Vector3.one, 0.2f).From(Vector3.one * 0.8f).SetEase(Ease.InOutSine).SetAutoKill(true);
            
            foreach (var scaleChild in _scaleChildren)
            {
                await scaleChild.transform.DOScale(Vector3.one, 0.5f).From(Vector3.zero).SetEase(Ease.InOutBounce).SetAutoKill(true);
            }
            
            _continueButton.interactable = true;
            
            var completionSource = new UniTaskCompletionSource<bool>();

            void OnContinueButtonClick() => completionSource.TrySetResult(true);

            _continueButton.onClick.AddListener(OnContinueButtonClick);

            await completionSource.Task;
            
            _continueButton.onClick.RemoveListener(OnContinueButtonClick);
            
            foreach (var scaleChild in _scaleChildren.Reverse())
            {
                await scaleChild.transform.DOScale(Vector3.zero, 0.5f).SetEase(Ease.InOutBounce).SetAutoKill(true);
            }
            
            await transform.DOScale(Vector3.one * 0.8f, 0.2f).SetEase(Ease.InOutSine).SetAutoKill(true); 
            
            gameObject.SetActive(false);
        }
    }
}