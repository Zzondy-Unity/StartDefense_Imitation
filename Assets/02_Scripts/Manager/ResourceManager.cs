using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 리소스를 관리하는 매니저입니다.
/// </summary>
public class ResourceManager : IManager
{
    private Dictionary<string, object> assetPools;
    
    public void Init()
    {
        assetPools = new Dictionary<string, object>();
    }
    
    public T LoadAsset<T>(string key) where T : UnityEngine.Object
    {
        if(assetPools.ContainsKey(key))
        {
            return (T)assetPools[key];
        }

        var asset = Resources.Load<T>(key);
        if (asset != null)
        {
            assetPools.Add(key, asset);
        }
        return asset;
    }
}
