using UniRx;

namespace UI.Coins
{
    public class CoinsModel
    {
        public ReactiveProperty<int> Coins { get; } = new ReactiveProperty<int>(0);

        public void AddCoins(int amount)
        {
            Coins.Value += amount;
        }

        public void ResetCoins()
        {
            Coins.Value = 0;
        }
    }
    
}
