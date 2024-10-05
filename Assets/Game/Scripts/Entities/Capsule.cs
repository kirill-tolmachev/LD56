using UnityEngine;

namespace Game.Scripts.Entities
{
    public class Capsule : MonoBehaviour
    {
        [SerializeField] private Fill _fill;
        [SerializeField] private Press _press;
        [SerializeField] private OutTube _outTube;
        
        [Header("Settings")]
        [SerializeField] private float _pressInterval = 10f;
        
        public float PressInterval => _pressInterval;
        
        public Fill Fill => _fill;
        public Press Press => _press;
        public OutTube OutTube => _outTube;
    }
}