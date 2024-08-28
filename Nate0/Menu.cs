using Godot;
using System;

public partial class Menu : CanvasLayer
{
	private Control menuControl;
	private KeyBindingMenu keyBindingMenu;

	public override void _Ready()
	{
		menuControl = GetNode<Control>("Control");
		keyBindingMenu = GD.Load<PackedScene>("res://KeyBindingMenu.tscn").Instantiate<KeyBindingMenu>();
		AddChild(keyBindingMenu);
		keyBindingMenu.Hide();
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

	private void _on_resume_button_pressed()
	{
		ToggleMenu();
	}

	private void _on_key_bindings_button_pressed()
	{
		menuControl.Hide();
		keyBindingMenu.Show();
	}

	private void _on_title_screen_button_pressed()
	{
		GetTree().Paused = false;
		GetTree().ChangeSceneToFile("res://TitleScreen.tscn");
	}

	public void ShowMainMenu()
	{
		keyBindingMenu.Hide();
		menuControl.Show();
	}

	public void Hide()
	{
		menuControl.Hide();
		keyBindingMenu.Hide();
	}

	public void Show()
	{
		menuControl.Show();
	}
}
