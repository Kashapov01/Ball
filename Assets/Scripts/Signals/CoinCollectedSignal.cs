using Interfaces;

namespace Signals
{
    public class CoinCollectedSignal
    {
        public ICoin CollectedCoin { get; }

        public CoinCollectedSignal(ICoin collectedCoin)
        {
            CollectedCoin = collectedCoin;
        }
    }
}