using UnityEngine;
using UnityEngine.Pool;

public class Pool<T> where T : Component, IPoolable
{
    private readonly IObjectPool<T> _pool;
    
    public Pool(T prefab, Transform parent, int defaultCapacity, int maxSize)
    {
        _pool = new ObjectPool<T>(
            () => Create(prefab, parent),
            OnTakeFromPool,
            OnReturnedToPool,
            OnDestroyPoolObject,
            true,
            defaultCapacity,
            maxSize);
    }

    public T Get()
    {
        return _pool.Get();
    }

    public void Release(T obj)
    {
        _pool.Release(obj);
    }

    private void OnDestroyPoolObject(T obj)
    {
        Object.Destroy(obj.gameObject);
    }

    private void OnReturnedToPool(T obj)
    {
        obj?.OnReturnToPool();
        obj.gameObject.SetActive(false);
    }

    private void OnTakeFromPool(T obj)
    {
        obj.gameObject.SetActive(true);
        obj?.OnSpawnFromPool();
    }

    private T Create(T prefab, Transform parent)
    {
        var obj = Object.Instantiate(prefab, parent);
        obj.gameObject.SetActive(false);    // TakeFromPool에서 true로 변경됨
        obj.poolKey = prefab;
        return obj;
    }
}
