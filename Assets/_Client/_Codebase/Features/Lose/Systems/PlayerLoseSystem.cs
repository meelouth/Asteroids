using System.Threading.Tasks;

namespace _Client
{
    public class PlayerLoseSystem : IInitSystem, IRunSystem
    {
        private readonly UIService _ui;
        private readonly StateMachine _stateMachine;
        
        private EcsFilter _hitPlayerShips;

        public PlayerLoseSystem(UIService ui, StateMachine stateMachine)
        {
            _ui = ui;
            _stateMachine = stateMachine;
        }

        public async Task Init(EcsSystems systems)
        {
            _hitPlayerShips = systems
                .GetWorld()
                .Filter()
                .With<Player>().With<HitByDanger>().Build();
        }

        public void Run(EcsSystems systems)
        {
            foreach (var entity in _hitPlayerShips)
            {
                var loseWorld = new EcsWorld();
                var loseSystems = new EcsSystems(loseWorld);

                loseSystems
                    .Register(new GameRestartSystem());

                var playerWallet = entity.GetComponent<Wallet>();
                
                _ui.LoseScreen.Init(loseWorld);
                _ui.LoseScreen.SetScoreCount(playerWallet.Value);
                _ui.LoseScreen.Show();
                
                var loseState = new PlayerLoseState(loseSystems);
                _stateMachine.SwitchState(loseState);
            }
        }
    }
}