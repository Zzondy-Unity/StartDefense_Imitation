using System;
using UnityEngine;

public class Monster : MonoBehaviour, IDamageable, IPoolable
{
    private HealthSystem healthSystem;
    private MonsterController monsterController;
    public Component poolKey { get; set; }

    public void Init(Vector3[] waypoints)
    {
        if (healthSystem == null)
        {
            healthSystem = new HealthSystem();
            if (TryGetComponent<MonsterController>(out MonsterController _monsterController))
            {
                monsterController = _monsterController;
            }
            else
            {
                monsterController = gameObject.AddComponent<MonsterController>();
            }
        }
        else
        {
            Regenerate();
        }
        
        monsterController.SetWaypoints(waypoints);
    }

    public void Regenerate()
    {
        healthSystem.Restore();
        monsterController.OnRestore();
    }

    /// <summary>
    /// 데미지를 처리하는 함수입니다.
    /// </summary>
    /// <param name="damage">데미지 총량</param>
    /// <returns>데미지를 입으면 true를 반환합니다.</returns>
    public bool TakeDamage(int damage)
    {
        return healthSystem.TakeDamage(damage);
    }

    public void OnSpawnFromPool()
    {
        healthSystem.OnDeath += monsterController.OnDeath;
    }

    public void OnReturnToPool()
    {
        healthSystem.OnDeath -= monsterController.OnDeath;
    }
}