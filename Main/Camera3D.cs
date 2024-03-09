using Godot;
using System;

public partial class Camera3D : Godot.Camera3D
{
	float CollapseCooldown = 0f;
	PlayerVariables PV;

	public override void _Ready()
	{
		PV = GetNode<PlayerVariables>("/root/PlayerVariables");
		Input.MouseMode = Input.MouseModeEnum.Captured;
	}

	public override void _Process(double delta)
	{
		Vector2 mouseInput = Input.GetLastMouseVelocity();

		if (mouseInput.Y > 0f && Transform.Basis.GetEuler().X > -0.9f)
		{
			Transform = Transform.RotatedLocal(Transform.Basis.X, -mouseInput.Y * (float)delta * 0.001f);
		}
		else if (mouseInput.Y < 0f && Transform.Basis.GetEuler().X < 0.9f)
		{
			Transform = Transform.RotatedLocal(Transform.Basis.X, -mouseInput.Y * (float)delta * 0.001f);
		}

		CollapseCooldown -= (float)delta;
	}

	private const float RayLength = 1000.0f;

	public override void _PhysicsProcess(double delta)
	{
		if (Input.IsMouseButtonPressed(MouseButton.Left))
		{
			var spaceState = GetWorld3D().DirectSpaceState;
			var from = ProjectRayOrigin(GetTree().Root.GetVisibleRect().Size / 2f);
			var to = from + ProjectRayNormal(GetTree().Root.GetVisibleRect().Size / 2f) *
			RayLength;

			var query = PhysicsRayQueryParameters3D.Create(from, to);
			query.CollideWithAreas = false;

			Vector3 intersectPosition = (Vector3)spaceState.IntersectRay(query)["position"];
			TryCollapseFaceAtPosition(intersectPosition);
		}
	}

	public void TryCollapseFaceAtPosition(Vector3 position)
	{
		if (CollapseCooldown <= 0f)
		{
			PV.CurrentPlanet.CollapseFaceAtPosition(position);
			CollapseCooldown = 1f;
		}
	}
}
