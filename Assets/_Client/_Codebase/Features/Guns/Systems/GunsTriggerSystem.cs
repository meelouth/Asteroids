using System.Threading.Tasks;

namespace _Client
{
    public class GunsTriggerSystem : IInitSystem, IRunSystem
    {
        private EcsFilter _playerGuns;
        
        public Task Init(EcsSystems systems)
        {
            _playerGuns = systems
                .GetWorld()
                .Filter()
                .With<PlayerInput>().With<Armory>().Build();
            
            return Task.CompletedTask;
        }

        public void Run(EcsSystems systems)
        {
            foreach (var entity in _playerGuns)
            {
                var input = entity.GetComponent<PlayerInput>();

                if (input.IsShootingPrimaryGun)
                {
                    var armory = entity.GetComponent<Armory>();
                    armory.PrimaryGun.AddComponent<Triggered>();
                }

                if (input.IsShootingSecondaryGun)
                {
                    var armory = entity.GetComponent<Armory>();
                    armory.SecondaryGun.AddComponent<Triggered>();
                }
            }
        }
    }
}