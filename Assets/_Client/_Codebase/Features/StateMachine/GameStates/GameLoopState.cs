using System.Threading.Tasks;

namespace _Client
{
    public class GameLoopState : IState
    {
        private EcsSystems _systems;

        public GameLoopState(EcsSystems systems)
        {
            _systems = systems;
        }

        public async Task Enter()
        {
            
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