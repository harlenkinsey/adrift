using Godot;

public class StateMachine
{
	public IState currentState;

	public void ChangeState(IState newState)
	{
		if (currentState != null)
			currentState.Exit();

		currentState = newState;
		currentState.Enter(this);
	}

	public void Update()
	{
		if (currentState != null) currentState.Execute();
	}

	public void _Input(InputEvent @event)
	{
		if (currentState != null) currentState._Input(@event);
	}
}

public interface IState
{
	public void Enter(StateMachine stateMachine);
	public void Execute();
	public void Exit();
	public void _Input(InputEvent @event);
}
