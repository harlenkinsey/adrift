using Godot;
using System;

public partial class GravityField : Area3D
{
	PlayerVariables PV;

	public override void _Ready()
	{
		PV = GetNode<PlayerVariables>("/root/PlayerVariables");
	}

	public void _on_body_entered(Node3D body)
	{
		if (body.Name == "Player")
		{
			PV.CurrentPlanet = (Planet)GetParent();
		}
	}

	public void _on_body_exited(Node3D body)
	{
		if (body.Name == "Player")
		{
			PV.CurrentPlanet = null;
		}
	}
}
