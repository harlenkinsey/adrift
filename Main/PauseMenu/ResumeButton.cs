using Godot;
using System;

public partial class ResumeButton : Button
{
	[Export]
	public PauseMenu PauseMenu;
	public override void _Pressed()
	{
		GetTree().Paused = false;
		PauseMenu.StateMachine.ChangeState(new PauseMenuState_InterfaceHidden(PauseMenu));
	}
}
