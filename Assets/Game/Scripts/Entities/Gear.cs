using UnityEngine;
using VContainer;

namespace Game.Scripts.Entities
{
    public class Gear : MonoBehaviour
    {
        public bool IsRotating;
        public float RotationSpeed = 100f;
        
        [Inject] private LevelState _levelState;
        
        private void Update()
        {
            IsRotating = !_levelState.IsPaused;
            
            if (!IsRotating)
                return;
            
            transform.Rotate(Vector3.forward * RotationSpeed * Time.deltaTime);
        }
    }
}