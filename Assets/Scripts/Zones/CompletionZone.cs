using Interfaces;
using Signals;
using UnityEngine;
using Zenject;

namespace Zones
{
    public class CompletionZone : MonoBehaviour
    {
        private Renderer _displayZone;
        private BoxCollider _collider;
        private SignalBus _signalBus;

        private void Awake()
        {
            _collider = GetComponent<BoxCollider>();
            _displayZone = gameObject.GetComponentInChildren<Renderer>();
            _collider.enabled = false;
            _displayZone.enabled = false;
        }

        [Inject]
        private void Construct(SignalBus signalBus)
        {
            _signalBus = signalBus;
            _signalBus.Subscribe<AllCoinCollected>(ShowZone);
            _signalBus.Subscribe<LevelChangedSignal>(LevelChanged);
        }

        private void LevelChanged()
        {
            _collider.enabled = false;
            _displayZone.enabled = false;
        }
        
        private void ShowZone()
        {
            _displayZone.enabled = true;
            _collider.enabled = true;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out IBallProvider _))
            {
                _signalBus.Fire<LevelCompletedSignal>();
            }
        }
    }
}