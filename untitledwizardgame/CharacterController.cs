using Godot;
using System;

public partial class CharacterController : CharacterBody3D
{
	public const float Speed = 5.0f;
	public const float JumpVelocity = 4.5f;

	// Get the gravity from the project settings to be synced with RigidBody nodes.
	public float gravity = ProjectSettings.GetSetting("physics/3d/default_gravity").AsSingle();

	private Node3D cameraNode;

	public override void _Ready()
	{
		// Assuming the camera is a child of the character and named "Camera3D"
		cameraNode = GetNode<Node3D>("Camera3D");
		if (cameraNode == null)
		{
			GD.PrintErr("Camera3D node not found as a child of the character");
		}
	}

	public override void _PhysicsProcess(double delta)
	{
		Vector3 velocity = Velocity;

		// Add the gravity.
		if (!IsOnFloor())
			velocity.Y -= gravity * (float)delta;

		// Handle Jump.
		if (Input.IsActionJustPressed("ui_accept") && IsOnFloor())
			velocity.Y = JumpVelocity;

		// Get the input direction and handle the movement/deceleration.
		Vector2 inputDir = Input.GetVector("pl_strafe_left", "pl_strafe_right", "pl_forward", "pl_backward");
		Vector3 direction = (Transform.Basis * new Vector3(inputDir.X, 0, inputDir.Y)).Normalized();
		if (direction != Vector3.Zero)
		{
			velocity.X = direction.X * Speed;
			velocity.Z = direction.Z * Speed;
		}
		else
		{
			velocity.X = Mathf.MoveToward(Velocity.X, 0, Speed);
			velocity.Z = Mathf.MoveToward(Velocity.Z, 0, Speed);
		}

		Velocity = velocity;
		MoveAndSlide();

		// Update character rotation to face away from the camera
		if (cameraNode != null)
		{
			Vector3 lookAtPos = GlobalPosition + (cameraNode.GlobalTransform.Basis.Z * -1);
			LookAt(lookAtPos, Vector3.Up);
		}
	}
}
