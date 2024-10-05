using DG.Tweening;
using UnityEngine;

public class BlinkAnimation : MonoBehaviour
{
    [SerializeField] private float _blinkSpeed;
    [SerializeField] private float _delay;
    
    private Transform _target;
    private Vector3 _initialScale;
    
    private void Awake()
    {
        _target = transform;
        _initialScale = _target.localScale;
    }
    
    void OnEnable()
    {
        DOTween.Sequence()
            .AppendInterval(_delay)
            .Append(_target.DOScaleY(0f, _blinkSpeed).SetEase(Ease.InOutSine))
            .Append(_target.DOScaleY(_initialScale.y, _blinkSpeed).SetEase(Ease.InOutSine))
            .SetLoops(-1)
            .SetId(_target);
    }
    
    void OnDisable()
    {
        DOTween.Kill(_target);
    }
}
