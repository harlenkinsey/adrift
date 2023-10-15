using Godot;
using System;

public partial class SettingsButton : Button
{
	SettingsManager SM;

	public override void _Ready()
	{
		SM = GetNode<SettingsManager>("/root/SettingsManager");
	}

	public override void _Pressed()
	{
		SM.StateMachine.ChangeState(new SettingsState_InterfaceVisible(SM));
	}
}
