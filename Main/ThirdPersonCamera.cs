using Godot;
using System;

public partial class ThirdPersonCamera : Camera3D
{

	public override void _Ready()
	{
		Input.MouseMode = Input.MouseModeEnum.Captured;
	}


	public override void _Process(double delta)
	{
		Vector2 mouseInput = Input.GetLastMouseVelocity();

		Transform = Transform.RotatedLocal(Vector3.Right, -mouseInput.Y * (float)delta * 0.002f);
		Transform = Transform.RotatedLocal(Vector3.Up, -mouseInput.X * (float)delta * 0.002f);

		Vector2 movementInputDirection = Input.GetVector("Movement_Left", "Movement_Right", "Movement_Backward", "Movement_Forward");
		Vector3 movementDirection = (Transform.Basis * new Vector3(movementInputDirection.X, 0, -movementInputDirection.Y)).Normalized();

		Transform = new Transform3D(Transform.Basis, Transform.Origin + (movementDirection * 3f));
	}
}
