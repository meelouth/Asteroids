using System;
using System.Threading.Tasks;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _Client
{
    public class SpawnUFOSystem : IInitSystem, IRunSystem
    {
        private readonly PoolContainer _ufoPool;
        private readonly UFOConfiguration _ufoConfiguration;
        private readonly SpawnService _spawnService;

        private EcsFilter _players;
        private float _ufoSpawnCooldown;

        public SpawnUFOSystem(PoolContainer ufoPool, UFOConfiguration ufoConfiguration, SpawnService spawnService)
        {
            _ufoPool = ufoPool;
            _ufoConfiguration = ufoConfiguration;
            _spawnService = spawnService;
        }

        public async Task Init(EcsSystems systems)
        {
            _players = systems
                .GetWorld()
                .Filter()
                .With<TransformRef>().With<Player>().Build();
        }

        public void Run(EcsSystems systems)
        {
            _ufoSpawnCooldown -= Time.deltaTime;

            if (_ufoSpawnCooldown <= 0)
            {
                if (IsWaveRolled())
                {
                    SpawnWave(systems);
                }

                _ufoSpawnCooldown = RandomizeUFOsWaveCooldown();
            }
        }

        private void SpawnWave(EcsSystems systems)
        {
            var ufosCount = RandomizeUFOsCount();

            for (var i = 0; i < ufosCount; i++)
            {
                var entity = systems.GetWorld().CreateEntity();
                var poolObject = _ufoPool.Get();

                AsFollower(entity);
                AsPoolable(entity, poolObject);
                AsUFO(entity);
                AddSpeed(entity);
                AddCollider(systems, poolObject.gameObject, entity);
                SetupTransform(entity, poolObject.gameObject);
            }
        }
        
        private bool IsWaveRolled()
        {
            return Random.Range(0, 100) >= _ufoConfiguration.SpawnWaveChance;
        }

        private void SetupTransform(Entity entity, GameObject gameObject)
        {
            var transformRef = entity.AddComponent<TransformRef>();
            var spawnResult = _spawnService.GenerateRandomPosition(_ufoConfiguration.SpawnDistance);
            
            transformRef.Ref = gameObject.transform;
            transformRef.Ref.position = spawnResult.Position;
        }

        private static void AddCollider(EcsSystems systems, GameObject gameObject, Entity entity)
        {
            var colliderView = gameObject.GetComponent<ColliderView>();
            colliderView.Init(entity, systems.GetWorld());
        }

        private void AddSpeed(Entity entity)
        {
            var velocity = entity.AddComponent<Velocity>();
            velocity.Amount = GenerateRandomSpeed();
        }

        private void AsFollower(Entity entity)
        {
            var follower = entity.AddComponent<Following>();
            follower.Target = GetPlayerTransform();
            follower.RotationSpeed = _ufoConfiguration.RotationSpeed;
        }

        private void AsPoolable(Entity entity, PoolObject poolObject)
        {
            var poolable = entity.AddComponent<Poolable>();
            poolable.PoolObject = poolObject;
        }
        
        private void AsUFO(Entity entity)
        {
            entity.AddComponent<PlayerObjective>();
            entity.AddComponent<UFO>();
        }

        private float GenerateRandomSpeed()
        {
            var speed = Random.Range(_ufoConfiguration.MinSpeed, _ufoConfiguration.MaxSpeed);

            return speed;
        }

        private int RandomizeUFOsCount()
        {
            var count = Random.Range(_ufoConfiguration.MinUFOInWave, _ufoConfiguration.MaxUFOInWave);

            return count;
        }

        private float RandomizeUFOsWaveCooldown()
        {
            var cooldown = Random.Range(_ufoConfiguration.MinWaveDelay, _ufoConfiguration.MaxWaveDelay);

            return cooldown;
        }
        
        private Transform GetPlayerTransform()
        {
            foreach (var entity in _players)
            {
                var transform = entity.GetComponent<TransformRef>().Ref;

                return transform;
            }

            return null;
        }
    }
}