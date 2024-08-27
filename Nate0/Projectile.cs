using Godot;
using System;

public partial class Projectile : CharacterBody2D
{
	private const float Speed = 700.0f;
	private Vector2 direction = Vector2.Zero;
	private Camera2D camera;

	public override void _Ready()
	{
		direction = Vector2.Right;
		camera = GetViewport().GetCamera2D();
	}

	public override void _PhysicsProcess(double delta)
	{
		Vector2 velocity = direction * Speed;
		Velocity = velocity;
		MoveAndSlide();

		if (!IsInCameraView())
		{
			QueueFree();
		}
	}

	public void SetDirection(Vector2 newDirection)
	{
		direction = newDirection.Normalized();
	}

	private bool IsInCameraView()
	{
		if (camera == null)
		{
			return true; // Assume it's in view if we can't find the camera
		}

		Vector2 cameraPosition = camera.GetScreenCenterPosition();
		Vector2 viewportSize = GetViewportRect().Size;
		Rect2 cameraRect = new Rect2(cameraPosition - viewportSize / 2, viewportSize);
		Vector2 projectileSize = new Vector2(10, 10); // Adjust this based on your projectile's actual size
		Rect2 projectileRect = new Rect2(GlobalPosition - projectileSize / 2, projectileSize);

		bool isInView = cameraRect.Intersects(projectileRect);
		return isInView;
	}

	private void OnBodyEntered(Node2D body)
	{
		if (body is Enemy enemy)
		{
			enemy.Hit();
			QueueFree();
		}
	}
}
