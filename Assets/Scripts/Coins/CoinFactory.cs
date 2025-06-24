using UnityEngine;
using Zenject;

namespace Coins
{
    public class CoinFactory : IFactory<Coin>
    {
        private Coin _prefab;
        private Transform _parent;
        private DiContainer _container;

        private const string PathPrefabCoin = "Prefabs/Coin/CoinPrefab";
        
        public CoinFactory(DiContainer container, Transform parent)
        {
            _parent = parent;
            _container = container;
            _prefab = Resources.Load<Coin>(PathPrefabCoin);
        }

        public Coin Create()
        {
            var coin = _container.InstantiatePrefab(_prefab, _parent).GetComponent<Coin>();
            return coin;
        }
    }
    
}
