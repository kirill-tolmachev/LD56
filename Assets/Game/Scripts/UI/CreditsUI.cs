using Cysharp.Threading.Tasks;
using DG.Tweening;
using GameAnalyticsSDK;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Scripts.UI
{
    public class CreditsUI : MonoBehaviour
    {
        [SerializeField] private TMP_Text _text;
        [SerializeField] private Image _bg;
        [SerializeField] private Button _playAgainButton;
        [SerializeField] private RectTransform _curtains;
        
        public async UniTask Show(bool curtain)
        {
            _playAgainButton.gameObject.SetActive(false);
            _text.color = curtain ? Color.white : Color.black;
            _bg.color = curtain ? new Color(0,0,0,0) : new Color(125, 143, 149, 255);

            _text.text = "";
            gameObject.SetActive(true);
            
            if (curtain)
            {
                transform.localScale = Vector3.one;
                await CurtainsClose();
            }
            else
            {
                await transform.DOScale(Vector3.one, 0.2f).From(Vector3.one * 0.8f).SetEase(Ease.InOutSine).SetAutoKill(true);    
            }
            

            _text.text = "DE SMÅ";
            await UniTask.Delay(7000);

            _text.text = "Game by dysleixc";
            await UniTask.Delay(7000);

            _text.text = "Made in 48 hours for Ludum Dare 56";
            await UniTask.Delay(7000);
            
            _text.text = "Stop war in Ukraine";
            await UniTask.Delay(7000);
            
            _text.text = "Thank you for playing";
            GameAnalytics.NewProgressionEvent(GAProgressionStatus.Complete, "full_complete_" + (curtain ? "corp" : "rebel"));
            GameAnalytics.NewProgressionEvent(GAProgressionStatus.Complete, "full_complete");
            
            await UniTask.Delay(7000);
            
            _playAgainButton.transform.localScale = Vector3.zero;
            // _playAgainButton.gameObject.SetActive(true);
            _playAgainButton.transform.DOScale(Vector3.one, 0.2f).SetEase(Ease.InOutSine).SetAutoKill(true);
            
            var completionSource = new UniTaskCompletionSource();

            void OnClick() => completionSource.TrySetResult();

            _playAgainButton.onClick.AddListener(OnClick);
            _playAgainButton.interactable = true;
            
            await completionSource.Task;
            
            _playAgainButton.onClick.RemoveListener(OnClick);
            
            _playAgainButton.gameObject.SetActive(false);
            _playAgainButton.interactable = false;
            _curtains.anchorMin = new Vector2(0f, 1f);
            
            gameObject.SetActive(false);
        }

        public async UniTask CurtainsClose()
        {
            await _curtains.DOAnchorMin(Vector2.zero, 1f).SetEase(Ease.OutBounce).SetAutoKill(true);
        }
    }
}