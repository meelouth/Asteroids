using UnityEngine;

namespace _Client
{
    [System.Serializable]
    public class UFOConfiguration
    {
        [SerializeField] private float minWaveDelay;
        [SerializeField] private float maxWaveDelay;

        [SerializeField] private float minSpeed;
        [SerializeField] private float maxSpeed;

        [SerializeField] private int minUFOInWave;
        [SerializeField] private int maxUFOInWave;

        [SerializeField] private float rotationSpeed;

        [SerializeField] private float spawnDistance;

        [SerializeField] private float spawnWaveChance;
        
        public float MinWaveDelay => minWaveDelay;
        public float MaxWaveDelay => maxWaveDelay;

        public float MinSpeed => minSpeed;
        public float MaxSpeed => maxSpeed;

        public int MinUFOInWave => minUFOInWave;
        public int MaxUFOInWave => maxUFOInWave;

        public float RotationSpeed => rotationSpeed;
        public float SpawnDistance => spawnDistance;
        public float SpawnWaveChance => spawnWaveChance;
    }
}