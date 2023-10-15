using Godot;
using System;

public partial class FOVSlider : HSlider
{
	SettingsManager SM;

	public override void _Ready()
	{
		SM = GetNode<SettingsManager>("/root/SettingsManager");
		SM.FOVChanged += OnFOVChanged;
		Value = float.Parse(SM.Settings["FOV"]);
	}

	public void OnFOVChanged(float value)
	{
		Value = value;
	}
}
