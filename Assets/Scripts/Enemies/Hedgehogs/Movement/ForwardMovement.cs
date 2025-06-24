using System.Threading;
using Interfaces;
using UnityEngine;
using Cysharp.Threading.Tasks;

namespace Enemies.Hedgehogs.Movement
{
    public class ForwardMovement : IMovement
    {
        private readonly Transform _transform;
        private readonly string _obstacle;

        private Vector3 _velocity;
        
        private const float DistanceToTurn = 1f;

        public ForwardMovement(Transform transform, float speed, string obstacle)
        {
            _obstacle = obstacle;
            _transform = transform;
            _velocity = Vector3.forward * speed;
        }

        public async UniTask Move(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested && _transform != null)
            {
                _transform.position += _velocity * Time.deltaTime;
                CheckBounce();
                await UniTask.Yield();
            }
        }

        private void CheckBounce()
        {
            if (Physics.Raycast(_transform.position, _velocity.normalized, DistanceToTurn, LayerMask.GetMask(_obstacle)))
                _velocity *= -1;
        }
    }
}