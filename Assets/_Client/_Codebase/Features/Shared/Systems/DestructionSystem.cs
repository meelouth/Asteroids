using System.Threading.Tasks;

namespace _Client
{
    public class DestructionSystem : IInitSystem, IRunSystem
    {
        private EcsFilter _destroyed;
        
        public Task Init(EcsSystems systems)
        {
            _destroyed = systems
                .GetWorld()
                .Filter()
                .With<DestroyCommand>()
                .Build();
            
            return Task.CompletedTask;
        }

        public void Run(EcsSystems systems)
        {
            foreach (var entity in _destroyed)
            {
                systems
                    .GetWorld().DeleteEntity(entity.GetId());
            }
        }
    }
}