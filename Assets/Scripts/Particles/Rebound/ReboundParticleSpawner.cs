using Configs;
using Signals;
using UnityEngine;
using Zenject;

namespace Particles.Rebound
{
    public class ReboundParticleSpawner : MonoBehaviour
    {
        private int _countParticle;
        private Transform _ballTransform;
        private ReboundParticlePool _pool;
        
        [Inject]
        private void Construct(GameConfig config, SignalBus signalBus, DiContainer container)
        {
            _countParticle = config.Particles.reboundParticleCount;
            var factory = new ReboundParticleFactory(container, transform);
            _pool = new ReboundParticlePool(factory, _countParticle);
            signalBus.Subscribe<ImpactEffectSignal>(ActivateParticle);
            signalBus.Subscribe<ReboundParticleStoppedSignal>(ParticleStopped);
        }

        private void ActivateParticle(ImpactEffectSignal signal)
        {
            var particle = _pool.Get();
            particle.Play(signal.Bounce.point);
        }

        private void ParticleStopped(ReboundParticleStoppedSignal signal)
        {
            if (signal.Particle is ReboundParticle particle)
            {
                _pool.Release(particle);
            }
        }
    }
}