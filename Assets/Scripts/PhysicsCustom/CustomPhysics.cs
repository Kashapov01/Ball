using Configs;
using UnityEngine;

namespace PhysicsCustom
{
    public class CustomPhysics
    {
        private readonly GameConfig _config;
        public CustomPhysics(GameConfig config)
        {
            _config = config;
        }

        public Vector3 CalculateAngularVelocity(Vector3 velocity)
        {
            return new Vector3(velocity.z, 0, -velocity.x) / _config.Physics.ballRadius *
                   _config.Physics.smoothRotate * Mathf.Rad2Deg;
        }

        public Vector3 GravityScale()
        {
            return _config.Physics.gravityStrength * Vector3.down * Time.deltaTime;
        }

        public bool IsGrounded(Vector3 position)
        {
            return Physics.Raycast(position, Vector3.down, _config.Physics.groundCheckDistance,
                LayerMask.GetMask(_config.Physics.ground));
        }

        public RaycastHit CheckBounce(Vector3 position, Vector3 velocity)
        {
            var obstacle = LayerMask.GetMask(_config.Physics.obstacle);
            var radius = _config.Physics.ballRadius;
            var nextPosition = position + velocity * Time.deltaTime;
            
            Physics.SphereCast(position, radius, velocity.normalized, out RaycastHit hit, (nextPosition - position).magnitude, obstacle);
            return hit;
        }

        public Vector3 ReflectVelocity(Vector3 velocity, Vector3 normal)
        {
            var reflected = Vector3.Reflect(velocity, normal);

            if (reflected.magnitude > _config.Physics.maxSpeed)
            {
                reflected = reflected.normalized * _config.Physics.maxSpeed;
            }
        
            return reflected;
        }
        public bool IsTopOrBottomHit(Vector3 hitNormal)
        {
            return Mathf.Abs(hitNormal.x) > Mathf.Abs(hitNormal.z);
        }
    }
}