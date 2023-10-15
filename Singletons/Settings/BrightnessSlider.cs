using Godot;
using System;

public partial class BrightnessSlider : HSlider
{
	SettingsManager SM;

	public override void _Ready()
	{
		SM = GetNode<SettingsManager>("/root/SettingsManager");
		SM.BrightnessChanged += OnBrightnessChanged;
		Value = float.Parse(SM.Settings["Brightness"]);
	}

	public void OnBrightnessChanged(float value)
	{
		Value = value;
	}
}
