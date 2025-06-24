using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Levels
{
    public class LevelFactory : IFactory<List<Level>>
    {
        private DiContainer _container;
        private List<Level> _levelsPrefabs;
        private List<Level> _levels;

        private const string PathPrefabLevel1 = "Prefabs/Levels/Level1";
        private const string PathPrefabLevel2 = "Prefabs/Levels/Level2";
        private const string PathPrefabLevel3 = "Prefabs/Levels/Level3";
        
        public LevelFactory(DiContainer container)
        {
            _container = container;
            
            _levelsPrefabs = new List<Level>
            {
                Resources.Load<Level>(PathPrefabLevel1),
                Resources.Load<Level>(PathPrefabLevel2),
                Resources.Load<Level>(PathPrefabLevel3)
            };
        }
        public List<Level> Create()
        {
            _levels = new List<Level>
            {
                _container.InstantiatePrefab(_levelsPrefabs[0]).GetComponent<Level>(),
                _container.InstantiatePrefab(_levelsPrefabs[1]).GetComponent<Level>(),
                _container.InstantiatePrefab(_levelsPrefabs[2]).GetComponent<Level>(),
            };
            _levels.ForEach(l => l.gameObject.SetActive(false));
            return _levels;
        }
    }
}