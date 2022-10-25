using System.Threading.Tasks;
using UnityEngine;

namespace _Client
{
    public class LaserGunSystem : IInitSystem, IRunSystem
    {
        private EcsFilter _triggeredLaserGuns;
        private EcsFilter _laserGuns;
        
        public Task Init(EcsSystems systems)
        {
            _triggeredLaserGuns = systems
                .GetWorld()
                .Filter()
                .With<LaserGun>().With<Triggered>().Build();

            _laserGuns = systems
                .GetWorld()
                .Filter()
                .With<LaserGun>().Build();

            CreateLaserView();
            
            return Task.CompletedTask;
        }

        public void Run(EcsSystems systems)
        {
            Recharge();

            Shoot();
        }

        private void Shoot()
        {
            foreach (var entity in _triggeredLaserGuns)
            {
                var laserGun = entity.GetComponent<LaserGun>();

                if (laserGun.LeftCharges <= 0)
                {
                    continue;
                }

                laserGun.LeftCharges--;

                var center = CreateCenterOfLaserRay(laserGun);
                var width = CreateLaserWidth(laserGun);

                var hits = Physics.BoxCastAll(center, width, laserGun.Muzzle.up, laserGun.Muzzle.rotation, laserGun.Distance);

                KillHitThreats(hits);

                laserGun.Particle.Play();
            }
        }

        private void KillHitThreats(RaycastHit[] hits)
        {
            foreach (var hit in hits)
            {
                if (hit.transform.TryGetComponent<ColliderView>(out var collider))
                {
                    var hitEntity = collider.GetEntity();

                    if (hitEntity.HasComponent<PlayerObjective>())
                    {
                        hitEntity.GetOrAddComponent<HitByLaser>();
                        hitEntity.GetOrAddComponent<HitByPlayer>();
                    }
                }
            }
        }

        private void Recharge()
        {
            foreach (var entity in _laserGuns)
            {
                var laserGun = entity.GetComponent<LaserGun>();

                if (laserGun.LeftCharges < laserGun.MaxCharges)
                {
                    laserGun.RemainingTimeToRecharge -= Time.deltaTime;

                    if (laserGun.RemainingTimeToRecharge <= 0)
                    {
                        laserGun.RemainingTimeToRecharge = laserGun.CooldownToRecharge;
                        laserGun.LeftCharges++;
                    }
                }
            }
        }

        private void CreateLaserView()
        {
            foreach (var entity in _laserGuns)
            {
                var laserGun = entity.GetComponent<LaserGun>();
                var center = CreateCenterOfLaserRay(laserGun);

                var ray = Object.Instantiate(laserGun.Particle, laserGun.Muzzle);
                ray.transform.localPosition = new Vector3(center.x, laserGun.Distance / 2, center.z);
                ray.transform.localScale = new Vector3(laserGun.Width, laserGun.Distance);

                laserGun.Particle = ray;
            }
        }
        
        private Vector3 CreateCenterOfLaserRay(LaserGun laserGun)
        {
            var center = laserGun.Muzzle.position;

            return center;
        }

        private Vector3 CreateLaserWidth(LaserGun laserGun)
        {
            var width = new Vector3(laserGun.Width / 2, 0, 0);

            return width;
        }
    }
}