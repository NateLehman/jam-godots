using Godot;
using System;

public partial class SpellParser : CanvasLayer
{
	private LineEdit parser;

	public bool HasFocus()
	{
		return parser.HasFocus();
	}
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		parser = GetNode<LineEdit>("LineEdit");
		parser.TextSubmitted += HandleSpellInput;
		Hide();
	}

	public override void _Input(InputEvent @event)
	{
		base._Input(@event);

		if (@event.IsActionPressed("begin_spell"))
		{
			Show();
			parser.GrabFocus();
		}
	}

	public void HandleSpellInput(string spellText)
	{
		parser.Clear();
		parser.ReleaseFocus();
		Hide();
		GD.Print("Spell: " + spellText);
	}
}
