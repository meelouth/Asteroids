using System.Threading.Tasks;

namespace _Client
{
    public class PlayerLoseState : IState
    {
        private EcsSystems _systems;

        public PlayerLoseState(EcsSystems systems)
        {
            _systems = systems;
        }

        public async Task Enter()
        {
            await _systems.Init();
        }

        public void Run()
        {
            _systems?.Run();
        }

        public void Exit()
        {
            _systems?.Destroy();
            _systems = null;
        }
    }
}