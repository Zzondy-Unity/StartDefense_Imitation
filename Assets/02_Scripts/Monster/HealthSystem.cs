using System;
using UnityEngine;

public class HealthSystem
{
    public float maxHealth { get; private set; }
    public float currentHealth { get; private set; }
    atom_sliderTxt healthSlider;

    public event Action OnDeath;

    public HealthSystem(float maxHealth, atom_sliderTxt slider)
    {
        this.healthSlider = slider;
        currentHealth = maxHealth;
        this.maxHealth = maxHealth;
        
        healthSlider.maxValue = maxHealth;
        healthSlider.value = currentHealth;
        
        Restore();
    }

    public bool TakeDamage(float damage, IAttacker attacker)
    {
        if (damage <= 0) return false;
        
        // Logger.Log($"{attacker} attacke {damage}");
        
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