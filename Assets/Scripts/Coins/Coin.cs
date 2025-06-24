using System.Threading;
using Interfaces;
using Levels;
using Cysharp.Threading.Tasks;
using Signals;
using Zenject;
using UnityEngine;

namespace Coins
{
    public class Coin : MonoBehaviour, ICoin
    {
        private int _reward;
        private int _speedRotation;
        private SignalBus _signalBus;
        private LevelProvider _levelProvider;
        private CancellationTokenSource _cancellationToken;


        [Inject]
        private void Construct(SignalBus signalBus, LevelProvider levelProvider)
        {
            _signalBus = signalBus;
            _levelProvider = levelProvider;
            _speedRotation = _levelProvider.GetCurrentLevelConfig().Coin.speedRotation;
            _signalBus.Subscribe<LevelChangedSignal>(LevelChanged);
            _reward = _levelProvider.GetCurrentLevelConfig().Coin.reward;
        }

        private void OnEnable()
        {
            _cancellationToken = new CancellationTokenSource();
            Rotation(_cancellationToken.Token);
        }

        private void OnDisable()
        {
            _cancellationToken.Cancel();
            _cancellationToken.Dispose();
        }

        private async void Rotation(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                transform.Rotate(Vector3.up * _speedRotation * Time.deltaTime);
                await UniTask.Yield();
            }
        }

        private void LevelChanged(LevelChangedSignal signal)
        {
            _reward = _levelProvider.GetCurrentLevelConfig().Coin.reward;
        }

        public void Collect()
        {
            _signalBus.Fire(new CoinCollectedSignal(this));
        }

        public int GetReward()
        {
            return _reward;
        }

        public Vector3 GetPosition()
        {
            return transform.position;
        }
    }
}