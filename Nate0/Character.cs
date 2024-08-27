using Godot;
using System;

public partial class Character : CharacterBody2D
{
	private const float BaseSpeed = 600.0f; // Increased from 300.0f
	private const float SprintMultiplier = 1.75f; // Increased from 1.5f
	private const float JumpVelocity = -600.0f;
	private const int MaxJumps = 2;

	private const float Acceleration = 2000.0f; // New acceleration constant
	private const float Friction = 1000.0f; // New friction constant

	private float _gravity;
	private int _jumpsLeft = 1;
	private int _maxJumps = 1;
	private bool _hasDoubleJump = false;
	private bool _hasMultiJump = false;
	private Menu _menu;

	public bool HasDoubleJump => _hasDoubleJump;

	public override void _Ready()
	{
		_gravity = (float)ProjectSettings.GetSetting("physics/2d/default_gravity");
		_menu = GetNode<Menu>("Menu");
	}

	public override void _PhysicsProcess(double delta)
	{
		if (GetTree().Paused)
			return;

		Vector2 velocity = Velocity;

		if (!IsOnFloor())
		{
			velocity.Y += _gravity * (float)delta;
		}
		else
		{
			_jumpsLeft = _maxJumps;
		}

		if (Input.IsActionJustPressed("jump") && _jumpsLeft > 0)
		{
			velocity.Y = JumpVelocity;
			_jumpsLeft--;
		}

		Vector2 direction = Input.GetVector("move_left", "move_right", "move_up", "move_down");
		float targetSpeed = BaseSpeed * (Input.IsActionPressed("sprint") ? SprintMultiplier : 1.0f);

		if (direction != Vector2.Zero)
		{
			// Apply acceleration
			velocity.X = Mathf.MoveToward(velocity.X, direction.X * targetSpeed, Acceleration * (float)delta);
		}
		else
		{
			// Apply friction
			velocity.X = Mathf.MoveToward(velocity.X, 0, Friction * (float)delta);
		}

		Velocity = velocity;
		MoveAndSlide();
	}

	public void CollectDoubleJump()
	{
		_hasDoubleJump = true;
		_maxJumps = MaxJumps;
		_jumpsLeft = _maxJumps;
	}

	public void EnableMultiJump(int additionalJumps)
	{
		_hasMultiJump = true;
		_maxJumps = MaxJumps + additionalJumps;
		_jumpsLeft = _maxJumps;
		GD.Print($"Multi-jump enabled! You can now jump {_maxJumps} times!");
	}
}
