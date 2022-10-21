using System.Threading.Tasks;
using UnityEngine.SceneManagement;

namespace _Client
{
    public class EnvironmentSceneProvider : LocalSceneProvider
    {
        public async Task LoadAndActivate()
        {
            await Load(SceneNames.EnvironmentScene, LoadSceneMode.Additive);
        }
    }
}