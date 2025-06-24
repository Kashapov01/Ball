using System.Collections.Generic;
using Configs;
using Interfaces;
using Signals;
using UnityEngine;
using Zenject;
using Zones;

namespace Levels
{
    public class LevelProvider
    {
        private List<Level> _levels;
        private LevelLoader _config;
        private SignalBus _signalBus;
        private IGameBallProvider _ball;

        private Level _currentLevel;
        private int _currentLevelIndex;

        [Inject]
        private void Construct(SignalBus signalBus, IGameBallProvider ball, DiContainer container)
        {
            _ball = ball;
            _signalBus = signalBus;
            _signalBus.Subscribe<LevelCompletedSignal>(LoadNextLevel);
            _config = new LevelLoader();
            _config.TryLoadNextLevelConfig();
            var factory = new LevelFactory(container);
            _levels = factory.Create();

            _currentLevel = _levels[_currentLevelIndex];
            _currentLevel.gameObject.SetActive(true);
            _currentLevel.SpawnObjects();
            _currentLevel.EnemiesMove();
            var startZone = _currentLevel.GetComponentInChildren<StartZone>();
            if (startZone != null)
            {
                _ball.SetBallPosition(startZone.transform.position);
            }
        }
        

        public LevelConfig GetCurrentLevelConfig()
        {
            return _config.CurrentLevelConfig;
        }

        private void LoadNextLevel()
        {
            _config.TryLoadNextLevelConfig();

            if (_currentLevel != null)
            {
                _currentLevel.gameObject.SetActive(false);
            }

            if (_currentLevelIndex < _levels.Count - 1)
            {
                _currentLevelIndex++;
                _currentLevel = _levels[_currentLevelIndex];
                _currentLevel.gameObject.SetActive(true);
                _signalBus.Fire(new LevelChangedSignal());

                var startZone = _currentLevel.GetComponentInChildren<StartZone>();
                if (startZone != null)
                {
                    _ball.SetBallPosition(startZone.transform.position);
                }

                _currentLevel.SpawnObjects();
                _currentLevel.EnemiesMove();
            }
            else
            {
                Debug.Log("Все уровни пройдены!");
            }
        }
    }
}