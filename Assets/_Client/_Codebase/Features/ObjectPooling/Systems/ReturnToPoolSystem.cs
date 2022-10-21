using System.Threading.Tasks;
using UnityEngine;

namespace _Client
{
    public class ReturnToPoolSystem : IInitSystem, IRunSystem
    {
        private EcsFilter _destroyedPoolObjects;
        
        public async Task Init(EcsSystems systems)
        {
            _destroyedPoolObjects = systems
                .GetWorld()
                .Filter()
                .With<Poolable>().With<DestroyCommand>().Build();
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