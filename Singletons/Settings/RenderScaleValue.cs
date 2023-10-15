using Godot;
using System;

public partial class RenderScaleValue : Label
{
	SettingsManager SM;

	public override void _Ready()
	{
		SM = GetNode<SettingsManager>("/root/SettingsManager");
		Text = SM.Settings["RenderScale"];
	}
	
	public void _on_render_scale_slider_value_changed(float value)
	{
		Text = value.ToString();
	}
}
