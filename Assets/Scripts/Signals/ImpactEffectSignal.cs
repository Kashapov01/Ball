using UnityEngine;

namespace Signals
{
    public class ImpactEffectSignal
    {
        public RaycastHit Bounce { get; }

        public ImpactEffectSignal(RaycastHit bounce)
        {
            Bounce = bounce;
        }
    }
}