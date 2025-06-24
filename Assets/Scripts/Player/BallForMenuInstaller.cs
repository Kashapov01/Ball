using Interfaces;
using Player;
using Zenject;

namespace Player
{
    public class BallForMenuInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesTo<BallForMenu>().FromComponentInHierarchy().AsSingle();
        }
    }
}