using System.Threading.Tasks;
using UnityEngine;

namespace _Client
{
    public class BulletGunSystem : IInitSystem, IRunSystem
    {
        private readonly PoolContainer _bulletPool;
        
        private EcsFilter _triggeredGuns;

        public BulletGunSystem(PoolContainer bulletPool)
        {
            _bulletPool = bulletPool;
        }

        public Task Init(EcsSystems systems)
        {
            _triggeredGuns = systems
                .GetWorld()
                .Filter()
                .With<BulletGun>().With<Triggered>().Build();
            
            return Task.CompletedTask;
        }

        public void Run(EcsSystems systems)
        {
            foreach (var entity in _triggeredGuns)
            {
                var bulletEntity = systems
                    .GetWorld()
                    .CreateEntity();

                var bulletGun = entity.GetComponent<BulletGun>();
                var poolObject = _bulletPool.Get();
                var bulletView = poolObject.GetComponent<BulletView>();

                AsPoolable(bulletEntity, poolObject);
                AsBullet(bulletEntity, bulletView);
                AddCollider(systems.GetWorld(), poolObject.gameObject, bulletEntity);
                AddVelocity(bulletEntity, bulletGun);
                AddLifetime(bulletEntity, bulletGun);
                SetupTransform(bulletEntity, bulletView, bulletGun);
            }
        }

        private static void SetupTransform(Entity bulletEntity, BulletView bulletView, BulletGun bulletGun)
        {
            var transformRef = bulletEntity.AddComponent<TransformRef>();
            transformRef.Ref = bulletView.transform;
            
            var muzzle = bulletGun.Muzzle.transform;
            bulletView.transform.SetPositionAndRotation(muzzle.position, muzzle.rotation);
        }

        private void AsPoolable(Entity bulletEntity, PoolObject poolObject)
        {
            var poolable = bulletEntity.AddComponent<Poolable>();
            poolable.PoolObject = poolObject;
        }

        private void AsBullet(Entity bulletEntity, BulletView bulletView)
        {
            var bullet = bulletEntity.AddComponent<Bullet>();
            bullet.View = bulletView;
        }

        private void AddCollider(EcsWorld world, GameObject view, Entity bulletEntity)
        {
            var colliderView = view.GetComponent<ColliderView>();
            colliderView.Init(bulletEntity, world);
        }

        private void AddLifetime(Entity bulletEntity, BulletGun bulletGun)
        {
            var lifetime = bulletEntity.AddComponent<Lifetime>();
            lifetime.RemainingTime = bulletGun.BulletLifetime;
        }

        private void AddVelocity(Entity bulletEntity, BulletGun bulletGun)
        {
            var velocity = bulletEntity.AddComponent<Velocity>();
            velocity.Amount = bulletGun.BulletSpeed;
        }
    }
}