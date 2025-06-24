using System.Collections.Generic;
using System.Threading;
using Configs;
using Cysharp.Threading.Tasks;
using Signals;
using TMPro;
using UniRx;
using Zenject;
using UnityEngine;

namespace UI.Coins
{
    public class CoinsView : MonoBehaviour
    {
        [SerializeField] private TMP_Text _label;
        
        private CoinsViewModel _viewModel;
        private readonly CompositeDisposable _disposables = new CompositeDisposable();

        private int _lastCoins;
        private bool _isAnimating;
        
        private GameConfig _config;
        private Queue<int> _rewardQueue = new Queue<int>();
        private CancellationTokenSource _cancellationToken;
        
        [Inject]
        private void Construct(CoinsViewModel viewModel, GameConfig config, SignalBus signalBus)
        {
            _config = config;
            _viewModel = viewModel;
            _cancellationToken = new CancellationTokenSource();
            _viewModel.Coins.Subscribe(UpdateCoinsText).AddTo(_disposables);
            signalBus.Subscribe<LevelCompletedSignal>(Reset);
        }


        private void UpdateCoinsText(int newCoins)
        {
            var reward = newCoins - _lastCoins;
            
            if (reward > 0) 
            {
                _rewardQueue.Enqueue(reward);
                _lastCoins = newCoins; 

                if (!_isAnimating)
                {
                    ProcessQueue(_cancellationToken.Token).Forget();
                }
            }
        }

        private async UniTask ProcessQueue(CancellationToken token)
        {
            _isAnimating = true;
            
            while (_rewardQueue.Count > 0 && !token.IsCancellationRequested)
            {
                var reward = _rewardQueue.Dequeue();
                await SmoothCalculateReward(reward, token);
            }
            
            _isAnimating = false;
        }

        private async UniTask SmoothCalculateReward(int reward, CancellationToken token)
        {
            var startValue = int.Parse(_label.text);
            var endValue = int.Parse(_label.text) + reward;
            var fontSize = _label.fontSize;
            
            var elapsed = 0f;

            while (elapsed <= _config.CoinsUI.timeSmoothCalculate && !token.IsCancellationRequested)
            {
                var coinsCount = Mathf.Lerp(startValue, endValue, elapsed /  _config.CoinsUI.timeSmoothCalculate);
                _label.text = Mathf.RoundToInt(coinsCount).ToString();
                _label.fontSize = fontSize +  _config.CoinsUI.bonusFontSize;
                elapsed += Time.deltaTime;
                await UniTask.Yield();
            }

            _label.fontSize = fontSize;
        }
        
        private void OnDestroy()
        {
            _disposables.Dispose();
        }
        
        private void Reset()
        {
            _lastCoins = 0; 
            _label.text = "0"; 
            _rewardQueue.Clear(); 
            _cancellationToken.Cancel(); 
            _cancellationToken.Dispose(); 
            _cancellationToken = new CancellationTokenSource();
        }
    }
}