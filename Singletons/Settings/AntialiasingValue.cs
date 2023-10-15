using Godot;
using System;

public partial class AntialiasingValue : Label
{
	SettingsManager SM;

	public override void _Ready()
	{
		SM = GetNode<SettingsManager>("/root/SettingsManager");
		Text = SM.Settings["Antialiasing"];
	}
	
	public void _on_antialiasing_slider_value_changed(float value)
	{
		Text = value.ToString();
	}
}
