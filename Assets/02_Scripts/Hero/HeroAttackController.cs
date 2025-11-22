using System;
using System.Collections.Generic;
using UnityEngine;

public class HeroAttackController : MonoBehaviour
{
    public float attackSpeed { get; private set; }
    public float attack { get; private set; }
    
    private CircleCollider2D attackCollider;
    private LayerMask monsterLayer;

    private HashSet<Monster> inAttackRange = new();

    private float lastAttack = 0;
    private Monster curAttackMonster;

    private float attackInterval
    {
        get { return attackSpeed == 0 ? 1f / attackSpeed : 1; }
    }

    private bool isAlive = false;

    public void Init(HeroData data)
    {
        EventManager.UnSubscribe(GameEventType.MonsterDeath, OnMonsterDead);
        EventManager.Subscribe(GameEventType.MonsterDeath, OnMonsterDead);
        
        attackSpeed = data.attackRate;
        attack = data.attack;
        
        attackCollider = GetComponent<CircleCollider2D>();
        isAlive = true;
        
        monsterLayer = LayerMask.GetMask("monster");
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
        // 투사체발사?
        // 투사체가 날아가다가 닿으면 범위공격이네
        
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
}
