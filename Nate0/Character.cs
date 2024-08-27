using Godot;
using System;

public partial class Character : CharacterBody2D
{
	private const float BaseSpeed = 600.0f;
	private const float SprintMultiplier = 1.75f;
	private const float JumpVelocity = -600.0f;
	private const int MaxJumps = 2;

	private const float Acceleration = 2000.0f;
	private const float Friction = 1000.0f;

	private float _gravity;
	private int _jumpsLeft = 1;
	private int _maxJumps = 1;
	private bool _hasDoubleJump = false;
	private bool _hasMultiJump = false;
	private Menu _menu;

	[Export]
	public PackedScene ProjectileScene { get; set; }

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
			velocity.X = Mathf.MoveToward(velocity.X, direction.X * targetSpeed, Acceleration * (float)delta);
		}
		else
		{
			velocity.X = Mathf.MoveToward(velocity.X, 0, Friction * (float)delta);
		}

		Velocity = velocity;
		MoveAndSlide();

		// Handle shooting
		if (Input.IsActionJustPressed("character_shoot"))
		{
			Shoot();
		}
	}

	private void Shoot()
	{
		var projectile = ProjectileScene.Instantiate() as Projectile;
		GetParent().AddChild(projectile);
		Vector2 spawnPosition = GlobalPosition + new Vector2(20, 0); // Spawn slightly to the right
		projectile.GlobalPosition = spawnPosition;
		projectile.SetDirection(Vector2.Right);
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
