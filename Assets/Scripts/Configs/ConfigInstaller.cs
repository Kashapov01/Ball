using Zenject;

namespace Configs
{
    public class ConfigInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<GameConfig>().AsSingle();
        }
    }
}