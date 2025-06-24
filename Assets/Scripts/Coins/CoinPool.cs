using Zenject;

namespace Coins
{
    public class CoinPool : CustomPool<Coin>
    {
        public CoinPool(IFactory<Coin> factory, int countObjects) : base(factory, countObjects) { }
    }
}
