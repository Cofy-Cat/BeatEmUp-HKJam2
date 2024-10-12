using System;
using System.Threading;
using System.Threading.Tasks;
using cfEngine.Asset;
using cfEngine.Logging;
using UnityEngine;

public class ResourceAssetManager : AssetManager<UnityEngine.Object>
{
    protected override UnityEngine.Object _Load(string path)
    {
        var asset = Resources.Load(path);
        if (asset == null)
        {
            throw new ArgumentException($"Asset not found ({path})", nameof(path));
        }

        return asset;
    }

    protected override T _Load<T>(string path)
    {
        var asset = Resources.Load<T>(path);
        if (asset == null)
        {
            throw new ArgumentException($"Asset not found ({path})", nameof(path));
        }

        return asset;
    }

    protected override async Task<UnityEngine.Object> _LoadAsync(string path, CancellationToken token)
    {
        if (token.WaitHandle != null)
        {
            Log.LogWarning("Resources Load can't really be cancelled");
        }

        try
        {
            var request = Resources.LoadAsync(path);
            await request;

            return request.asset;
        }
        catch (Exception e)
        {
            Log.LogException(e, $"Resource {path} load failed.");
            return null;
        }
    }

    protected override async Task<T> _LoadAsync<T>(string path, CancellationToken token)
    {
        if (token.WaitHandle != null)
        {
            Log.LogWarning("Resources Load can't really be cancelled");
        }

        try
        {
            var req = Resources.LoadAsync<T>(path);
            await req;

            var t = (T)req.asset;
            return t;
        }
        catch (Exception e)
        {
            Log.LogException(e, $"Resource {path} load failed.");
            return null;
        }
    }
}