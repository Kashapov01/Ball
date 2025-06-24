using Configs;
using Zenject;
using Cysharp.Threading.Tasks;
using Interfaces;
using UnityEngine;

namespace Player
{
    public class CameraFollow
    {
        private Camera _camera;
        private Vector3 _offset;
        private GameConfig _config;
        private IGameBallProvider _ball;

        [Inject]
        private void Construct(Camera camera, GameConfig config)
        {
            _camera = camera;
            _config = config;
        }

        public void StartFollow(IGameBallProvider ball)
        {
            _ball = ball;
            _offset = _camera.transform.position - _ball.GetBallPosition();
            Follow().Forget();
        }
        
        private async UniTaskVoid Follow()
        {
            while (_ball != null && _camera != null)
            {
                var desiredPosition = _ball.GetBallPosition() + _offset;
                var smoothedPosition = Vector3.Lerp(_camera.transform.position, desiredPosition, _config.Camera.smoothSpeed);
                _camera.transform.position = smoothedPosition;

                AdjustCameraFOV();
                await UniTask.NextFrame();
            }
        }

        private void AdjustCameraFOV()
        {
            var targetFOV = Mathf.Lerp(_config.Camera.minFOV, _config.Camera.maxFOV, _ball.GetBallVelocity().magnitude * _config.Camera.speedFactor);
            _camera.fieldOfView = Mathf.Lerp(_camera.fieldOfView, targetFOV, _config.Camera.smoothSpeed * Time.deltaTime);
        }
    }
    
}
