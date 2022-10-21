using System.Threading.Tasks;

namespace _Client
{
    public class ScoreCountingSystem : IInitSystem, IRunSystem
    {
        private readonly PlayerScoringConfiguration _configuration;
        
        private EcsFilter _destroyedPlayerObjectives;
        private EcsFilter _playersWithWallet;

        public ScoreCountingSystem(PlayerScoringConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task Init(EcsSystems systems)
        {
            _destroyedPlayerObjectives = systems
                .GetWorld()
                .Filter()
                .With<PlayerObjective>().With<DestroyedByPlayer>().With<DestroyCommand>().Build();

            _playersWithWallet = systems
                .GetWorld()
                .Filter()
                .With<Player>().With<Wallet>().Build();
        }

        public void Run(EcsSystems systems)
        {
            foreach (var _ in _destroyedPlayerObjectives)
            {
                foreach (var playerEntity in _playersWithWallet)
                {
                    AddScoreToPlayer(playerEntity);
                }
            }
        }

        private void AddScoreToPlayer(Entity player)
        {
            var wallet = player.GetComponent<Wallet>();

            wallet.Value += _configuration.ScoreByDestroyingObjective;
        }
    }
}