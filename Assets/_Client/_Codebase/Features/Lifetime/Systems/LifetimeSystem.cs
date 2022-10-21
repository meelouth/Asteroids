using System.Threading.Tasks;
using UnityEngine;

namespace _Client
{
    public class LifetimeSystem : IInitSystem, IRunSystem
    {
        private EcsFilter _lifetimes;
        
        public async Task Init(EcsSystems systems)
        {
            _lifetimes = systems
                .GetWorld()
                .Filter()
                .With<Lifetime>().Build();
        }

        public void Run(EcsSystems systems)
        {
            foreach (var entity in _lifetimes)
            {
                var lifetime = entity.GetComponent<Lifetime>();

                lifetime.RemainingTime -= Time.deltaTime;
                
                if (lifetime.RemainingTime <= 0)
                    entity.AddComponent<DestroyCommand>();
            }
        }
    }
}