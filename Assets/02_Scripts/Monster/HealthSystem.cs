using System;
using UnityEngine;
using UnityEngine.UI;

public class HealthSystem
{
    private float maxHealth;
    private float currentHealth;
    Slider healthSlider;

    public event Action OnDeath;

    public HealthSystem(MonsterData data, Slider slider)
    {
        this.healthSlider = slider;
        maxHealth = data.maxHealth;
        currentHealth = maxHealth;
        
        healthSlider.maxValue = maxHealth;
        healthSlider.value = currentHealth;
        
        Restore();
    }

    public bool TakeDamage(float damage)
    {
        if (damage <= 0) return false;

        currentHealth -= damage;
        healthSlider.value = currentHealth;
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            healthSlider.value = 0;
            Dead();
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