using System;
using UnityEngine;

public class HealthSystem
{
    private int maxHealth;
    private int currentHealth;

    public event Action OnDeath;

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