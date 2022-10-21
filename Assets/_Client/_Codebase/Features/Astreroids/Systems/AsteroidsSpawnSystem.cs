using System.Threading.Tasks;
using UnityEngine;

namespace _Client
{
    public class AsteroidsSpawnSystem : IInitSystem, IRunSystem
    {
        private readonly AsteroidsConfiguration _asteroidsConfiguration;
        private readonly AsteroidsFactoryService _factory;
        private readonly SpawnService _spawnService;
        
        private float _leftTimeToSpawnWave;

        public AsteroidsSpawnSystem(AsteroidsConfiguration asteroidsConfiguration, AsteroidsFactoryService factory, SpawnService spawnService)
        {
            _asteroidsConfiguration = asteroidsConfiguration;
            _factory = factory;
            _spawnService = spawnService;
        }

        public async Task Init(EcsSystems systems)
        {
            SpawnWave();
        }

        public void Run(EcsSystems systems)
        {
            if (_leftTimeToSpawnWave <= 0)
                SpawnWave();

            _leftTimeToSpawnWave -= Time.deltaTime;
        }

        private void SpawnWave()
        {
            _leftTimeToSpawnWave = RandomizeWaveTimer();

            var asteroidsCount = RandomizeWaveAsteroidsCount();

            for (var i = 0; i < asteroidsCount; i++)
            {
                SpawnAsteroid();
            }
        }

        private void SpawnAsteroid()
        {
            var spawnResult = _spawnService.GenerateRandomPosition(_asteroidsConfiguration.SpawnDistance);

            var variance = Random.Range(-_asteroidsConfiguration.TrajectoryVariance,
                _asteroidsConfiguration.TrajectoryVariance);

            var angle = Mathf.Atan2(spawnResult.Direction.y, spawnResult.Direction.x) * Mathf.Rad2Deg + 90 + variance;
            var rotation = Quaternion.AngleAxis(angle, Vector3.forward);

            var asteroidEntity = _factory.Create(spawnResult.Position, rotation, RandomizeSpeed());

            var bigAsteroid = asteroidEntity.AddComponent<BigAsteroid>();
            bigAsteroid.ChipsCount = _asteroidsConfiguration.ChipCount;
            bigAsteroid.ChipsScaleModifier = _asteroidsConfiguration.ChipScaleModificator;
            bigAsteroid.ChipsSpeedModifier = _asteroidsConfiguration.ChipSpeedModificator;
        }


        private float RandomizeWaveTimer()
        {
            var waveTime = Random.Range(_asteroidsConfiguration.MinWaveDelay,
                _asteroidsConfiguration.MaxWaveDelay);

            return waveTime;
        }

        private int RandomizeWaveAsteroidsCount()
        {
            var count = Random.Range(_asteroidsConfiguration.MinAsteroidsInWave,
                _asteroidsConfiguration.MaxAsteroidsInWave);

            return count;
        }

        private float RandomizeSpeed()
        {
            var speed = Random.Range(_asteroidsConfiguration.MinSpeed, _asteroidsConfiguration.MaxSpeed);

            return speed;
        }
    }
}