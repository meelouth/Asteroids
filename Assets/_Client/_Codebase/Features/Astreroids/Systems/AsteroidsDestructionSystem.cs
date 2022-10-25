using System.Threading.Tasks;
using UnityEngine;

namespace _Client
{
    public class AsteroidsDestructionSystem : IInitSystem, IRunSystem
    {
        private readonly AsteroidsFactoryService _factory;
        
        private EcsFilter _bigAsteroidsHitByBullet;
        private EcsFilter _asteroidsChipsHitByBullet;
        private EcsFilter _asteroidsHitByLaser;

        public AsteroidsDestructionSystem(AsteroidsFactoryService factory)
        {
            _factory = factory;
        }

        public Task Init(EcsSystems systems)
        {
            _bigAsteroidsHitByBullet = systems
                .GetWorld()
                .Filter()
                .With<Asteroid>().With<HitByBullet>().With<Velocity>().With<TransformRef>().With<BigAsteroid>().Build();

            _asteroidsChipsHitByBullet = systems
                .GetWorld()
                .Filter()
                .With<Asteroid>().With<HitByBullet>().With<Chip>().Build();

            _asteroidsHitByLaser = systems
                .GetWorld()
                .Filter()
                .With<Asteroid>().With<HitByLaser>().Build();
            
            return Task.CompletedTask;
        }

        public void Run(EcsSystems systems)
        {
            foreach (var entity in _bigAsteroidsHitByBullet)
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
            var bigAsteroid = destroyedAsteroid.GetComponent<BigAsteroid>();
            
            for (var i = 0; i < bigAsteroid.ChipsCount; i++)
            {
                var rotation = RandomizeRotation();

                var asteroidChip = _factory.Create(destroyedAsteroidTransform.position, rotation, destroyedAsteroidVelocity.Amount * bigAsteroid.ChipsSpeedModifier);
                asteroidChip.AddComponent<Chip>();

                var view = asteroidChip.GetComponent<Asteroid>();
                view.View.SetScale(bigAsteroid.ChipsScaleModifier);
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