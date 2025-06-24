using Configs;
using Cysharp.Threading.Tasks;
using Signals;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Zenject;

namespace Menu
{
    public class Play : MonoBehaviour
    {
        [SerializeField] private Button _playButton;
        [SerializeField] private Slider _slider;
        
        private string _mainSceneName;
        private float _delayInLoading;
        private float _durationLaunch;

        private SignalBus _signalBus;
        
        [Inject]
        private void Construct(SignalBus signalBus, GameConfig config)
        {
            _signalBus = signalBus;
            _delayInLoading = config.Settings.delayInLoading;
            _mainSceneName = config.Settings.mainSceneName;
            _durationLaunch = config.Settings.durationLaunch;
            _playButton.onClick.AddListener(StartLaunch);
        }

        private async void StartLaunch()
        {
            _playButton.gameObject.SetActive(false);
            _slider.gameObject.SetActive(true);

            var started = false;
            float elapsed = 0;

            while (elapsed <= _durationLaunch)
            {
                if (elapsed >= _durationLaunch / 2 && !started)
                {
                    started = true;
                    await UniTask.WaitForSeconds(_delayInLoading);
                }
                _slider.value = Mathf.Lerp(_slider.minValue, _slider.maxValue, elapsed / _durationLaunch);
                elapsed += Time.deltaTime;
                await UniTask.Yield();
            }
            
            StartGame();
        }

        private void OnDisable()
        {
            _playButton.onClick.RemoveListener(StartGame);
        }

        private void StartGame()
        {
            _signalBus.Fire(new ButtonPlayGameClick());
            SceneManager.LoadScene(_mainSceneName);
        }
    }
}