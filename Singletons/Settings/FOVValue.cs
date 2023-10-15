using Godot;
using System;

public partial class FOVValue : Label
{
	SettingsManager SM;

	public override void _Ready()
	{
		SM = GetNode<SettingsManager>("/root/SettingsManager");
		Text = SM.Settings["FOV"];
	}
	
	public void _on_fov_slider_value_changed(float value)
	{
		Text = value.ToString();
	}

}
