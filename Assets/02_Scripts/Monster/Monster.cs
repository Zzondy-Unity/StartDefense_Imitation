using System;
using UnityEngine;

[RequireComponent(typeof(MonsterController))]
public class Monster : MonoBehaviour, IDamageable, IPoolable
{
    private HealthSystem healthSystem;
    private MonsterController monsterController;
    public Component poolKey { get; set; }
    public MonsterData data { get; private set; }

    public void Init(MonsterData monsterData)
    {
        data = monsterData;
        if (healthSystem == null)
        {
            healthSystem = new HealthSystem(data);
            if (TryGetComponent<MonsterController>(out MonsterController _monsterController))
            {
                monsterController = _monsterController;
                monsterController.Init(data);
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
        
        healthSystem.OnDeath -= OnDeath;
        healthSystem.OnDeath += OnDeath;
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
    public bool TakeDamage(float damage)
    {
        return healthSystem.TakeDamage(damage);
    }

    private void OnDeath()
    {
        monsterController.OnDeath();
        EventManager.Publish(GameEventType.MonsterDeath, this);
    }

    public void OnSpawnFromPool()
    {
        
    }

    public void OnReturnToPool()
    {
        
    }
}