using UnityEngine;
using Zenject;

namespace Particles.Rebound
{
    public class ReboundParticleFactory : IFactory<ReboundParticle>
    {
        private Transform _parent;
        private DiContainer _container;
        private ReboundParticle _prefab;

        private const string PathPrefabReboundParticle = "Prefabs/Particles/ReboundParticle";
        
        public ReboundParticleFactory(DiContainer container, Transform parent)
        {
            _parent = parent;
            _container = container;
            _prefab = Resources.Load<ReboundParticle>(PathPrefabReboundParticle);
        }
        
        public ReboundParticle Create()
        {
            var particle = _container.InstantiatePrefab(_prefab, _parent).GetComponent<ReboundParticle>();
            return particle;
        }
    }
}