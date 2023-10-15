using Godot;
using System;

public partial class ViewDistanceSlider : HSlider
{
	SettingsManager SM;

	public override void _Ready()
	{
		SM = GetNode<SettingsManager>("/root/SettingsManager");
		SM.ViewDistanceChanged += OnViewDistanceChanged;
		Value = float.Parse(SM.Settings["ViewDistance"]);
	}

	public void OnViewDistanceChanged(float value)
	{
		Value = value;
	}
}
