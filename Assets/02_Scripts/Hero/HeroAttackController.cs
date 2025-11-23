using System;
using System.Collections.Generic;
using UnityEngine;

public class HeroAttackController : MonoBehaviour
{
    public float attackSpeed { get; private set; }
    public float attack { get; private set; }
    public float attackRange { get; private set; }
    
    private CircleCollider2D attackCollider;
    private LayerMask monsterLayer;

    private HashSet<Monster> inAttackRange = new();

    private float lastAttack = 0;
    private Monster curAttackMonster;

    private Projectile bulletPrefab;

    private float attackInterval
    {
        get { return attackSpeed == 0 ? 1f / attackSpeed : 1; }
    }

    private bool isAlive = false;

    private IAttacker attacker;

    public void Init(HeroData data, IAttacker attacker)
    {
        EventManager.UnSubscribe(GameEventType.MonsterDeath, OnMonsterDead);
        EventManager.Subscribe(GameEventType.MonsterDeath, OnMonsterDead);
        
        this.attacker = attacker;
        attackSpeed = data.attackRate;
        attack = data.attack;
        attackRange = data.attackRange;
        
        attackCollider = GetComponent<CircleCollider2D>();
        attackCollider.radius = attackRange;
        isAlive = true;
        
        monsterLayer = LayerMask.GetMask("monster");
        bulletPrefab = GameManager.Resource.LoadAsset<Projectile>(data.bulletKey);
    }

    public void OnDead()
    {
        isAlive = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // 레이어 비교 후 
        if (monsterLayer.Contains(other.gameObject.layer))
        {
            // Monster이면
            if (other.gameObject.TryGetComponent<Monster>(out Monster monster))
            {
                // 리스트에 추가
                inAttackRange.Add(monster);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        // 레이어 비교 후
        if (monsterLayer.Contains(other.gameObject.layer))
        {
            // Monster이면
            if (other.gameObject.TryGetComponent<Monster>(out Monster monster))
            {
                // 리스트에 이미 있으면
                if (inAttackRange.Contains(monster))
                {
                    // 리스트에서 제거
                    inAttackRange.Remove(monster);

                    // 공격중이던 몬스터였을 시
                    if (monster == curAttackMonster)
                    {
                        curAttackMonster = null;
                    }
                }
            }
        }
    }

    private void Update()
    {
        if (!isAlive) return;
        if (inAttackRange.Count == 0) return;

        if (Time.time >= lastAttack + attackInterval)
        {
            if (CheckAttack())
            {
                Attack();
                lastAttack = Time.time;
            }
        }
    }

    private void Attack()
    {
        var bullet = GameManager.Pool.GetFromPool(bulletPrefab);
        bullet.Init(monsterLayer, 3f, attack, curAttackMonster, attacker);
        bullet.transform.position = transform.position;
    }

    public void OnMonsterDead(object org)
    {
        if (org is Monster monster)
        {
            if (inAttackRange.Contains(monster))
            {
                inAttackRange.Remove(monster);

                if (curAttackMonster == monster)
                {
                    curAttackMonster = null;
                }
            }
        }
    }

    private bool CheckAttack()
    {
        bool canAttack =                                        // 공격하려면
            curAttackMonster != null &&                      // 현재 공격중인 몬스터여야하고
            curAttackMonster.gameObject.activeInHierarchy &&    // Active상태여야 하며
            inAttackRange.Contains(curAttackMonster);           // 사거리 내에 있어야한다.

        if (!canAttack)
        {
            curAttackMonster = GetNearestMonster();
        }

        if (curAttackMonster == null)
        {
            Logger.Log($"공격할 수 없습니다.");
            return false;
        }

        return true;
    }

    private Monster GetNearestMonster()
    {
        Monster nearest = null;
        float nearestDistance = float.MaxValue;
        Vector3 myPosition = transform.position;

        foreach (var monster in inAttackRange)
        {
            if (monster == null || !monster.gameObject.activeInHierarchy) continue;
            
            float distance = (monster.transform.position - myPosition).sqrMagnitude;
            if (distance < nearestDistance)
            {
                nearestDistance = distance;
                nearest = monster;
            }
        }

        return nearest;
    }

    private void OnDestroy()
    {
        EventManager.UnSubscribe(GameEventType.MonsterDeath, OnMonsterDead);
    }
    
    private void OnDrawGizmos()
    {
        // 공격 범위(Trigger Collider)를 기즈모로 그리기
        // CircleCollider2D 를 공격 범위로 쓰고 있다고 가정
        var circle = GetComponent<CircleCollider2D>();
        if (circle == null)
            return;

        // 장면 뷰에서 보기 좋게 색 지정
        Gizmos.color = Color.red;

        // 2D라도 DrawWireSphere 를 써도 됩니다(씬 뷰에서 원처럼 보입니다).
        // 로컬 스케일이 적용되도록 radius * scale 처리
        float radius = circle.radius * Mathf.Max(transform.localScale.x, transform.localScale.y);

        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
