using System.Threading;
using Configs;
using Interfaces;
using Cysharp.Threading.Tasks;
using Signals;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace Player
{
    public class BallForMenu : MonoBehaviour, IBallMenuProvider
    {
        private SignalBus _signalBus;
        private MovementController _movementController;
        private CancellationTokenSource _cancellationToken;
        
        private float _duration;
        private float _negativeDirection;
        private float _positiveDirection;

        [Inject]
        private void Construct(GameConfig config, SignalBus signalBus)
        {
            _signalBus = signalBus;
            _duration = config.Settings.timeToMoveInMenu;
            _negativeDirection = config.Settings.negativeDirectionBallForMenu;
            _positiveDirection = config.Settings.positiveDirectionBallForMenu;
            _movementController = new MovementController(transform, config, signalBus);
            _cancellationToken = new CancellationTokenSource();
            BallMovement(_cancellationToken.Token);
            _signalBus.Subscribe<ButtonPlayGameClick>(StopMovement);
        }

        private void StopMovement()
        {
            _cancellationToken.Cancel();
            _signalBus?.Unsubscribe<ButtonPlayGameClick>(StopMovement);
            _cancellationToken?.Dispose();
        }
        
        private async void BallMovement(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                var direction = ReturnDirectionValue();
                _movementController.StartMove(direction, _cancellationToken.Token).Forget();
                await UniTask.WaitForSeconds(_duration);
            }
        }

        private Vector3 ReturnDirectionValue()
        {
            return new Vector3(Random.Range(_negativeDirection, _positiveDirection), 0, Random.Range(_negativeDirection, _positiveDirection)).normalized;
        }

        public Vector3 GetBallPosition()
        {
            return transform.position;
        }
    }
}