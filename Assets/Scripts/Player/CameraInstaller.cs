using UnityEngine;
using Zenject;

namespace Player
{
    public class CameraInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<CameraFollow>().AsSingle(); 
            Container.Bind<Camera>().FromComponentInHierarchy().AsSingle();
        }
    }
}