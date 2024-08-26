using Godot;
using System;

public partial class Character : CharacterBody2D
{
	private const float Speed = 300.0f;
	private const float SprintMultiplier = 1.5f;
	private const float JumpVelocity = -400.0f;
	private const int MaxJumps = 2;

	private float _gravity;
	private int _jumpsLeft = 1;
	private bool _hasDoubleJump = false;

	public override void _Ready()
	{
		_gravity = (float)ProjectSettings.GetSetting("physics/2d/default_gravity");
	}

	public override void _PhysicsProcess(double delta)
	{
		Vector2 velocity = Velocity;

		if (!IsOnFloor())
		{
			velocity.Y += _gravity * (float)delta;
		}
		else
		{
			_jumpsLeft = _hasDoubleJump ? MaxJumps : 1;
		}

		if (Input.IsActionJustPressed("ui_accept") && _jumpsLeft > 0)
		{
			velocity.Y = JumpVelocity;
			_jumpsLeft--;
		}

		Vector2 direction = Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down");
		float currentSpeed = Speed * (Input.IsActionPressed("ui_select") ? SprintMultiplier : 1.0f);

		if (direction != Vector2.Zero)
		{
			velocity.X = direction.X * currentSpeed;
		}
		else
		{
			velocity.X = Mathf.MoveToward(velocity.X, 0, currentSpeed);
		}

		Velocity = velocity;
		MoveAndSlide();
	}

	public void CollectDoubleJump()
	{
		_hasDoubleJump = true;
		_jumpsLeft = MaxJumps;
	}
}
