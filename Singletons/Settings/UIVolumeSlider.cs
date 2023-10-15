using Godot;
using System;

public partial class UIVolumeSlider : HSlider
{
	SettingsManager SM;

	public override void _Ready()
	{
		SM = GetNode<SettingsManager>("/root/SettingsManager");
		SM.UIVolumeChanged += OnUIVolumeChanged;
		Value = float.Parse(SM.Settings["UIVolume"]);
	}

	public void OnUIVolumeChanged(float value)
	{
		Value = value;
	}
}
