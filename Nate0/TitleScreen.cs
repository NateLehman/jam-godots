using Godot;
using System;

public partial class TitleScreen : Control
{
	private void OnStartButtonPressed()
	{
		GetTree().ChangeSceneToFile("res://PlatformScene.tscn");
	}
}
