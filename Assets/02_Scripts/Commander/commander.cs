using System;
using UnityEngine;

public class Commander : MonoBehaviour, IDamageable, IAttacker
{
   private HealthSystem healthSystem;
   [SerializeField] private atom_sliderTxt healthSlider;
   public int maxHP = 200;

   private LayerMask monsterLayerMask;

   public void Init()
   {
      monsterLayerMask = LayerMask.GetMask("monster");
      
      if (healthSlider == null)
      {
         healthSlider = GetComponentInChildren<atom_sliderTxt>();
      }
      healthSystem = new HealthSystem(maxHP, healthSlider);

      healthSystem.OnDeath -= OnDeath;
      healthSystem.OnDeath += OnDeath;
   }

   private void OnDeath()
   {
      EventManager.Publish(GameEventType.CommanderDeath);
   }

   public bool TakeDamage(float damage, IAttacker attacker)
   {
      return healthSystem.TakeDamage(damage, attacker);
   }

   private void OnTriggerEnter2D(Collider2D other)
   {
      if (monsterLayerMask.Contains(other.gameObject.layer))
      {
         if (other.TryGetComponent<Monster>(out Monster monster))
         {
            monster.monsterController.ChangeMonsterState<MonsterAttackState>();
         }
      }
   }
}
