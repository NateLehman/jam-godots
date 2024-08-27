using Godot;
using System;

public partial class MultiJumpPowerup : Area2D
{
	[Export]
	public int AdditionalJumps { get; set; } = 3;

	private void OnBodyEntered(Node2D body)
	{
		if (body is Character character)
		{
			if (character.HasDoubleJump)
			{
				character.EnableMultiJump(AdditionalJumps);
				QueueFree();
			}
			else
			{
				GD.Print("You need the Double Jump power-up first!");
			}
		}
	}
}
