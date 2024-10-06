using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Game.Scripts.Config;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Scripts.UI
{
    public class NarrationUI : MonoBehaviour
    {
        [SerializeField] private RectTransform _iconParent;
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private TMP_Text _text;
        [SerializeField] private RectTransform _box;

        private RectTransform _icon;
        private bool _skipRequested = false;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
            {
                _skipRequested = true;
            }
            
        }

        public void Show()
        {
            _canvasGroup.alpha = 1f;
            gameObject.SetActive(true);
        }
        
        public void Hide()
        {
            gameObject.SetActive(false);
        }

        public async UniTask HideAsync()
        {
            await _canvasGroup.DOFade(0f, 0.5f).From(1f).SetEase(Ease.InOutSine).SetAutoKill(true);
            Hide();
        }
        
        public async UniTask ShowText(string text, Narrators.Narrator narrator)
        {
            await Animate(text, narrator);
        }

        private async UniTask Animate(string text, Narrators.Narrator narrator)
        {
            if (narrator.Icon != null)
            {
                _icon = Instantiate(narrator.Icon, _iconParent);
            }
            
            text = $"<color=#{ColorUtility.ToHtmlStringRGBA(narrator.TextColor)}>{text}</color>";
            
            string colorHex = $"#{ColorUtility.ToHtmlStringRGBA(narrator.Color)}";
            _text.text = !string.IsNullOrEmpty(narrator.Name) ? $"<color={colorHex}>{narrator.Name}</color>: " : "";

            _skipRequested = false;

            int i = 0;
            while (i < text.Length)
            {
                if (_skipRequested)
                {
                    // Append the rest of the text immediately
                    _text.text += text.Substring(i);
                    break;
                }

                if (text[i] == '<')
                {
                    // Find the closing '>'
                    int tagEnd = text.IndexOf('>', i);
                    if (tagEnd != -1)
                    {
                        // Append the entire tag at once
                        string tag = text.Substring(i, tagEnd - i + 1);
                        _text.text += tag;
                        i = tagEnd + 1;
                        continue;
                    }
                    else
                    {
                        // Malformed tag; append '<' and continue
                        _text.text += '<';
                        i++;
                    }
                }
                else
                {
                    char currentChar = text[i];
                    _text.text += currentChar;
                    i++;

                    // Skip delay if the character is whitespace
                    if (!char.IsWhiteSpace(currentChar))
                    {
                        await UniTask.Delay(20);
                    }
                }
            }

            _skipRequested = false; // Reset the flag after animation

            var waitForSkip = UniTask.WaitUntil(() => _skipRequested);
            await UniTask.WhenAny(waitForSkip, UniTask.Delay(4000));

            _skipRequested = false;
            
            if (_icon != null)
            {
                Destroy(_icon.gameObject);
            }
        }

        public async UniTask ShakeAsync(float duration = 0.2f)
        {
            await _box.DOShakeAnchorPos(duration, 0.2f).SetEase(Ease.OutBack).SetId(_box).SetAutoKill(true);
        }
    }
}