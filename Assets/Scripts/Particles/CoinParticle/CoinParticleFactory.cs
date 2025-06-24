using UnityEngine;
using Zenject;

namespace Particles.CoinParticle
{
    public class CoinParticleFactory : IFactory<CoinParticle>
    {
        private CoinParticle _prefab;
        private Transform _parent;
        private DiContainer _container;

        private const string PathPrefabCoinParticle = "Prefabs/Particles/CoinParticle";

        public CoinParticleFactory(DiContainer container, Transform parent)
        {
            _container = container;
            _parent = parent;
            _prefab = Resources.Load<CoinParticle>(PathPrefabCoinParticle);
        }
        public CoinParticle Create()
        {
            var coinParticle = _container.InstantiatePrefab(_prefab, _parent).GetComponent<CoinParticle>();
            return coinParticle;
        }
    }
}