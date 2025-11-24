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
      // 죽어있는 상태로 변경 -> 데미지를 입지않아 이 함수를 호출하지 않아야하며,
      // 흐음
      isAlive = false;
      EventManager.Publish(GameEventType.GameEnd, false);
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
