using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace _Client
{
    public class GunsFactory
    {
        private readonly EcsWorld _world;

        public GunsFactory(EcsWorld world)
        {
            _world = world;
        }

        public async Task<Entity> Create(GunConfiguration configuration, Transform muzzle)
        {
            var entity = _world.CreateEntity();

            return configuration.Type switch
            {
                GunType.BulletGun => await CreateBulletGun((BulletGunConfiguration) configuration, muzzle),
                GunType.LaserGun => await CreateLaserGun((LaserGunConfiguration) configuration, muzzle),
                _ => throw new ArgumentOutOfRangeException(nameof(configuration.Type), configuration.Type, null)
            };
        }

        private async Task<Entity> CreateLaserGun(LaserGunConfiguration laserGunConfiguration, Transform muzzle)
        {
            var laserGunEntity = _world.CreateEntity();
            var laserGun = laserGunEntity.AddComponent<LaserGun>();
            laserGun.Muzzle = muzzle;
            laserGun.MaxCharges = laserGunConfiguration.MaxCharges;
            laserGun.LeftCharges = laserGunConfiguration.MaxCharges;
            laserGun.Width = laserGunConfiguration.Width;
            laserGun.Distance = laserGunConfiguration.Distance;
            laserGun.RemainingTimeToRecharge = laserGunConfiguration.RechargeTime;
            laserGun.CooldownToRecharge = laserGunConfiguration.RechargeTime;
            
            var laserParticle = await Addressables.LoadAssetAsync<GameObject>(laserGunConfiguration.RayViewAssetKey).Task;
            laserGun.Particle = laserParticle.GetComponent<ParticleSystem>();

            laserGunEntity.AddComponent<AttachLaserGunUIViewCommand>().AssetId = laserGunConfiguration.UIViewAssetKey;
            
            return laserGunEntity;
        }

        private async Task<Entity> CreateBulletGun(BulletGunConfiguration bulletGunConfiguration, Transform muzzle)
        {
            var bulletGunEntity = _world.CreateEntity();
            var bulletGun = bulletGunEntity.AddComponent<BulletGun>();
            bulletGun.Muzzle = muzzle;
            bulletGun.BulletSpeed = bulletGunConfiguration.BulletSpeed;
            bulletGun.BulletLifetime = bulletGunConfiguration.BulletLifetime;
            return bulletGunEntity;
        }
    }
}