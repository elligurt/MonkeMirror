using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;

namespace MonkeMirror.Tools
{
    //https://github.com/developer9998/GorillaHistoricalTeleporter/blob/main/GorillaHistoricalTeleporter/Tools/AssetLoader.cs (slightly modified)
    public class AssetLoader
    {
        private static AssetBundle loadedBundle;
        private static readonly Dictionary<string, Object> loadedAssets = new Dictionary<string, Object>();

        private static Task bundleLoadTask;

        public static async Task<T> LoadAsset<T>(string assetName) where T : Object
        {
            Object cached;
            if (loadedAssets.TryGetValue(assetName, out cached) && cached is T)
            {
                return (T)cached;
            }

            if (loadedBundle == null)
            {
                if (bundleLoadTask == null)
                {
                    bundleLoadTask = LoadAssetBundle();
                }
                await bundleLoadTask;
            }

            var completionSource = new TaskCompletionSource<T>();

            AssetBundleRequest request = loadedBundle.LoadAssetAsync<T>(assetName);
            request.completed += _ =>
            {
                T result = request.asset as T;
                completionSource.TrySetResult(result);
            };

            T loadedAsset = await completionSource.Task;

            if (loadedAsset != null)
            {
                loadedAssets[assetName] = loadedAsset;
            }

            return loadedAsset;
        }

        private static async Task LoadAssetBundle()
        {
            var completionSource = new TaskCompletionSource<AssetBundle>();

            Stream stream = typeof(AssetLoader).Assembly.GetManifestResourceStream("MonkeMirror.Content.mirror");
            if (stream == null)
            {
                throw new FileNotFoundException("Embedded asset bundle not found. Make sure it's added as an embedded resource!!!!");
            }

            AssetBundleCreateRequest request = AssetBundle.LoadFromStreamAsync(stream);
            request.completed += _ =>
            {
                completionSource.TrySetResult(request.assetBundle);
            };

            loadedBundle = await completionSource.Task;
        }
    }
}