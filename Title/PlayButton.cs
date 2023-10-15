using Godot;
using System;

public partial class PlayButton : Button
{

  SceneManager SceneManager;
  
  public override void _Ready()
  {
      SceneManager = GetNode<SceneManager>("/root/SceneManager");
  }
  public override void _Pressed()
  {
	  SceneManager.ChangeScene("res://Main/Main.tscn");
	}
}
