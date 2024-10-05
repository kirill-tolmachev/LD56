using DG.Tweening;
using UnityEngine;

namespace Game.Animations
{
    public class EnemyAnimation : MonoBehaviour
    {
        public Transform[] Bones;

        [SerializeField] private float _maxDistance = 0.5f;
        [SerializeField] private float _animationMinDuration = 2f; // Duration for each movement
        [SerializeField] private float _animationMaxDuration = 2f; // Duration for each movement

        private Vector3[] _initialPositions;

        private Tween[] _boneTweens; // Store the tweens for cleanup

        public void Start()
        {
            // Store the initial positions of the bones
            _initialPositions = new Vector3[Bones.Length];
            _boneTweens = new Tween[Bones.Length]; // Initialize tweens array

            for (int i = 0; i < Bones.Length; i++)
            {
                _initialPositions[i] = Bones[i].position;
                AnimateBone(i, Bones[i], _initialPositions[i]);
            }
        }

        private void AnimateBone(int index, Transform bone, Vector3 initialPosition)
        {
            // Generate a random offset within _maxDistance in the XY plane
            Vector2 randomOffset2D = Random.insideUnitCircle * _maxDistance;
            Vector3 randomOffset = new Vector3(randomOffset2D.x, randomOffset2D.y, 0f);

            // Calculate the target position
            Vector3 targetPosition = initialPosition + randomOffset;

            var duration = Random.Range(_animationMinDuration, _animationMaxDuration);
            // Move the bone to the new position and loop the animation
            _boneTweens[index] = bone.DOMove(targetPosition, duration)
                .SetEase(Ease.InOutSine)
                .OnComplete(() => AnimateBone(index, bone, initialPosition)); // Recursively animate again after completing
        }

        // Ensure that all tweens are killed when the object is destroyed
        private void OnDestroy()
        {
            // Kill each individual tween if it exists
            if (_boneTweens != null)
            {
                foreach (var tween in _boneTweens)
                {
                    tween?.Kill();
                }
            }
        }
    }
}
