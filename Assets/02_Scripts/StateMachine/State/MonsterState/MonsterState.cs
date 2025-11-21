
public abstract class MonsterState : IState
{
    protected MonsterController monsterController;
    
    public MonsterState(MonsterController monsterController)
    {
        this.monsterController = monsterController;
    }

    public abstract void Enter();

    public abstract void Exit();

    public abstract void Update();
}
