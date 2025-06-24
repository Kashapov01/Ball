using Interfaces;
using Player;
using Zenject;

namespace Player
{
    public class BallInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesTo<Ball>().FromComponentInHierarchy().AsSingle();
        }
    }
    
}
