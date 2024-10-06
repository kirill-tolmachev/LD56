using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace Game.Scripts.Entities
{
    public class Conveyor : MonoBehaviour
    {
        [SerializeField] private float _moveInterval = 1.33f;
        [SerializeField] private float _moveSpeed = 2f;
        
        public WooType ExpectedWooType;
        
        private readonly HashSet<Woo> _woos = new();

        private float _lastMoveTime;
        
        private void OnTriggerStay2D(Collider2D other)
        {
            var woo = other.GetComponentInParent<Woo>();
            if (woo == null)
                return;

            _woos.Add(woo);
        }
        
        private void OnTriggerExit2D(Collider2D other)
        {
            var woo = other.GetComponentInParent<Woo>();
            if (woo == null)
                return;
            
            _woos.Remove(woo);
        }

        private void Update()
        {
            if (Time.time - _lastMoveTime < _moveInterval)
                return;
            
            _lastMoveTime = Time.time;
            List<Woo> destroyed = new();
            foreach (var woo in _woos)
            {
                if (!woo)
                {
                    destroyed.Add(woo);
                    continue;
                }
                
                var targetPosition = woo.transform.position + Vector3.right * _moveSpeed;
                woo.transform.DOMoveX(targetPosition.x, 0.2f).SetEase(Ease.InOutBack).SetId(woo.transform).SetAutoKill(true);
            }
            
            foreach (var destroyedWoo in destroyed)
            {
                _woos.Remove(destroyedWoo);
            }
        }
    }
}