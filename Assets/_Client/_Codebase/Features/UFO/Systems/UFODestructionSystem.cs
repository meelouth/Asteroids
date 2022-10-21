using System.Threading.Tasks;

namespace _Client
{
    public class UFODestructionSystem : IInitSystem, IRunSystem
    {
        private EcsFilter _hitUFOs;
        
        public async Task Init(EcsSystems systems)
        {
            _hitUFOs = systems
                .GetWorld()
                .Filter()
                .With<HitByPlayer>().With<UFO>().Build();
        }

        public void Run(EcsSystems systems)
        {
            foreach (var entity in _hitUFOs)
            {
                entity.AddComponent<DestroyCommand>();
                entity.AddComponent<DestroyedByPlayer>();
            }
        }
    }
}