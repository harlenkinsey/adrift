using Godot;
using System;

public partial class Camera3D : Godot.Camera3D
{
	
	Basis MaxCameraDown;
	Basis MaxCameraUp;

    public override void _Ready()
    {
		Input.MouseMode = Input.MouseModeEnum.Captured;
    }


    public override void _Process(double delta)
	{
		Vector2 mouseInput = Input.GetLastMouseVelocity();
		
		if(mouseInput.Y > 0f && Transform.Basis.GetEuler().X > -0.9f)
		{
			Transform = Transform.RotatedLocal(Transform.Basis.X, -mouseInput.Y * (float) delta * 0.001f);
		}
		else if (mouseInput.Y < 0f && Transform.Basis.GetEuler().X < 0.9f)
		{
			Transform = Transform.RotatedLocal(Transform.Basis.X, -mouseInput.Y * (float) delta * 0.001f);
		}

	}
}
