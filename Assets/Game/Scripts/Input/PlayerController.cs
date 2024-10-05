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
        
        private WorldBounds _worldBounds;
        private PlayerState _playerState;
        private Camera _mainCamera;
        private GameConfiguration _gameConfiguration;
        private Press _press;
        
        [Inject]
        public void Construct(WorldBounds worldBounds, PlayerState playerState, Camera mainCamera, GameConfiguration gameConfiguration, Press press)
        {
            _worldBounds = worldBounds;
            _playerState = playerState;
            _mainCamera = mainCamera;
            _gameConfiguration = gameConfiguration;
            _press = press;
        }
        
        private void Update()
        {
            var input = Input.GetAxisRaw("Horizontal");
            var direction = Vector3.right * input;
            var nextPosition = _player.position + direction * (_gameConfiguration.PlayerSpeed * Time.deltaTime);
            nextPosition.x = Mathf.Clamp(nextPosition.x, _worldBounds.Left + _playerRadius, _worldBounds.Right - _playerRadius);
            _player.position = nextPosition;
            _playerState.Position = nextPosition;
            
            
            var mousePosition = Input.mousePosition;
            var mouseWorldPosition = _mainCamera.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, _mainCamera.nearClipPlane));
            _playerState.AimPosition = mouseWorldPosition;
            Debug.DrawLine(_player.position, _playerState.AimPosition, Color.red);
            
            if (Input.GetKeyDown(KeyCode.Space))
            {
                _press.DoPress();
            }

        }
    }
}