using Cysharp.Threading.Tasks;
using Interfaces;
using Signals;
using UnityEngine;
using Zenject;

namespace Particles.CoinParticle
{
    public class CoinParticle : MonoBehaviour, IParticle
    {
        private Transform _coin;
        private SignalBus _signalBus;
        private ParticleSystem _particleSystem;
        
        [Inject]
        private void Construct(SignalBus signalBus)
        {
            _signalBus = signalBus;
            _particleSystem = GetComponent<ParticleSystem>();
        }
        public async void Play(Vector3 position)
        {
            transform.position = position;
            _particleSystem.Play();
            await UniTask.WaitForSeconds((int)_particleSystem.main.duration);
            _signalBus.Fire(new CoinParticleStoppedSignal(this));
        }
    }
}