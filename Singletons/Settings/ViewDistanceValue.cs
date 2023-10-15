using Godot;
using System;

public partial class ViewDistanceValue : Label
{
	SettingsManager SM;

	public override void _Ready()
	{
		SM = GetNode<SettingsManager>("/root/SettingsManager");
		Text = SM.Settings["ViewDistance"];
	}
	
	public void _on_view_distance_slider_value_changed(float value)
	{
		Text = value.ToString();
	}
}
