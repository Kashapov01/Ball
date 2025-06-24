using UnityEngine;

namespace UI.BallValue
{
    public class BallModel
    {
        public Vector3 Rotation { get; private set; }
        public float Velocity { get; private set; }

        public void SetRotation(Vector3 rotation)
        {
            Rotation = rotation;
        }

        public void SetVelocity(float velocity)
        {
            Velocity = velocity;
        }
    }
}