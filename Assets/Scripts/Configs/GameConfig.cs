using Configs.GameCore;
using Newtonsoft.Json;
using UnityEngine;

namespace Configs
{
    public class GameConfig
    {
        public readonly CameraConfig Camera;
        public readonly PhysicsConfig Physics;
        public readonly CoinsUIConfig CoinsUI;
        public readonly ParticleConfig Particles;
        public readonly WorldSettingsConfig Settings;

        public GameConfig()
        {
            var jsonCamera = Resources.Load<TextAsset>("Configs/GameCore/CameraConfig");
            var jsonPhysics = Resources.Load<TextAsset>("Configs/GameCore/CustomPhysicsConfig");
            var jsonCoinsUI = Resources.Load<TextAsset>("Configs/GameCore/CoinsUIConfig");
            var jsonParticles = Resources.Load<TextAsset>("Configs/GameCore/ParticleConfig");
            var jsonSettings = Resources.Load<TextAsset>("Configs/GameCore/WorldSettingsConfig");

            Camera = JsonConvert.DeserializeObject<CameraConfig>(jsonCamera.text);
            Physics = JsonConvert.DeserializeObject<PhysicsConfig>(jsonPhysics.text);
            CoinsUI = JsonConvert.DeserializeObject<CoinsUIConfig>(jsonCoinsUI.text);
            Particles = JsonConvert.DeserializeObject<ParticleConfig>(jsonParticles.text);
            Settings = JsonConvert.DeserializeObject<WorldSettingsConfig>(jsonSettings.text);
        }
    }
}