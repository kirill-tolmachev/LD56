using System;
using DG.Tweening;
using UnityEngine;

namespace Game.Scripts.Util
{
    public class Pulse : MonoBehaviour
    {
        private void OnEnable()
        {
            transform.DOScale(Vector3.one * 1.2f, 0.2f).SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo).SetId(transform).SetAutoKill(true);
        }
    }
}