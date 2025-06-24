using Configs;
using Signals;
using UnityEngine;
using Zenject;

namespace Particles.CoinParticle
{
    public class CoinParticleSpawner : MonoBehaviour
    {
        private int _countParticle;
        private CoinParticlePool _pool;

        [Inject]
        private void Construct(GameConfig config, SignalBus signalBus, DiContainer container)
        {
            _countParticle = config.Particles.coinsParticleCount;
            var factory = new CoinParticleFactory(container, transform);
            _pool = new CoinParticlePool(factory, _countParticle);
            signalBus.Subscribe<CoinCollectedSignal>(ActivateParticle);
            signalBus.Subscribe<CoinParticleStoppedSignal>(ParticleStopped);
        }

        private void ActivateParticle(CoinCollectedSignal signal)
        {
            var particle = _pool.Get();
            particle.Play(signal.CollectedCoin.GetPosition());
        }

        private void ParticleStopped(CoinParticleStoppedSignal signal)
        {
            if (signal.Particle is CoinParticle particle)
            {
                _pool.Release(particle);
            }
        }
    }
}