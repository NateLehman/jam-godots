using Godot;
using System;

public partial class TitleScreen : Control
{
    public override void _Ready()
    {
        var startButton = GetNode<Button>("StartButton");
        startButton.Pressed += OnStartButtonPressed;
    }

    private void OnStartButtonPressed()
    {
        GetTree().ChangeSceneToFile("res://PlatformScene.tscn");
    }
}