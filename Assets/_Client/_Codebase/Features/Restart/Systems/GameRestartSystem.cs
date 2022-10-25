using System.Threading.Tasks;
using UnityEngine.SceneManagement;

namespace _Client
{
    public class GameRestartSystem : IInitSystem, IRunSystem
    {
        private EcsFilter _restartCommands;
        
        public Task Init(EcsSystems systems)
        {
            _restartCommands = systems
                .GetWorld()
                .Filter()
                .With<RestartGameCommand>().Build();
            
            return Task.CompletedTask;
        }

        public void Run(EcsSystems systems)
        {
            foreach (var entity in _restartCommands)
            {
                SceneManager.LoadScene(SceneNames.GameplayScene, LoadSceneMode.Single);
            }
        }
    }
}