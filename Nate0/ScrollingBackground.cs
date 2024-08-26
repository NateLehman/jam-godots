using Godot;
using System;

public partial class ScrollingBackground : ParallaxBackground
{
    [Export]
    public float ScrollSpeed { get; set; } = 100.0f;

    public override void _Process(double delta)
    {
        ScrollOffset = new Vector2(ScrollOffset.X - ScrollSpeed * (float)delta, ScrollOffset.Y);
    }
}