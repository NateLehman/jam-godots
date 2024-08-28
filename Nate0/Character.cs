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
	private PackedScene _minionScene;

	[Export]
	private int _minionCount = 10;

	public bool HasDoubleJump => _hasDoubleJump;

	public override void _Ready()
	{
		_gravity = (float)ProjectSettings.GetSetting("physics/2d/default_gravity");
		_menu = GetNode<Menu>("Menu");
		SpawnMinions();
	}

	private void SpawnMinions()
	{
		if (_minionScene == null)
		{
			GD.PrintErr("Minion scene is not set!");
			return;
		}

		for (int i = 0; i < _minionCount; i++)
		{
			Minion minion = _minionScene.Instantiate() as Minion;
			if (minion != null)
			{
				var addChildAction = () => GetParent().AddChild(minion);
				Callable.From(addChildAction).CallDeferred();
				minion.Position = new Vector2(GD.RandRange(-100, 100), GD.RandRange(-100, 100));
				
				// Randomize minion properties for extra goofiness
				minion.JumpForce = (float)GD.RandRange(250, 350);
				minion.MoveSpeed = (float)GD.RandRange(80, 120);
				minion.MaxJumpInterval = (float)GD.RandRange(2, 4);
				minion.RotationSpeed = (float)GD.RandRange(3, 7);
				minion.CollisionRotationFactor = (float)GD.RandRange(0.05f, 0.15f);
			}
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
