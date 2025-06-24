using Signals;
using UniRx;
using Zenject;

namespace UI.Coins
{
    public class CoinsViewModel
    {
        private readonly CoinsModel _coinsModel;
        
        public IReadOnlyReactiveProperty<int> Coins => _coinsModel.Coins;
        
        [Inject]
        public CoinsViewModel(CoinsModel coinsModel, SignalBus signalBus)
        {
            _coinsModel = coinsModel;

            signalBus.Subscribe<CoinCollectedSignal>(signal => _coinsModel.AddCoins(signal.CollectedCoin.GetReward()));
            signalBus.Subscribe<LevelCompletedSignal>(_coinsModel.ResetCoins);
        }
    }
}