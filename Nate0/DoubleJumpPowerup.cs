using Godot;
using System;

public partial class DoubleJumpPowerup : Area2D
{
	private void OnBodyEntered(Node2D body)
	{
		if (body is Character character)
		{
			character.CollectDoubleJump();
			QueueFree();
		}
	}
}
