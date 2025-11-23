using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Projectile : MonoBehaviour, IPoolable
{
    private Vector3 targetPosition;
    private LayerMask targetLayerMask;

    private Collider2D[] hits = new Collider2D[10];
    private HashSet<Monster> damaged = new();

    private float speed = 10f;
    private float radius;
    private float damage;
    
    public IAttacker attacker { get; private set; }

    public void Init(LayerMask targetLayer, float radius, float damage, Monster target, IAttacker attacker)
    {
        targetLayerMask = targetLayer;
        this.radius = radius;
        this.damage = damage;
        this.targetPosition = target.transform.position;
        
        Vector2 direction = targetPosition - transform.position;
        direction.Normalize();

        this.gameObject.transform.right = direction;
        this.attacker = attacker;
    }

    private void Update()
    {
        if (!gameObject.activeInHierarchy) return;
        
        Vector2 curPos = transform.position;
        Vector2 nextPosition = Vector2.MoveTowards(curPos, targetPosition, speed * Time.deltaTime);
        transform.position = nextPosition;
        
        if ((transform.position - targetPosition).sqrMagnitude <= 0.1f)
        {
            OnArrive();
        }
    }

    private void OnArrive()
    {
        damaged.Clear();
        
        int hitCount = Physics2D.OverlapCircleNonAlloc(transform.position, radius, hits, targetLayerMask);
        for (int i = 0; i < hitCount; i++)
        {
            if (hits[i].TryGetComponent(out Monster monster))
            {
                if (damaged.Contains(monster) || !monster.gameObject.activeInHierarchy) continue;

                if (damaged.Count >= 3) break;   // 광역데미지는 3마리까지만

                if (monster.TakeDamage(damage, attacker))
                {
                    // 공격 성공시
                    damaged.Add(monster);
                }
            }
        }
        
        GameManager.Pool.ReturnToPool(this);
    }

    public Component poolKey { get; set; }
    public void OnSpawnFromPool()
    {
        
    }

    public void OnReturnToPool()
    {
        damaged.Clear();
    }
}