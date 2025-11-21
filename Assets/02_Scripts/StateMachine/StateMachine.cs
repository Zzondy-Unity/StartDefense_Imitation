public abstract class StateMachine
{
    public IState currentState;
    public IState previousState;

    public void ChangeState(IState newState)
    {
        currentState?.Exit();
        
        previousState = currentState;
        currentState = newState;
        
        currentState.Enter();
    }

    public void Update()
    {
        currentState?.Update();
    }
}
