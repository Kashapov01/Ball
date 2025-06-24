using UnityEngine;

namespace Configs.GameCore
{
    public class PhysicsConfig
    {
        public float gravityStrength;
        public float groundCheckDistance;

        public float force;
        public float reflectImpulse;
        public float maxSpeed;
        public float minDistanceToMove;

        public float friction;
        public float smoothRotate;
        public float reboundActionTime;
        public float ballRadius;
        public float minHitZoneHedgehog;
        public float maxHitZoneHedgehog;

        public string ground;
        public string obstacle;

        public Color originalColor;
        public Color impactColor;
        public Color damageColor;

        public Vector3 originalScale;
        public Vector3 verticalScale;
        public Vector3 horizontalScale;
    }
}