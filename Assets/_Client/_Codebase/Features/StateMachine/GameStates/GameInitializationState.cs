using System.Threading.Tasks;

namespace _Client
{
    public class GameInitializationState : IState
    {
        private readonly StateMachine _stateMachine;
        private readonly EcsSystems _systems;
        private readonly UIService _ui;
        private readonly PoolContainer[] _pools;
        private readonly EnvironmentSceneProvider _environmentSceneProvider;

        public GameInitializationState(StateMachine stateMachine, EcsSystems systems, UIService ui, 
            EnvironmentSceneProvider environmentSceneProvider, params PoolContainer[] pools)
        {
            _stateMachine = stateMachine;
            _systems = systems;
            _ui = ui;
            _pools = pools;
            _environmentSceneProvider = environmentSceneProvider;
        }

        public async Task Enter()
        {
            _ui.LoadingScreen.Show();

            await InitPools();
            await _environmentSceneProvider.LoadAndActivate();
            await _systems.Init();
            
            _stateMachine.SwitchState(new GameLoopState(_systems));
        }

        public void Run()
        {
            
        }

        public void Exit()
        {
            _ui.LoadingScreen.Hide();
        }

        private async Task InitPools()
        {
            foreach (var pool in _pools)
            {
                await pool.Load();
            }
        }
    }
}