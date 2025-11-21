using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterController : MonoBehaviour
{
    protected MonsterStateMachine stateMachine;
    
    private bool isAlive = true;

    private Dictionary<System.Type, MonsterState> monsterStateDic;
    
    public MonsterData monsterData { get; private set; }
    public Rigidbody2D rb2D { get; private set; }

    public void Init(MonsterData data)
    {
        stateMachine = new MonsterStateMachine();
        rb2D = GetComponentInChildren<Rigidbody2D>();
        
        monsterData = data;
        
        monsterStateDic = new Dictionary<System.Type, MonsterState>()
        {
            { typeof(MonsterMoveState), new MonsterMoveState(this) },
            { typeof(MonsterHitState), new MonsterHitState(this) },
            { typeof(MonsterAttackState), new MonsterAttackState(this) },
        };

        ChangeMonsterState<MonsterMoveState>();
    }

    private void FixedUpdate()
    {
        if (isAlive)
        {
            stateMachine.Update();
        }
    }

    public MonsterState ChangeMonsterState<T>() where T : MonsterState
    {
        var newStateType = typeof(T);
        if (monsterStateDic.TryGetValue(newStateType, out MonsterState newState))
        {
            stateMachine.ChangeState(newState);
            return newState;
        }

        return null;
    }
    
    public void OnRestore()
    {
        isAlive = true;

        if (stateMachine != null)
        {
            ChangeMonsterState<MonsterMoveState>();
        }
    }

    public void OnDeath()
    {
        isAlive = false;
    }
}