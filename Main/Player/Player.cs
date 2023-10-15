using Godot;
using System;

public partial class Player : RigidBody3D
{
	Vector3 localGravity;
	float moveSpeed = 0.5f;
	float rotationSpeed = 2f;
	float jumpInitialImpulse = 10f;
	bool IsJumping = false;
	Vector3 moveDirection = Vector3.Zero;
	Vector3 lastStrongDirection = Vector3.Zero;
	

    public override void _IntegrateForces(PhysicsDirectBodyState3D state)
    {
        localGravity = state.TotalGravity.Normalized();
		
		if(moveDirection.Length() > 0.2f)
		{
			lastStrongDirection = moveDirection.Normalized();
		}

		moveDirection = GetModelOrientedInput();
		OrientCharacterToDirection(lastStrongDirection, state.Step, state);

		if(IsOnFloor(state) && IsJumping)
		{
			ApplyCentralImpulse(-localGravity * jumpInitialImpulse);
		}	
		
		state.LinearVelocity = localGravity;
    }

    public Vector3 GetModelOrientedInput() 
	{

		float x = 0f;
		float velocityX = Input.GetLastMouseVelocity().X;

		if(velocityX > 0f)
		{
			x = -0.3f;
		} else if (velocityX < 0f)
		{
			x = 0.3f;
		}

		Vector3 rawInput = new Vector3(x, 0f, 0f);
		Vector3 input = Vector3.Zero;

		input.X = rawInput.X * Mathf.Sqrt(1f - rawInput.Y * rawInput.Y / 2f);
		input.Z = rawInput.Y * Mathf.Sqrt(1f - rawInput.X * rawInput.X / 2f);

		return Transform.Basis * rawInput;
	}

	public void OrientCharacterToDirection(Vector3 direction, float delta, PhysicsDirectBodyState3D state)
	{
		
		Vector3 leftAxis = -localGravity.Cross(direction);
		Basis rotationBasis = new Basis(leftAxis, -localGravity, direction).Orthonormalized();

		Basis a = new Basis(state.Transform.Basis.GetRotationQuaternion().Normalized().Slerp(rotationBasis.GetRotationQuaternion().Normalized(), delta * rotationSpeed));

		
		state.Transform = new Transform3D(a, state.Transform.Origin);
	}

	public bool IsOnFloor(PhysicsDirectBodyState3D state)
	{
		for(int i = 0; i < state.GetContactCount(); i++)
		{
			Vector3 contactNormal = state.GetContactLocalNormal(i);

			if(contactNormal.Dot(-localGravity) > 0.5f)
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
