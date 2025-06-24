using Interfaces;

namespace Signals
{
    public class ReboundParticleStoppedSignal
    {
        public IParticle Particle { get; }

        public ReboundParticleStoppedSignal(IParticle particle)
        {
            Particle = particle;
        }
    }
}