using Godot;
using System;

public partial class HUDManager : CanvasLayer
{

    PlayerVariables PV;
    ProgressBar HealthBar;
    ProgressBar StaminaBar;

    public override void _Ready()
    {
        PV = GetNode<PlayerVariables>("/root/PlayerVariables");

        HealthBar = (ProgressBar)FindChild("HealthBar");
        StaminaBar = (ProgressBar)FindChild("StaminaBar");

        HealthBar.Value = PV.Health;
        StaminaBar.Value = PV.Stamina;
    }
}
