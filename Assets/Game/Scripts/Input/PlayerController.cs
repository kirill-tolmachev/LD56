using Game.Scripts.Config;
using Game.Scripts.Entities;
using UnityEngine;
using VContainer;

namespace Game.Scripts
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private float _playerRadius;
        [SerializeField] private Transform _player;
        
        private PlayerState _playerState;
        private Camera _mainCamera;
        private GameConfiguration _gameConfiguration;
        
        [Inject]
        public void Construct(PlayerState playerState, Camera mainCamera, GameConfiguration gameConfiguration)
        {
            _playerState = playerState;
            _mainCamera = mainCamera;
            _gameConfiguration = gameConfiguration;
        }
        
        private void Update()
        {
            var input = Input.GetAxisRaw("Horizontal");
            var direction = Vector3.right * input;
            var nextPosition = _player.position + direction * (_gameConfiguration.PlayerSpeed * Time.deltaTime);
            // nextPosition.x = Mathf.Clamp(nextPosition.x, _worldBounds.Left + _playerRadius, _worldBounds.Right - _playerRadius);
            _player.position = nextPosition;
            _playerState.Position = nextPosition;
            
            
            var mousePosition = Input.mousePosition;
            var mouseWorldPosition = _mainCamera.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, _mainCamera.nearClipPlane));
            _playerState.AimPosition = mouseWorldPosition;
            Debug.DrawLine(_player.position, _playerState.AimPosition, Color.red);
        }
    }
}