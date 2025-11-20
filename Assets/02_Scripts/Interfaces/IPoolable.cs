using UnityEngine;
using UnityEngine.Pool;

public interface IPoolable
{
    public Component poolKey { get; set; }
    void OnSpawnFromPool();

    void OnReturnToPool();
}
