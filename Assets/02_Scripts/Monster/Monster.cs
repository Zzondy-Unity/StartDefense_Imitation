using UnityEngine;

[RequireComponent(typeof(MonsterController))]
public class Monster : MonoBehaviour, IDamageable, IPoolable, IAttacker
{
    [SerializeField] private atom_sliderTxt healthSlider;
    
    private HealthSystem healthSystem;
    public MonsterController monsterController { get; private set; }
    public Component poolKey { get; set; }
    public MonsterData data { get; private set; }

    public float HP;

    public void Init(MonsterData monsterData)
    {
        data = monsterData;
        if (healthSystem == null)
        {
            if (healthSlider == null)
            {
                healthSlider = GetComponentInChildren<atom_sliderTxt>();
            }
            
            healthSystem = new HealthSystem(data.maxHealth, healthSlider);
            if (TryGetComponent<MonsterController>(out MonsterController _monsterController))
            {
                monsterController = _monsterController;
                monsterController.Init(data, this);
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
        HP = data.maxHealth;
    }

    public void Regenerate()
    {
        healthSystem.Restore();
        monsterController.OnRestore();
        HP = data.maxHealth;
    }

    /// <summary>
    /// 데미지를 처리하는 함수입니다.
    /// </summary>
    /// <param name="damage">데미지 총량</param>
    /// <returns>데미지를 입으면 true를 반환합니다.</returns>
    public bool TakeDamage(float damage, IAttacker attacker)
    {
        bool damaged = healthSystem.TakeDamage(damage, attacker);
        if (damaged)
        {
            // MonsterHitState로 가고자 했으나
            // 데미지를 받는중에 이동할 수 없게되는거같아서 고민중
            // Logger.Log($"{attacker} attacked {gameObject.name} amount : {damage}");
        }

        HP = healthSystem.currentHealth;
        return damaged;
    }

    private void OnDeath()
    {
        monsterController.OnDeath();
        EventManager.Publish(GameEventType.GetGold, 10); // 일단 킬당 10골드
        EventManager.Publish(GameEventType.MonsterDeath, this);
        GameManager.Pool.ReturnToPool(this);
    }

    public void OnSpawnFromPool()
    {
        
    }

    public void OnReturnToPool()
    {
        
    }
}