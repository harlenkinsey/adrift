using Godot;
using System;

public partial class PauseMenuExitButton : Button
{
	SceneManager SceneManager;

	public override void _Ready()
	{
    	SceneManager = GetNode<SceneManager>("/root/SceneManager");
	}
	public override void _Pressed()
  	{
		GetTree().Paused = false;
		SceneManager.ChangeScene("res://Title/Title.tscn");
	}
}
