using Godot;
using System;

public partial class Menu : CanvasLayer
{
	private Control menuControl;

	public override void _Ready()
	{
		menuControl = GetNode<Control>("Control");
		Hide();
	}

	public override void _Input(InputEvent @event)
	{
		if (@event.IsActionPressed("ui_cancel")) // ESC key
		{
			ToggleMenu();
		}
	}

	public void ToggleMenu()
	{
		menuControl.Visible = !menuControl.Visible;
		GetTree().Paused = menuControl.Visible;
	}
	
	public void OnResumeButtonPressed()
	{
		ToggleMenu();
	}
	
	public void OnTitleScreenButtonPressed()
	{
		GetTree().Paused = false;
		GetTree().ChangeSceneToFile("res://TitleScreen.tscn");
	}

	public void Hide()
	{
		menuControl.Hide();
	}

	public void Show()
	{
		menuControl.Show();
	}
}
