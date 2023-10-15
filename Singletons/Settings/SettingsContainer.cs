using Godot;
using System;

public partial class SettingsContainer : CanvasLayer
{
    SettingsManager SettingsManager;

    public override void _EnterTree()
    {
		SettingsManager = GetNode<SettingsManager>("/root/SettingsManager");
        SettingsManager.SettingsContainer = this;
    }
}
