using Configs;
using Cysharp.Threading.Tasks;
using Signals;
using UnityEngine;
using Zenject;

namespace PhysicsCustom
{
    public class PhysicalBehavior
    {
        private readonly Transform _ball;
        private readonly Transform _mainObj;
        private readonly Renderer _renderer;
        private readonly GameConfig _config;
        private readonly SignalBus _signalBus;
        private readonly CustomPhysics _customPhysics;

        private Vector3 _velocity;
        public Vector3 Velocity => _velocity;
        public Vector3 RotationAngle => _ball.transform.rotation.eulerAngles;
        public bool Movement => _velocity.magnitude > 0.1f;
        
        public PhysicalBehavior(Transform obj, GameConfig config, SignalBus signalBus)
        {
            _mainObj = obj;
            _config = config;
            _signalBus = signalBus;
            _ball = _mainObj.GetChild(0);
            _renderer = _ball.GetComponent<Renderer>();
            _customPhysics = new CustomPhysics(_config);
        }

        public void Impulse(Vector3 direction)
        {
            direction = new Vector3(direction.x, 0, direction.z);
            _velocity = direction * _config.Physics.force;

            if (_velocity.magnitude > _config.Physics.maxSpeed)
            {
                _velocity = _velocity.normalized * _config.Physics.maxSpeed;
            }
        }

        public void UpdatePhysics()
        {
            _velocity += _customPhysics.GravityScale();
            if (_velocity.magnitude < 0.1f) _velocity = Vector3.zero;
            
            if (_customPhysics.IsGrounded(_mainObj.position) && _mainObj != null)
            {
                var bounce = _customPhysics.CheckBounce(_mainObj.position, _velocity);
                if (bounce.collider is not null)
                {
                    _velocity = _customPhysics.ReflectVelocity(_velocity, bounce.normal);
                    _signalBus.Fire(new ImpactEffectSignal(bounce));
                    ActionsDuringRebound(bounce.normal, _config.Physics.impactColor).Forget();
                }
                _velocity.y = 0;
                _velocity *= Mathf.Exp(- _config.Physics.friction * Time.deltaTime);
            }
            _mainObj.position += _velocity * Time.deltaTime;
            _ball.Rotate(_customPhysics.CalculateAngularVelocity(_velocity) * Time.deltaTime, Space.World);
        }

        public void ReflectVelocity(Vector3 normal)
        {
            if (_velocity.magnitude < _config.Physics.reflectImpulse)
            {
                _velocity = normal * _config.Physics.reflectImpulse;
            }
            else
            {
                _velocity = _customPhysics.ReflectVelocity(_velocity, normal);
            }
            
            ActionsDuringRebound(normal, _config.Physics.damageColor).Forget();
        }
        private async UniTask ActionsDuringRebound(Vector3 normal, Color targetColor)
        {
            if (_mainObj == null || _renderer == null) return;
            
            var originalScale = _config.Physics.originalScale;
            var targetScale = _customPhysics.IsTopOrBottomHit(normal)
                ? _config.Physics.verticalScale
                : _config.Physics.horizontalScale;
            
            await ChangeScaleAndColor(originalScale, targetScale, _config.Physics.originalColor,
                targetColor);
            
            await ChangeScaleAndColor(targetScale, originalScale, targetColor,
                _config.Physics.originalColor);
        }

        private async UniTask ChangeScaleAndColor(Vector3 startScale, Vector3 targetScale, Color startColor, Color finalColor)
        {
            var elapsed = 0f;
            while (elapsed < _config.Physics.reboundActionTime)
            {
                if (_mainObj == null || _renderer == null) return;
                
                var t = elapsed / _config.Physics.reboundActionTime;
        
                _mainObj.localScale = Vector3.Lerp(startScale, targetScale, t);
                _renderer.material.color = Color.Lerp(startColor, finalColor, t);
        
                elapsed += Time.deltaTime;
                await UniTask.Yield();
            }

            _mainObj.localScale = targetScale;
            _renderer.material.color = finalColor;
        }
    }
}