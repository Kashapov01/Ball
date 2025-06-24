using UI.Coins;
using Zenject;

namespace UI
{
    public class CoinsUIInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<CoinsModel>().AsSingle();
            Container.Bind<CoinsViewModel>().AsSingle();
        }
    }
}