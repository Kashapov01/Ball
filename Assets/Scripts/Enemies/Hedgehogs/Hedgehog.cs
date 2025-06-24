using System.Threading;
using Configs;
using Cysharp.Threading.Tasks;
using Interfaces;
using Levels;
using Enemies.Hedgehogs.Movement;
using Signals;
using UnityEngine;
using Zenject;

namespace Enemies.Hedgehogs
{
    public class Hedgehog : MonoBehaviour, IMovableEnemy
    {
        [SerializeField] private MovementType _movementType;

        private IMovement _movement;
        private LevelProvider _levelProvider;
        private CancellationTokenSource _cancellationToken;

        private int _damage;
        private float _speed;
        private float _minHitZoneHedgehog;
        private float _maxHitZoneHedgehog;

        [Inject]
        public void Construct(LevelProvider levelProvider, GameConfig config, SignalBus signalBus)
        {
            _levelProvider = levelProvider;
            var obstacle = config.Physics.obstacle;
            _minHitZoneHedgehog = config.Physics.minHitZoneHedgehog;
            _maxHitZoneHedgehog = config.Physics.maxHitZoneHedgehog;
            _speed = _levelProvider.GetCurrentLevelConfig().Enemies.hedgehog.speed;
            _damage = _levelProvider.GetCurrentLevelConfig().Enemies.hedgehog.damage;
            signalBus.Subscribe<LevelChangedSignal>(LevelChanged);
            _cancellationToken = new CancellationTokenSource();
            
            switch (_movementType)
            {
                case MovementType.OnlyForward:
                    _movement = new ForwardMovement(transform, _speed, obstacle);
                    break;
                case MovementType.OnlyRight:
                    _movement = new RightMovement(transform, _speed, obstacle);
                    break;
            }
        }

        private void LevelChanged()
        {
            _speed = _levelProvider.GetCurrentLevelConfig().Enemies.hedgehog.speed;
            _damage = _levelProvider.GetCurrentLevelConfig().Enemies.hedgehog.damage;
        }

        public void StartMove() => _movement.Move(_cancellationToken.Token);

        public void Interaction(IGameBallProvider ball)
        {
            var hitDirection = (ball.GetBallPosition() - transform.position).normalized;
            var dotProduct = Vector3.Dot(transform.forward, hitDirection);

            if (dotProduct > _maxHitZoneHedgehog || dotProduct < _minHitZoneHedgehog)
            {
                ball.TakeDamage(_damage);
                ball.Reflect(hitDirection);
            }
            else
            {
                Die();
            }
        }

        private void Die()
        {
            gameObject.SetActive(false);
            _cancellationToken.Cancel();
            _cancellationToken.Dispose();
        }
        
    }
}