using Godot;
using System;

public partial class Character : CharacterBody2D
{
	private const float Speed = 300.0f;
	private const float SprintMultiplier = 1.5f;
	private const float JumpVelocity = -600.0f;
	private const int MaxJumps = 2;

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

	public override void _Input(InputEvent @event)
	{
		if (@event.IsActionPressed("ui_cancel"))
		{
			_menu.ToggleMenu();
		}
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

		if (Input.IsActionJustPressed("ui_accept") && _jumpsLeft > 0)
		{
			velocity.Y = JumpVelocity;
			_jumpsLeft--;
		}


		Vector2 direction = Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down");
		float currentSpeed = Speed * (Input.IsKeyPressed(Key.Shift) ? SprintMultiplier : 1.0f);

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
