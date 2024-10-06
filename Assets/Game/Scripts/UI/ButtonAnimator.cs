using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Game.Scripts.UI
{
    public class ButtonAnimator : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerUpHandler, IPointerDownHandler
    {
        private Button _button;
        private RectTransform _rectTransform;
        private Vector3 _defaultScale;
        private Vector3 _hoverScale;
        private Vector3 _pressedScale;
        private Tween _scaleTween;
        private bool _isPressed = false;
        private bool _isPointerOver = false;

        private void Awake()
        {
            _button = GetComponent<Button>();
            _rectTransform = GetComponent<RectTransform>();
            _defaultScale = Vector3.one;
            _hoverScale = _defaultScale * 1.1f;
            _pressedScale = _defaultScale * 0.9f;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            _isPointerOver = true;
            _scaleTween?.Kill();

            if (_isPressed)
            {
                // If pressed, keep the pressed scale
                _scaleTween = _rectTransform.DOScale(_pressedScale, 0.1f);
            }
            else
            {
                // Scale up when hovered
                _scaleTween = _rectTransform.DOScale(_hoverScale, 0.2f);
            }
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            _isPointerOver = false;
            _scaleTween?.Kill();

            if (_isPressed)
            {
                // If pressed, keep the pressed scale
                _scaleTween = _rectTransform.DOScale(_pressedScale, 0.1f);
            }
            else
            {
                // Scale back to default when pointer exits
                _scaleTween = _rectTransform.DOScale(_defaultScale, 0.2f);
            }
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            _isPressed = true;
            _scaleTween?.Kill();

            // Scale down when pressed
            _scaleTween = _rectTransform.DOScale(_pressedScale, 0.1f);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            _isPressed = false;
            _scaleTween?.Kill();

            if (_isPointerOver)
            {
                // If pointer is still over, scale up to hover scale
                _scaleTween = _rectTransform.DOScale(_hoverScale, 0.2f);
            }
            else
            {
                // Otherwise, scale back to default
                _scaleTween = _rectTransform.DOScale(_defaultScale, 0.2f);
            }
        }
    }
}