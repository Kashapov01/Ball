using UI.BallValue;
using Zenject;

namespace UI
{
    public class BallValueUI : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<BallModel>().AsSingle();
            Container.Bind<BallViewModel>().AsSingle();
            Container.Bind<SpeedAndRotationView>().FromComponentInHierarchy().AsSingle();
        }
    }
}