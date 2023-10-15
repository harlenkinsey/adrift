using Godot;
using System;

public partial class RenderScaleSlider : HSlider
{
	SettingsManager SM;

	public override void _Ready()
	{
		SM = GetNode<SettingsManager>("/root/SettingsManager");
		SM.RenderScaleChanged += OnRenderScaleChanged;
		Value = float.Parse(SM.Settings["RenderScale"]);
	}

	public void OnRenderScaleChanged(float value)
	{
		Value = value;
	}
}
