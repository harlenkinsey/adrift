using Godot;
using System;

public partial class LODValue : Label
{
	SettingsManager SM;

	public override void _Ready()
	{
		SM = GetNode<SettingsManager>("/root/SettingsManager");
		Text = SM.Settings["LODThreshold"];
	}
	
	public void _on_lod_slider_value_changed(float value)
	{
		Text = value.ToString();
	}
}
