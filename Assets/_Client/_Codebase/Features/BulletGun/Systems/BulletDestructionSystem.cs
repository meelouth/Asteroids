﻿using System.Threading.Tasks;

namespace _Client
{
    public class BulletDestructionSystem : IInitSystem, IRunSystem
    {
        private EcsFilter _hitBulletsByDanger;
        
        public async Task Init(EcsSystems systems)
        {
            _hitBulletsByDanger = systems
                .GetWorld()
                .Filter()
                .With<Bullet>().With<HitByDanger>().Build();
        }

        public void Run(EcsSystems systems)
        {
            DestroyHitBulletsByDanger();
        }

        private void DestroyHitBulletsByDanger()
        {
            foreach (var entity in _hitBulletsByDanger)
            {
                DestroyBullet(entity);
            }
        }

        private static void DestroyBullet(Entity entity)
        {
            entity.AddComponent<DestroyCommand>();
        }
    }
}