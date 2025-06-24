using Zenject;

namespace Particles.Rebound
{
    public class ReboundParticlePool : CustomPool<ReboundParticle>
    {
        public ReboundParticlePool(IFactory<ReboundParticle> factory, int countObjects) : base(factory, countObjects) { }
    }
}