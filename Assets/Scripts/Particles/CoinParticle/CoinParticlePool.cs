using Zenject;

namespace Particles.CoinParticle
{
    public class CoinParticlePool : CustomPool<CoinParticle>
    {
        public CoinParticlePool(IFactory<CoinParticle> factory, int countObjects) : base(factory, countObjects) { }
    }
}