using UniRx;
using UnityEngine;

namespace UI.BallValue
{
    public class BallViewModel
    {
        private readonly BallModel _ballModel;
        public ReactiveProperty<string> SpeedText { get; private set; }
        public ReactiveProperty<string> RotationText { get; private set; }

        public BallViewModel(BallModel ballModel)
        {
            _ballModel = ballModel;
            SpeedText = new ReactiveProperty<string>();
            RotationText = new ReactiveProperty<string>();
            
            UpdateText();
        }

        public void UpdateBallData(float velocity, Vector3 rotation)
        {
            _ballModel.SetVelocity(velocity);
            _ballModel.SetRotation(rotation);

            UpdateText();
        }

        private void UpdateText()
        {
            SpeedText.Value = _ballModel.Velocity.ToString("F0");
            RotationText.Value = $"X:{_ballModel.Rotation.x:F0} Y:{_ballModel.Rotation.y:F0} Z:{_ballModel.Rotation.z:F0}";
        }
    }
}