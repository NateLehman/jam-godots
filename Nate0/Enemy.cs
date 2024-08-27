using Godot;
using System;

public partial class Enemy : CharacterBody2D
{
    public override void _Ready()
    {
    }

    public void Hit()
    {
        QueueFree();
    }
}