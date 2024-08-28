using Godot;
using System;

public partial class FollowerCharacter : Character
{
	private const float FollowDistance = 64.0f; // Assuming one body length is 64 pixels

	private Character _playerCharacter;

	public override void _Ready()
	{
		base._Ready();
		_playerCharacter = GetNode<Character>("/root/PlatformScene/Character");
	}

	public override void _PhysicsProcess(double delta)
	{
		if (GetTree().Paused)
			return;

		Vector2 toPlayer = _playerCharacter.GlobalPosition - GlobalPosition;
		Vector2 desiredPosition = _playerCharacter.GlobalPosition - toPlayer.Normalized() * FollowDistance;

		Vector2 velocity = (desiredPosition - GlobalPosition) / (float)delta;

		// Limit the velocity to match the player's speed
		float currentSpeed = Speed * (Input.IsActionPressed("sprint") ? SprintMultiplier : 1.0f);
		velocity = velocity.LimitLength(currentSpeed);

		// Apply gravity
		if (!IsOnFloor())
		{
			velocity.Y += _gravity * (float)delta;
		}

		// Mimic jumping
		if (_playerCharacter.Velocity.Y < 0 && IsOnFloor())
		{
			velocity.Y = JumpVelocity;
		}

		Velocity = velocity;
		MoveAndSlide();
	}
}
