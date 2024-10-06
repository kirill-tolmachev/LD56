using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using VContainer;

namespace Game.Scripts.Entities
{
    public class Conveyor : MonoBehaviour
    {
        private float MoveInterval => 1.33f * _levelState.ConveyorInterval;
        [SerializeField] private float _moveSpeed = 2f;
        [SerializeField] private Transform[] _gears;
        
        [Inject] private LevelState _levelState;
        
        public WooType ExpectedWooType;
        
        private readonly HashSet<Woo> _woos = new();

        private float _lastMoveTime;
        
        private void OnTriggerStay2D(Collider2D other)
        {
            var woo = other.GetComponentInParent<Woo>();
            if (woo == null)
                return;

            woo.Origin = this;
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
            if (_levelState.IsPaused)
                return;
            
            if (Time.time - _lastMoveTime < MoveInterval)
                return;
            
            float duration = 0.2f;
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
                woo.transform.DOMoveX(targetPosition.x, duration).SetEase(Ease.InOutBack).SetId(woo.transform).SetAutoKill(true);
            }
            
            foreach (var gear in _gears)
            {
                var newRotation = gear.localRotation * Quaternion.Euler(0, 0, -15f * _moveSpeed);
                gear.DOLocalRotateQuaternion(newRotation, duration).SetEase(Ease.InOutBack).SetId(gear).SetAutoKill(true);
            }
            
            foreach (var destroyedWoo in destroyed)
            {
                _woos.Remove(destroyedWoo);
            }
        }
    }
}