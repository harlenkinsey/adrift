using Godot;
using System;

public partial class MasterVolumeValue : Label
{
	SettingsManager SM;

	public override void _Ready()
	{
		SM = GetNode<SettingsManager>("/root/SettingsManager");
		Text = SM.Settings["MasterVolume"];
	}

	public void _on_master_volume_slider_value_changed(float value)
	{
		Text = value.ToString();
	}
	
}
