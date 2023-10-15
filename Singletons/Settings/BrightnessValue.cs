using Godot;
using System;

public partial class BrightnessValue : Label
{
	SettingsManager SM;

	public override void _Ready()
	{
		SM = GetNode<SettingsManager>("/root/SettingsManager");
		Text = SM.Settings["Brightness"];
	}
	
	public void _on_brightness_slider_value_changed(float value)
	{
		Text = value.ToString();
	}
}
