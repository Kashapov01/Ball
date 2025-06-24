using System.Collections.Generic;
using Interfaces;
using Levels;
using Signals;
using UnityEngine;
using Zenject;

namespace Coins
{
    public class CoinSpawner : MonoBehaviour, ISpawner
    {
        [SerializeField] private List<Transform> _pointPositions;

        private CoinPool _pool;
        private SignalBus _signalBus;
        private LevelProvider _levelProvider;
        private int _countCoin;

        [Inject]
        private void Construct(DiContainer container, SignalBus signalBus, LevelProvider levelProvider)
        {
            _signalBus = signalBus;
            _levelProvider = levelProvider;
            _countCoin = _levelProvider.GetCurrentLevelConfig().Coin.count;
            _signalBus.Subscribe<CoinCollectedSignal>(ReturnToPool);
            _signalBus.Subscribe<LevelChangedSignal>(LevelChanged);

            var factory = new CoinFactory(container, transform);
            _pool = new CoinPool(factory, _countCoin);
        }

        private void LevelChanged()
        {
            _countCoin = _levelProvider.GetCurrentLevelConfig().Coin.count;
        }

        private void ReturnToPool(CoinCollectedSignal signal)
        {
            if (signal.CollectedCoin is Coin coin)
            {
                _pool.Release(coin);
            }
        }

        public void StartSpawn()
        {
            for (int i = 0; i < _countCoin; i++)
            {
                var coin = _pool.Get();
                coin.transform.position = _pointPositions[i].position;
            }
        }

        public int CountObjToSpawn()
        {
            return _countCoin;
        }
    }
}