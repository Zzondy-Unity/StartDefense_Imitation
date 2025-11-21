using System;
using UnityEngine;

public class HealthSystem
{
    private float maxHealth;
    private float currentHealth;

    public event Action OnDeath;

    public HealthSystem(MonsterData data)
    {
        maxHealth = data.maxHealth;
        currentHealth = maxHealth;
        Restore();
    }

    public bool TakeDamage(int damage)
    {
        if (damage <= 0)
        {
            Dead();
            return false;
        }

        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            return true;
        }

        return true;
    }

    private void Dead()
    {
        OnDeath?.Invoke();
    }

    public void Restore()
    {
        currentHealth = maxHealth;
    }

    public void Heal(int amount)
    {
        currentHealth = Mathf.Min(maxHealth, currentHealth + amount);
    }
}