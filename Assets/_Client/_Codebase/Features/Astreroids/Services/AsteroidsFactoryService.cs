using UnityEngine;

namespace _Client
{
    public class AsteroidsFactoryService
    {
        private readonly PoolContainer _pool;
        private readonly AsteroidsConfiguration _asteroidsConfiguration;

        private readonly EcsWorld _world;

        public AsteroidsFactoryService(PoolContainer pool, AsteroidsConfiguration asteroidsConfiguration, EcsWorld world)
        {
            _pool = pool;
            _asteroidsConfiguration = asteroidsConfiguration;
            _world = world;
        }

        public Entity Create(Vector3 position, Quaternion rotation, float speed)
        {
            var entity = _world.CreateEntity();
            
            var poolObject = _pool.Get();
            var gameObject = poolObject.gameObject;
            
            AsAsteroid(entity, gameObject);
            AsPoolable(entity, poolObject);
            AddLifetime(entity);
            AsPlayerObjective(entity);
            AddVelocity(entity, speed);
            AsCollider(gameObject, entity);
            SetupTransform(entity, position, rotation, gameObject.transform);

            return entity;
        }

        private void AsAsteroid(Entity entity, GameObject poolObject)
        {
            var asteroid = entity.AddComponent<Asteroid>();
            var view = poolObject.GetComponent<AsteroidView>();
            
            view.SetModelRotation(RandomizeZRotation());
            
            asteroid.View = view;
        }

        private Quaternion RandomizeZRotation()
        {
            return Quaternion.Euler(new Vector3(0, 0, Random.Range(0, 360)));
        }

        private void SetupTransform(Entity entity, Vector3 position, Quaternion rotation, Transform transform)
        {
            var transformRef = entity.AddComponent<TransformRef>();

            transformRef.Ref = transform;

            transform.SetPositionAndRotation(position, rotation);
        }

        private void AsCollider(GameObject poolObject, Entity entity)
        {
            var colliderView = poolObject.GetComponent<ColliderView>();
            colliderView.Init(entity, _world);
        }

        private void AsPoolable(Entity entity, PoolObject poolObject)
        {
            var pooled = entity.AddComponent<Poolable>();
            pooled.PoolObject = poolObject;
        }

        private void AddVelocity(Entity entity, float speed)
        {
            var velocity = entity.AddComponent<Velocity>();
            velocity.Amount = speed;
        }

        private void AsPlayerObjective(Entity entity)
        {
            entity.AddComponent<PlayerObjective>();
        }

        private void AddLifetime(Entity entity)
        {
            var lifetime = entity.AddComponent<Lifetime>();
            lifetime.RemainingTime = _asteroidsConfiguration.Lifetime;
        }
    }
}