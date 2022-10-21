using System.Threading.Tasks;

namespace _Client
{
    public class DestructionSystem : IInitSystem, IRunSystem
    {
        private EcsFilter _destroyed;
        
        public async Task Init(EcsSystems systems)
        {
            _destroyed = systems
                .GetWorld()
                .Filter()
                .With<DestroyCommand>()
                .Build();
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