using Interfaces;

namespace Signals
{
    public class CoinParticleStoppedSignal
    {
        public IParticle Particle { get; }

        public CoinParticleStoppedSignal(IParticle particle)
        {
            Particle = particle;
        }
    }
}