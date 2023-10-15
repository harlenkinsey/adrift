using Godot;
using System;
using System.Diagnostics;

public partial class TitleColor : Label
{

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		LabelSettings.FontColor = LabelSettings.FontColor.Lerp(new Color(1, 1, 1), (float)delta / 2f);
	}
}
