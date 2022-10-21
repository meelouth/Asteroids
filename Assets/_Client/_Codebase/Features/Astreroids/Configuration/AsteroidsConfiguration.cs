using UnityEngine;

namespace _Client
{
    [System.Serializable]
    public class AsteroidsConfiguration
    {
        [SerializeField] private float minWaveDelay;
        [SerializeField] private float maxWaveDelay;

        [SerializeField] private float minSpeed;
        [SerializeField] private float maxSpeed;

        [SerializeField] private int minAsteroidsInWave;
        [SerializeField] private int maxAsteroidsInWave;

        [SerializeField] private float spawnDistance;
        [SerializeField] private float trajectoryVariance;

        [SerializeField] private float lifetime;

        [SerializeField] private int chipCount;
        [SerializeField] private float chipSpeedModificator;
        [SerializeField] private float chipScaleModificator;

        public float MinWaveDelay => minWaveDelay;
        public float MaxWaveDelay => maxWaveDelay;

        public float MinSpeed => minSpeed;
        public float MaxSpeed => maxSpeed;

        public int MinAsteroidsInWave => minAsteroidsInWave;
        public int MaxAsteroidsInWave => maxAsteroidsInWave;
        public float Lifetime => lifetime;
        
        public float TrajectoryVariance => trajectoryVariance;
        public float SpawnDistance => spawnDistance;

        public float ChipScaleModificator => chipScaleModificator;
        public int ChipCount => chipCount;
        public float ChipSpeedModificator => chipSpeedModificator;
    }
}