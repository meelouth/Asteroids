using UnityEngine;

namespace _Client
{
    [System.Serializable]
    public class ShipConfiguration
    {
        [SerializeField] private float accelerationRate;
        [SerializeField] private float decelerationRate;
        [SerializeField] private float maxSpeed;
        [SerializeField] private float rotationSpeed;
        [SerializeField] private GunsConfiguration guns;
        
        public float AccelerationRate => accelerationRate;
        public float DecelerationRate => decelerationRate;
        public float MaxSpeed => maxSpeed;
        public float RotationSpeed => rotationSpeed;
        public GunsConfiguration Guns => guns;
    }
}