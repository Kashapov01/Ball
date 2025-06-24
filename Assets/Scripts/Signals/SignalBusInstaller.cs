using Zenject;

namespace Signals
{
    public class SignalBusInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Zenject.SignalBusInstaller.Install(Container);

            Container.DeclareSignal<CoinCollectedSignal>();
            Container.DeclareSignal<AllCoinCollected>();
            Container.DeclareSignal<LevelCompletedSignal>();
            Container.DeclareSignal<LevelChangedSignal>();
            Container.DeclareSignal<ImpactEffectSignal>();
            Container.DeclareSignal<ReboundParticleStoppedSignal>();
            Container.DeclareSignal<CoinParticleStoppedSignal>();
            Container.DeclareSignal<ButtonPlayGameClick>();
        }
    }
    
}
