using Godot;
using System;

public partial class PauseMenu : CanvasLayer
{
	public StateMachine StateMachine = new StateMachine();
	
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		StateMachine.ChangeState(new PauseMenuState_InterfaceHidden(this));
	}

	public override void _Process(double delta)
	{
		StateMachine.Update();
	}

	public override void _Input(InputEvent @event)
	{
		StateMachine._Input(@event);
	}

}

public class PauseMenuState_InterfaceVisible : IState
{
	PauseMenu owner;

	public PauseMenuState_InterfaceVisible(PauseMenu owner) { this.owner = owner; }

	public void Enter(StateMachine stateMachine)
	{  
		owner.Show();
		Input.MouseMode = Input.MouseModeEnum.Confined;
		owner.GetTree().Paused = true;
	}

	public void Execute()
	{
	}

	public void Exit()
	{
		owner.GetTree().Paused = false;
	}

	public void _Input(InputEvent @event)
	{
		if (@event.IsActionPressed("UI_Back"))
		{
			owner.StateMachine.ChangeState(new PauseMenuState_InterfaceHidden(owner));
		}
	}

}

public class PauseMenuState_InterfaceHidden : IState
{
	PauseMenu owner;

	public PauseMenuState_InterfaceHidden(PauseMenu owner) { this.owner = owner; }

	public void Enter(StateMachine stateMachine)
	{
		owner.Hide();
		Input.MouseMode = Input.MouseModeEnum.Captured;
	}

	public void Execute()
	{
	}

	public void Exit()
	{
	}

	public void _Input(InputEvent @event)
	{
		if (@event.IsActionPressed("UI_Back"))
		{
			owner.StateMachine.ChangeState(new PauseMenuState_InterfaceVisible(owner));
		}
	}

}
