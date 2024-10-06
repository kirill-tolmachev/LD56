using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace Game.Scripts.Util
{
    public class SplashScreen : MonoBehaviour
    {
        public float Delay = 1;
        public float Duration = 2f;
        [SerializeField] private CanvasGroup _canvasGroup;
        
        public async UniTask ShowAsync()
        {
            _canvasGroup.alpha = 1f;
            _canvasGroup.gameObject.SetActive(true);
            await UniTask.Delay((int)(Delay * 1000f));
            await _canvasGroup.DOFade(0f, Duration).From(1f).SetEase(Ease.InOutSine).SetAutoKill(true);
            _canvasGroup.gameObject.SetActive(false);
        }
    }
}