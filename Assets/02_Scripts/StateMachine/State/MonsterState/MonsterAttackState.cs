using UnityEngine;

public class MonsterAttackState : MonsterState
{
    private float attackInterval = 1f;
    private float lastAttack = 0;

    private float attackDamage = 1;
    
    public MonsterAttackState(MonsterController monsterController) : base(monsterController)
    {
    }

    public override void Enter()
    {
        
    }

    public override void Exit()
    {
        
    }

    public override void Update()
    {
        // 일정시간마다 지휘관에게 공격
        if (Time.time > attackInterval + lastAttack)
        {
            AttackCommander();
        }
    }

    private void AttackCommander()
    {
        var manager = GameManager.Scene.curSceneManager as GameSceneManager;
        if (manager != null && manager.Commander.isAlive && manager.Commander.TakeDamage(attackDamage, monsterController.attacker))
        {
            // TODO :: Shake Camera
        }
    }
}
