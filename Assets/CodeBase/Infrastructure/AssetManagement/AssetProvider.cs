using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace CodeBase.Infrastructure.AssetManagement
{
    public class AssetProvider : IAssetProvider
    {
        private readonly Dictionary<string, AsyncOperationHandle> _completedCaches =
            new Dictionary<string, AsyncOperationHandle>();

        private readonly Dictionary<string, List<AsyncOperationHandle>> _handles =
            new Dictionary<string, List<AsyncOperationHandle>>();

        public void Initialize()
        {
            Addressables.InitializeAsync();
        }
        
        public async Task<T> Load<T>(AssetReference assetReference) where T : class
        {
            if (_completedCaches.TryGetValue(assetReference.AssetGUID, out AsyncOperationHandle completedHandle))
            {
                return completedHandle.Result as T;
            }

            return await RunWithCacheOnComplete(
                handle: Addressables.LoadAssetAsync<T>(assetReference),
                cacheKey: assetReference.AssetGUID);
        }

        public async Task<T> Load<T>(string address) where T : class
        {
            if (_completedCaches.TryGetValue(address, out AsyncOperationHandle completedHandle))
            {
                return completedHandle.Result as T;
            }

            return await RunWithCacheOnComplete(
                handle: Addressables.LoadAssetAsync<T>(address),
                cacheKey: address);
        }

        public Task<GameObject> Instantiate(string address) =>
            Addressables.InstantiateAsync(address).Task;

        public Task<GameObject> Instantiate(string address, Vector3 at) =>
            Addressables.InstantiateAsync(address, at, Quaternion.identity).Task;

        public void CleanUp()
        {
            foreach (List<AsyncOperationHandle> cachedHandles in _handles.Values)
            {
                foreach (var handle in cachedHandles)
                {
                    Addressables.Release(handle);
                }
            }
            _handles.Clear();
            _completedCaches.Clear();
        }

        private async Task<T> RunWithCacheOnComplete<T>(AsyncOperationHandle<T> handle, string cacheKey) where T : class
        {
            handle.Completed += completeHandle =>
                _completedCaches[cacheKey] = completeHandle;

            AddHandle(cacheKey, handle);

            return await handle.Task;
        }

        private void AddHandle<T>(string key, AsyncOperationHandle<T> handle) where T : class
        {
            if (!_handles.TryGetValue(key, out List<AsyncOperationHandle> cachedHandles))
            {
                cachedHandles = new List<AsyncOperationHandle>();
                _handles[key] = cachedHandles;
            }

            cachedHandles.Add(handle);
        }
    }
}