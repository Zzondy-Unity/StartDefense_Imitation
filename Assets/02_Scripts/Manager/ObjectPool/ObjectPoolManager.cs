using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class ObjectPoolManager : MonoBehaviour, IManager
{
    // TODO :: Component를 id로 바꾸어서
    private readonly Dictionary<Component, object> _pools = new();  //Component는 프리팹 키, object는 Pool<T>
    
    public void Init()
    {
        _pools.Clear();
    }

    public T GetFromPool<T>(T prefab, int capacity = 10, int maxSize = 100) where T : Component, IPoolable
    {
        var pool = GetPool<T>(prefab,  capacity, maxSize);
        return pool.Get();
    }

    public void ReturnToPool<T>(T obj) where T : Component, IPoolable
    {
        var key = obj.poolKey;
        if (_pools.TryGetValue(key, out var pooled) && pooled is Pool<T> pool)
        {
            pool.Release(obj);
        }
    }

    private Pool<T> GetPool<T>(T prefab, int capacity = 10, int maxSize = 100)  where T : Component, IPoolable
    {
        if (_pools.TryGetValue(prefab, out var cached))
        {
            if (cached is Pool<T> pool)
            {
                return pool;
            }
            else
            {
                Logger.WarningLog($"[ObjectPoolManager] 오브젝트 풀이 아닌 객체입니다.");
                return null;
            }
        }
        else
        {
            GameObject obj = new GameObject(typeof(T).Name);
            obj.transform.SetParent(transform);
            
            Pool<T> newPool = new Pool<T>(prefab, obj.transform , capacity, maxSize);
            _pools.Add(prefab, newPool);
            return newPool;
        }
    }
}
