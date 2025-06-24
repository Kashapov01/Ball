using Interfaces;
using UniRx;
using TMPro;
using UnityEngine;
using Zenject;

namespace UI.BallValue
{
    public class SpeedAndRotationView : MonoBehaviour
    {
        [SerializeField] private TMP_Text _labelSpeed;
        [SerializeField] private TMP_Text _labelRotation;

        private IGameBallProvider _ball;
        private BallViewModel _viewModel;

        [Inject]
        private void Construct(BallViewModel viewModel, IGameBallProvider ball)
        {
            _ball = ball;
            _viewModel = viewModel;
            _viewModel.SpeedText.Subscribe(text => _labelSpeed.text = text).AddTo(this);
            _viewModel.RotationText.Subscribe(text => _labelRotation.text = text).AddTo(this);
        }

        private void Update()
        {
            _viewModel.UpdateBallData(_ball.GetBallVelocity().magnitude, _ball.GetBallRotation());
        }
    }
}