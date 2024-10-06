using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Game.Scripts.UI
{
    public class LevelFailedUI : MonoBehaviour
    {
        [SerializeField] private Button _restartLevelButton;
        [SerializeField] private Button _exitLevelButton;
        
        public async UniTask<bool> Show()
        {
            gameObject.SetActive(true);
            _restartLevelButton.interactable = true;
            
            var completionSource = new UniTaskCompletionSource<bool>();

            void OnRestartButtonClick() => completionSource.TrySetResult(true);
            void OnCancelButtonClick() => completionSource.TrySetResult(false);

            _restartLevelButton.onClick.AddListener(OnRestartButtonClick);
            _exitLevelButton.onClick.AddListener(OnCancelButtonClick);

            bool result = await completionSource.Task;
            
            _restartLevelButton.onClick.RemoveListener(OnRestartButtonClick);
            _exitLevelButton.onClick.RemoveListener(OnCancelButtonClick);
            gameObject.SetActive(false);
            
            return result;
        }
    }
}