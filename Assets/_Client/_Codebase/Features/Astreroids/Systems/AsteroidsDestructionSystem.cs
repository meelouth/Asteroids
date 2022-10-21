using System.Threading.Tasks;
using UnityEngine;

namespace _Client
{
    public class AsteroidsDestructionSystem : IInitSystem, IRunSystem
    {
        private readonly AsteroidsConfiguration _asteroidsConfiguration;
        private readonly AsteroidsFactoryService _factory;
        
        private EcsFilter _asteroidsHitByBullet;
        private EcsFilter _asteroidsChipsHitByBullet;
        private EcsFilter _asteroidsHitByLaser;

        public AsteroidsDestructionSystem(AsteroidsConfiguration asteroidsConfiguration, AsteroidsFactoryService factory)
        {
            _asteroidsConfiguration = asteroidsConfiguration;
            _factory = factory;
        }

        public async Task Init(EcsSystems systems)
        {
            _asteroidsHitByBullet = systems
                .GetWorld()
                .Filter()
                .With<Asteroid>().With<HitByBullet>().With<Velocity>().With<TransformRef>().Except<Chip>().Build();

            _asteroidsChipsHitByBullet = systems
                .GetWorld()
                .Filter()
                .With<Asteroid>().With<HitByBullet>().With<Chip>().Build();

            _asteroidsHitByLaser = systems
                .GetWorld()
                .Filter()
                .With<Asteroid>().With<HitByLaser>().Build();
        }

        public void Run(EcsSystems systems)
        {
            foreach (var entity in _asteroidsHitByBullet)
            {
                CreateAsteroidChips(entity);
                DestroyAsteroid(entity);
            }

            foreach (var entity in _asteroidsChipsHitByBullet)
            {
                DestroyAsteroid(entity);
            }

            foreach (var entity in _asteroidsHitByLaser)
            {
                DestroyAsteroid(entity);
            }
        }

        private void CreateAsteroidChips(Entity destroyedAsteroid)
        {
            var destroyedAsteroidTransform = destroyedAsteroid.GetComponent<TransformRef>().Ref;
            var destroyedAsteroidVelocity = destroyedAsteroid.GetComponent<Velocity>();
            
            for (var i = 0; i < _asteroidsConfiguration.ChipCount; i++)
            {
                var rotation = RandomizeRotation();

                var asteroidChip = _factory.Create(destroyedAsteroidTransform.position, rotation, destroyedAsteroidVelocity.Amount * _asteroidsConfiguration.ChipSpeedModificator);
                asteroidChip.AddComponent<Chip>();

                var view = asteroidChip.GetComponent<Asteroid>();
                view.View.SetScale(_asteroidsConfiguration.ChipScaleModificator);
            }
        }

        private Quaternion RandomizeRotation()
        {
            return Quaternion.Euler(0, 0, Random.Range(0, 360));
        }

        private void DestroyAsteroid(Entity entity)
        {
            entity.AddComponent<DestroyCommand>();
            entity.AddComponent<DestroyedByPlayer>();
        }
    }
}