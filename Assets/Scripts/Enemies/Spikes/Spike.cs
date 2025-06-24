using Interfaces;
using Levels;
using Signals;
using UnityEngine;
using Zenject;

namespace Enemies.Spikes
{
    public class Spike : MonoBehaviour, IEnemy
    {
        private int _damage;
        private LevelProvider _levelProvider;

        [Inject]
        public void Construct(LevelProvider levelProvider, SignalBus signalBus)
        {
            _levelProvider = levelProvider;
            signalBus.Subscribe<LevelChangedSignal>(LevelChanged);
            _damage = _levelProvider.GetCurrentLevelConfig().Enemies.spikes.damage;
        }
        
        private void LevelChanged()
        {
            _damage = _levelProvider.GetCurrentLevelConfig().Enemies.spikes.damage;
        }
        
        public void Interaction(IGameBallProvider ball)
        {
            ball.TakeDamage(_damage);
            ball.Reflect((ball.GetBallPosition() - transform.position).normalized);
        }
    }
}
