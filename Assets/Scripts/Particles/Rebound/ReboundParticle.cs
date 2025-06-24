using Interfaces;
using Cysharp.Threading.Tasks;
using Signals;
using UnityEngine;
using Zenject;

namespace Particles.Rebound
{
    public class ReboundParticle : MonoBehaviour, IParticle
    {
        private IBallProvider _ball;
        private SignalBus _signalBus;
        private ParticleSystem _particleSystem;

        [Inject]
        private void Construct(SignalBus signalBus, IBallProvider ball)
        {
            _ball = ball;
            _signalBus = signalBus;
            _particleSystem = GetComponent<ParticleSystem>();
        }
        public async void Play(Vector3 position)
        {
            transform.position = position;
            transform.LookAt(_ball.GetBallPosition());
            _particleSystem.Play();
            await UniTask.WaitForSeconds((int)_particleSystem.main.duration);
            _signalBus.Fire(new ReboundParticleStoppedSignal(this));
        }
    }
}