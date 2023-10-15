using Godot;
using System;

public partial class MusicVolumeValue : Label
{
	SettingsManager SM;

	public override void _Ready()
	{
		SM = GetNode<SettingsManager>("/root/SettingsManager");
		Text = SM.Settings["MusicVolume"];
	}

	public void _on_music_volume_slider_value_changed(float value)
	{
		Text = value.ToString();
	}
}
