using System;
using UnityEngine;

public class Commander : MonoBehaviour, IDamageable, IAttacker
{
   private HealthSystem healthSystem;
   [SerializeField] private atom_sliderTxt healthSlider;
   public int maxHP = 200;

   private LayerMask monsterLayerMask;
   public bool isAlive { get; private set; }

   public void Init()
   {
      monsterLayerMask = LayerMask.GetMask("monster");
      isAlive = true;
      
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
      isAlive = false;
      EventManager.Publish(GameEventType.GameEnd, false);
   }

   public bool TakeDamage(float damage, IAttacker attacker)
   {
      return healthSystem.TakeDamage(damage, attacker);
   }

   // 현재 중복되어있기 때문에 변경가능성 높
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
