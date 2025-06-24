using UnityEngine;

namespace Interfaces
{
    public interface ICoin
    {
        public void Collect();
        public int GetReward();
        public Vector3 GetPosition();
    }
}