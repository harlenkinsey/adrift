using Godot;
using System;

public partial class UIVolumeValue : Label
{
	SettingsManager SM;

	public override void _Ready()
	{
		SM = GetNode<SettingsManager>("/root/SettingsManager");
		Text = SM.Settings["UIVolume"];
	}

	public void _on_ui_volume_slider_value_changed(float value)
	{
		Text = value.ToString();
	}
}
