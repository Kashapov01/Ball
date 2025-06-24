using System.Threading;
using Interfaces;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Enemies.Hedgehogs.Movement
{
    public class RightMovement : IMovement
    {
        private readonly Transform _transform;
        private readonly string _obstacle;

        private Vector3 _velocity;
        
        private const float DistanceToTurn = 1f;

        public RightMovement(Transform transform, float speed, string obstacle)
        {
            _obstacle = obstacle;
            _transform = transform;
            _velocity = Vector3.right * speed;
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