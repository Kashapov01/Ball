using System.Threading;
using Configs;
using Interfaces;
using Levels;
using Cysharp.Threading.Tasks;
using Signals;
using UnityEngine;
using Zenject;

namespace Player
{
    public class Ball : MonoBehaviour, IGameBallProvider
    {
        private int _maxHealth;
        private int _currentHealth;

        private SignalBus _signalBus;
        private LevelProvider _levelProvider;
        private MovementController _movementController;
        private CancellationTokenSource _cancellationForMove;
        private Vector3 Velocity => _movementController.Velocity;
        private Vector3 Rotation => _movementController.Rotation;

        [Inject]
        private void Construct(GameConfig config, LevelProvider levelProvider, CameraFollow cameraFollow,
            SignalBus signalBus)
        {
            _signalBus = signalBus;
            _levelProvider = levelProvider;
            _maxHealth = _levelProvider.GetCurrentLevelConfig().Ball.health;
            _currentHealth = _maxHealth;
            _movementController = new MovementController(transform, config, signalBus);
            _cancellationForMove = new CancellationTokenSource();
            cameraFollow.StartFollow(this);
            _signalBus.Subscribe<LevelChangedSignal>(LevelChanged);
        }
        
        public void Move(Vector3 direction)
        {
            _cancellationForMove.Cancel();
            _cancellationForMove = new CancellationTokenSource();
            _movementController.StartMove(direction, _cancellationForMove.Token).Forget();
        }

        private void OnTriggerEnter(Collider obj)
        {
            if (obj.TryGetComponent(out ICoin collectable))
            {
                collectable.Collect();
            }
            else if (obj.TryGetComponent(out IEnemy enemy))
            {
                enemy.Interaction(this);
            }
        }

        public void TakeDamage(int damage)
        {
            _currentHealth -= damage;

            if (_currentHealth <= 0)
            {
                Die();
            }
        }
        
        public void Reflect(Vector3 normal)
        {
            _movementController.Reflect(normal, _cancellationForMove.Token).Forget();
        }
        
        private void Die()
        {
            _cancellationForMove.Cancel();
            _signalBus?.Unsubscribe<LevelChangedSignal>(LevelChanged);
            gameObject.SetActive(false);
        }
        
        private void LevelChanged()
        {
            _maxHealth = _levelProvider.GetCurrentLevelConfig().Ball.health;
            _currentHealth = _maxHealth;
        }
        
        private void OnDestroy()
        {
            _cancellationForMove.Cancel();
            _cancellationForMove.Dispose();
        }

        public void SetBallPosition(Vector3 position)
        {
            transform.position = position;
        }

        public Vector3 GetBallPosition()
        {
            return this != null ? transform.position : Vector3.zero;
        }

        public Vector3 GetBallVelocity()
        {
            return this != null ? Velocity : Vector3.zero;
        }

        public Vector3 GetBallRotation()
        {
            return this != null ? Rotation : Vector3.zero;
        }
    }
}