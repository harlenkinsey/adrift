using Godot;
using System;

public partial class AntialiasingSlider : HSlider
{
	SettingsManager SM;

	public override void _Ready()
	{
		SM = GetNode<SettingsManager>("/root/SettingsManager");
		SM.AntialiasingChanged += OnAntialiasingChanged;
		Value = float.Parse(SM.Settings["Antialiasing"]);
	}

	public void OnAntialiasingChanged(float value)
	{
		Value = value;
	}
}
