using System.Threading.Tasks;
using UnityEngine;

namespace _Client
{
    public class ReturnToPoolSystem : IInitSystem, IRunSystem
    {
        private EcsFilter _destroyedPoolObjects;
        
        public Task Init(EcsSystems systems)
        {
            _destroyedPoolObjects = systems
                .GetWorld()
                .Filter()
                .With<Poolable>().With<DestroyCommand>().Build();
            
            return Task.CompletedTask;
        }

        public void Run(EcsSystems systems)
        {
            foreach (var entity in _destroyedPoolObjects)
            {
                var pooled = entity.GetComponent<Poolable>();
                
                pooled.PoolObject.Return();
            }
        }
    }
}