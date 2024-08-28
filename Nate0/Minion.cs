using Godot;
using System;

public partial class Minion : CharacterBody2D
{
	[Export]
	public float JumpForce = 300.0f;

	[Export]
	public float MoveSpeed = 100.0f;

	[Export]
	public float MaxJumpInterval = 3.0f;

	[Export]
	public float RotationSpeed = 5.0f;

	[Export]
	public float CollisionRotationFactor = 0.1f;

	private float _jumpTimer = 0.0f;
	private float _currentJumpInterval;

	public override void _Ready()
	{
		_currentJumpInterval = (float)GD.RandRange(0.5, MaxJumpInterval);
	}

	public override void _PhysicsProcess(double delta)
	{
		Vector2 velocity = Velocity;

		// Apply gravity
		if (!IsOnFloor())
			velocity.Y += 980 * (float)delta;

		// Move towards cursor
		Vector2 cursorPos = GetGlobalMousePosition();
		Vector2 direction = (cursorPos - GlobalPosition).Normalized();

		velocity.X = direction.X * MoveSpeed;

		// Jump at random intervals
		_jumpTimer += (float)delta;
		if (_jumpTimer >= _currentJumpInterval)
		{
			_jumpTimer = 0;
			if (IsOnFloor())
			{
				velocity.Y = -JumpForce;
			}
			_currentJumpInterval = (float)GD.RandRange(0.5, MaxJumpInterval);
		}

		Velocity = velocity;
		MoveAndSlide();

		// Rotate based on collisions
		for (int i = 0; i < GetSlideCollisionCount(); i++)
		{
			var collision = GetSlideCollision(i);
			Vector2 collisionNormal = collision.GetNormal();
			float rotationAmount = collisionNormal.Cross(Vector2.Up) * CollisionRotationFactor;
			Rotate(rotationAmount);
		}

		// Apply constant rotation
		Rotate(RotationSpeed * (float)delta);
	}
}
