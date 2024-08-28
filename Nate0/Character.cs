using Godot;
using System;

public partial class Character : CharacterBody2D
{
	protected const float Speed = 300.0f;
	protected const float SprintMultiplier = 1.5f;
	protected const float JumpVelocity = -600.0f;
	protected const int MaxJumps = 2;

	protected float _gravity;
	protected int _jumpsLeft = 1;
	protected int _maxJumps = 1;
	protected bool _hasDoubleJump = false;
	protected bool _hasMultiJump = false;
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

		if (Input.IsActionJustPressed("jump") && _jumpsLeft > 0)
		{
			velocity.Y = JumpVelocity;
			_jumpsLeft--;
		}


		Vector2 direction = Input.GetVector("move_left", "move_right", "move_up", "move_down");
		float currentSpeed = Speed * (Input.IsActionPressed("sprint") ? SprintMultiplier : 1.0f);

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
