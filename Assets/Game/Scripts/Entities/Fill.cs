using DG.Tweening;
using UnityEngine;

namespace Game.Scripts.Entities
{
    public class Fill : MonoBehaviour
    {
        [SerializeField] private float MaxScale;
        
        public float SetValue01(float value)
        {
            float scaleY = Mathf.Clamp01(value) * MaxScale;
            transform.DOKill();
            transform.DOScaleY(scaleY, 0.2f).SetEase(Ease.InOutSine).SetId(transform);
            return value;
        }
    }
}