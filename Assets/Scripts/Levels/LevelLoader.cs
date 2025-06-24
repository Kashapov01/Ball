using Configs;
using Newtonsoft.Json;
using UnityEngine;

namespace Levels
{
    public class LevelLoader
    {
        private readonly string[] _levelConfigs = { 
            "Configs/Levels/Level1Config", 
            "Configs/Levels/Level2Config", 
            "Configs/Levels/Level3Config" 
        };
        
        private int _currentLevelIndex = 0;
        public LevelConfig CurrentLevelConfig { get; private set; }

        public bool TryLoadNextLevelConfig()
        {
            if (_currentLevelIndex >= _levelConfigs.Length)
            {
                return false;
            }

            var json = Resources.Load<TextAsset>(_levelConfigs[_currentLevelIndex]);
            CurrentLevelConfig = JsonConvert.DeserializeObject<LevelConfig>(json.text);
            _currentLevelIndex++;
            
            return true;
        }
    }
}