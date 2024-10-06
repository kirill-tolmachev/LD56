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
        
        public Slot[] Slots;
        public Gear[] Gears;

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
        
        private void Start()
        {
            for (int i = 0; i < Slots.Length; i++)
            {
                Slots[i].Initialize(this, i == 0 ? null : Slots[i - 1], i == Slots.Length - 1 ? null : Slots[i + 1]);
            }
        }

        private void Update()
        {
            if (Time.time - _lastMoveTime < _moveInterval)
                return;
            
            _lastMoveTime = Time.time;
            foreach (var woo in _woos)
            {
                var targetPosition = woo.transform.position + Vector3.right * _moveSpeed;
                woo.transform.DOMoveX(targetPosition.x, 0.2f).SetEase(Ease.InOutBack).SetId(woo.transform);
            }
        }

        public async UniTask MoveSlotsAfterMerge(Slot self, Slot other)
        {
            var selfIndex = Array.IndexOf(Slots, self);
            var otherIndex = Array.IndexOf(Slots, other);
            
            var maxIndex = Math.Max(selfIndex, otherIndex);

            foreach (var gear in Gears)
            {
                gear.IsRotating = true;
            }
            
            var tasks = new List<UniTask>();
            for (int i = maxIndex + 1; i < Slots.Length; i++)
            {
                var slot = Slots[i];
                
                if (slot.Woo != null)
                {
                    Debug.Log("MoveSlotsAfterMerge: moving " + slot.Woo.Slot.Conveyor.gameObject.name + " to " + Slots[i - 1].Conveyor.gameObject.name + " at " + (i - 1));
                    var task = slot.Woo.MoveToSlot(Slots[i - 1]);
                    tasks.Add(task);
                }
            }
            
            await UniTask.WhenAll(tasks);
            
            foreach (var gear in Gears)
            {
                gear.IsRotating = false;
            }
        }
    }
}