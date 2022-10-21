using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace _Client
{
    public class PoolContainer : MonoBehaviour
    {
        [SerializeField] private string assetKey;
        [SerializeField] private Transform root;

        private readonly Stack<PoolObject> _storage = new();

        private GameObject _template;

        public async Task Load()
        {
            await LoadPrefab(assetKey);
        }

        public PoolObject Get()
        {
            PoolObject obj;
            
            if (_storage.Count > 0)
            {
                obj = _storage.Pop();
            }
            else
            {
                var go = Instantiate(_template);
                obj = go.AddComponent<PoolObject>();
                obj.CachedTransform.SetParent(root);
                obj.CachedTransform.localScale = Vector3.one;
                obj.SetContainer(this);
            }

            obj.SetActive(true);
            return obj;
        }

        public void Return(PoolObject poolObject)
        {
            if (poolObject != null)
            {
                poolObject.SetActive(false);

                if (!_storage.Contains(poolObject))
                {
                    _storage.Push(poolObject);
                    poolObject.CachedTransform.SetParent(root, false);
                }
            }
        }

        public void Destroy()
        {
            Addressables.Release(_template);
            _template = null;
            
            _storage.Clear();
        }
        
        private async Task LoadPrefab(string assetId)
        {
            var handle = Addressables.LoadAssetAsync<GameObject>(assetId);

            while (!handle.IsDone)
            {
                await Task.Yield();
            }

            if (handle.Result == null)
                throw new NullReferenceException($"Objet {assetId} that you trying to load, doesn't found.");

            _template = handle.Result;
        }
    }
}