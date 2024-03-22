using Godot;
using System;

public partial class Player : RigidBody3D
{
	public const float Speed = 5f;
	public const float JumpVelocity = 5f;
	Vector3 LocalGravity;
	public bool IsJumping = false;
	PhysicsDirectBodyState3D State;

	public override void _IntegrateForces(PhysicsDirectBodyState3D state)
	{
		State = state;

		LocalGravity = state.TotalGravity.Normalized();

		if (IsOnFloor(state) && IsJumping)
		{
			ApplyCentralImpulse(-LocalGravity * JumpVelocity);
		}

		Vector2 movementInputDirection = Input.GetVector("Movement_Left", "Movement_Right", "Movement_Backward", "Movement_Forward");
		Vector3 movementDirection = (state.Transform.Basis * new Vector3(movementInputDirection.X, 0, -movementInputDirection.Y)).Normalized();


		state.LinearVelocity += LocalGravity * 9.8f * state.Step;

		if (state.LinearVelocity.Length() < 8f && IsOnFloor(state))
		{
			state.LinearVelocity += movementDirection * 0.5f;
		}

		//OrientCharacter(state);
	}

	public override void _Process(double delta)
	{
		Vector2 mouseInput = Input.GetLastMouseVelocity();
		State.Transform = State.Transform.RotatedLocal(-LocalGravity, -mouseInput.X * (float)delta * 0.001f);
	}

	public void OrientCharacter(PhysicsDirectBodyState3D state)
	{
		Vector3 leftAxis = -LocalGravity.Cross(Transform.Basis.Z);
		Basis rotationBasis = new Basis(leftAxis, -LocalGravity, Transform.Basis.Z).Orthonormalized();

		Basis a = new Basis(state.Transform.Basis.GetRotationQuaternion().Normalized().Slerp(rotationBasis.GetRotationQuaternion().Normalized(), state.Step * 3f));

		state.Transform = new Transform3D(a, state.Transform.Origin);
	}

	public bool IsOnFloor(PhysicsDirectBodyState3D state)
	{
		for (int i = 0; i < state.GetContactCount(); i++)
		{
			Vector3 contactNormal = state.GetContactLocalNormal(i);

			if (contactNormal.Dot(-LocalGravity) > 0.5f)
			{
				return true;
			}
		}

		return false;
	}

	public override void _Input(InputEvent @event)
	{
		IsJumping = @event.GetActionStrength("Movement_Jump") > 0f ? true : false;
	}
}
