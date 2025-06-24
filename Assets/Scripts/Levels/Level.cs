using System.Collections.Generic;
using System.Linq;
using Interfaces;
using Signals;
using UnityEngine;
using Zenject;

namespace Levels
{
    public class Level : MonoBehaviour
    {
        private ISpawner _spawner;
        private List<IMovableEnemy> _enemies;
        private SignalBus _signalBus;

        private int _countObjOnLevel;
        private int _currentCountCoin;

        [Inject]
        private void Construct(SignalBus signalBus)
        {
            _signalBus = signalBus;
            _signalBus.Subscribe<CoinCollectedSignal>(OnCollectionCoin);
        }

        public void SpawnObjects()
        {
            _spawner = gameObject.GetComponentInChildren<ISpawner>();
            _countObjOnLevel = _spawner.CountObjToSpawn();
            _spawner.StartSpawn();
        }

        public void EnemiesMove()
        {
            _enemies = gameObject.GetComponentsInChildren<IMovableEnemy>().ToList();
            _enemies.ForEach(e => e.StartMove());
        }

        private void OnCollectionCoin(CoinCollectedSignal signal)
        {
            if (!gameObject.activeInHierarchy) return;
            
            _currentCountCoin++;
            if (_currentCountCoin == _countObjOnLevel)
            {
                _signalBus.Fire<AllCoinCollected>();
                _currentCountCoin = 0;
            }
        }
    }
}