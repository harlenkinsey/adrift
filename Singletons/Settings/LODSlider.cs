using Godot;
using System;

public partial class LODSlider : HSlider
{
	SettingsManager SM;

	public override void _Ready()
	{
		SM = GetNode<SettingsManager>("/root/SettingsManager");
		SM.LODThresholdChanged += OnLODThresholdChanged;
		Value = float.Parse(SM.Settings["LODThreshold"]);
	}

	public void OnLODThresholdChanged(float value)
	{
		Value = value;
	}
}
