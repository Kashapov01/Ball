using UnityEngine;

namespace Interfaces
{
    public interface IGameBallProvider : IBallProvider
    {
        public void Move(Vector3 direction);
        public void TakeDamage(int damage);
        public void Reflect(Vector3 normal);
        public void SetBallPosition(Vector3 position);
        public Vector3 GetBallVelocity();
        public Vector3 GetBallRotation();
    }
}