using Godot;
using System;

public partial class MusicVolumeSlider : HSlider
{
	SettingsManager SM;

	public override void _Ready()
	{
		SM = GetNode<SettingsManager>("/root/SettingsManager");
		SM.MusicVolumeChanged += OnMusicVolumeChanged;
		Value = float.Parse(SM.Settings["MusicVolume"]);
	}

	public void OnMusicVolumeChanged(float value)
	{
		Value = value;
	}
}
