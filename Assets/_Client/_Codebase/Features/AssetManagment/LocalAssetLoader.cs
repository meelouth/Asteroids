using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace _Client
{
    public class LocalAssetLoader
    {
        private GameObject _cachedObject;

        protected async Task<T> Load<T>(string assetId) where T : MonoBehaviour
        {
            var handle = Addressables.InstantiateAsync(assetId);

            _cachedObject = await handle.Task;

            if (handle.Result.TryGetComponent(out T component) == false)
            {
                throw new NullReferenceException(
                    $"Objet {assetId} that you trying to load, doesn't have {typeof(T)} component.");
            }
            
            return component;
        }

        public void Unload()
        {
            if (_cachedObject == null)
            {
                return;
            }

            Addressables.ReleaseInstance(_cachedObject);
            _cachedObject = null;
        }
    }
}