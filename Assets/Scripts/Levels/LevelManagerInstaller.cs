using Zenject;

namespace Levels
{
    public class LevelManagerInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<LevelProvider>().AsSingle().NonLazy();
        }
    }
}