using System.Threading.Tasks;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

namespace _Client
{
    public class LocalSceneProvider
    {
        private SceneInstance _cachedScene;

        protected async Task<SceneInstance> Load(string sceneId, LoadSceneMode loadSceneMode)
        {
            var handle = Addressables.LoadSceneAsync(sceneId, loadSceneMode);
            _cachedScene = await handle.Task;

            return _cachedScene;
        }

        public void Unload()
        {
            Addressables.UnloadSceneAsync(_cachedScene);
        }
    }
}