using System.Threading.Tasks;

namespace _Client
{
    public class DisplayPlayerInformationSystem : IInitSystem, IRunSystem
    {
        private readonly UIService _ui;

        private EcsFilter _players;

        public DisplayPlayerInformationSystem(UIService ui)
        {
            _ui = ui;
            
            _ui.PlayerInformationWidget.Show();
        }

        public Task Init(EcsSystems systems)
        {
            _players = systems
                .GetWorld()
                .Filter()
                .With<Velocity>().With<Wallet>().With<TransformRef>().With<Player>().Build();
            
            return Task.CompletedTask;
        }

        public void Run(EcsSystems systems)
        {
            foreach (var entity in _players)
            {
                var velocity = entity.GetComponent<Velocity>();
                _ui.PlayerInformationWidget.DisplayVelocity(velocity.Amount);

                var wallet = entity.GetComponent<Wallet>();
                _ui.PlayerInformationWidget.DisplayScore(wallet.Value);

                var transform = entity.GetComponent<TransformRef>().Ref;
                _ui.PlayerInformationWidget
                    .DisplayCoordinates(transform.position)
                    .DisplayRotation(transform.eulerAngles.z);
            }
        }
    }
}