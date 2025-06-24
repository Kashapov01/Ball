using System.Threading;
using Configs;
using Cysharp.Threading.Tasks;
using PhysicsCustom;
using UnityEngine;
using Zenject;

namespace Player
{
    public class MovementController
    {
        private readonly PhysicalBehavior _physicalBehavior;
        public Vector3 Velocity { get; private set; }
        public Vector3 Rotation { get; private set; }
        public MovementController(Transform transform, GameConfig config, SignalBus signalBus)
        {
            _physicalBehavior = new PhysicalBehavior(transform, config, signalBus);
        }
        
        public async UniTask StartMove(Vector3 direction, CancellationToken token)
        {
            _physicalBehavior.Impulse(direction);

            while (_physicalBehavior.Movement && !token.IsCancellationRequested)
            {
                _physicalBehavior.UpdatePhysics();
                Velocity = _physicalBehavior.Velocity;
                Rotation = _physicalBehavior.RotationAngle;
                await UniTask.Yield();
            }
        }

        public async UniTaskVoid Reflect(Vector3 normal, CancellationToken token)
        {
            _physicalBehavior.ReflectVelocity(normal);
            
            while (_physicalBehavior.Movement && !token.IsCancellationRequested)
            {
                _physicalBehavior.UpdatePhysics();
                Velocity = _physicalBehavior.Velocity;
                await UniTask.Yield();
            }
        }
    }
}