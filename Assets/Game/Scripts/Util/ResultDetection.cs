using Game.Scripts.Entities;
using Game.Scripts.Systems;
using UnityEngine;
using VContainer;

namespace Game.Scripts.Util
{
    public class ResultDetection : MonoBehaviour
    {
        [Inject] private LevelResultSystem _levelResultSystem;
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            var woo = other.GetComponentInParent<Woo>();
            if (woo == null)
                return;
            
            
            _levelResultSystem.AddWoo(woo);
        }
    }
}