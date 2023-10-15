using Godot;
using System;

public partial class MasterVolumeSlider : HSlider
{
	SettingsManager SM;

	public override void _Ready()
	{
		SM = GetNode<SettingsManager>("/root/SettingsManager");
		SM.MasterVolumeChanged += OnMasterVolumeChanged;
		Value = float.Parse(SM.Settings["MasterVolume"]);
	}

	public void OnMasterVolumeChanged(float value)
	{
		Value = value;
	}
}
