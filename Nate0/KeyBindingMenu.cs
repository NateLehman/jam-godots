using Godot;
using System;
using System.Collections.Generic;

public partial class KeyBindingMenu : CanvasLayer
{
    private Control menuControl;
    private Dictionary<string, Button> actionButtons = new Dictionary<string, Button>();
    private string currentAction = null;

    public override void _Ready()
    {
        menuControl = GetNode<Control>("Control");
        
        actionButtons["move_up"] = GetNode<Button>("Control/CenterContainer/VBoxContainer/GridContainer/MoveUpButton");
        actionButtons["move_down"] = GetNode<Button>("Control/CenterContainer/VBoxContainer/GridContainer/MoveDownButton");
        actionButtons["move_left"] = GetNode<Button>("Control/CenterContainer/VBoxContainer/GridContainer/MoveLeftButton");
        actionButtons["move_right"] = GetNode<Button>("Control/CenterContainer/VBoxContainer/GridContainer/MoveRightButton");

        UpdateButtonTexts();
    }

    private void UpdateButtonTexts()
    {
        foreach (var action in actionButtons.Keys)
        {
            var events = InputMap.ActionGetEvents(action);
            if (events.Count > 0 && events[0] is InputEventKey keyEvent)
            {
                actionButtons[action].Text = OS.GetKeycodeString(keyEvent.PhysicalKeycode);
            }
        }
    }

    private void _on_move_up_button_pressed()
    {
        WaitForInput("move_up");
    }

    private void _on_move_down_button_pressed()
    {
        WaitForInput("move_down");
    }

    private void _on_move_left_button_pressed()
    {
        WaitForInput("move_left");
    }

    private void _on_move_right_button_pressed()
    {
        WaitForInput("move_right");
    }

    private void WaitForInput(string action)
    {
        currentAction = action;
        actionButtons[action].Text = "Press any key...";
        GetViewport().SetInputAsHandled();
    }

    public override void _Input(InputEvent @event)
    {
        if (currentAction != null && @event is InputEventKey keyEvent && keyEvent.Pressed)
        {
            // Remove the old key binding
            InputMap.ActionEraseEvents(currentAction);

            // Add the new key binding
            InputMap.ActionAddEvent(currentAction, keyEvent);

            // Update the button text
            actionButtons[currentAction].Text = OS.GetKeycodeString(keyEvent.PhysicalKeycode);

            // Reset currentAction
            currentAction = null;

            // Stop processing input
            GetViewport().SetInputAsHandled();
        }
    }

    private void _on_back_button_pressed()
    {
        Hide();
        GetParent<Menu>().ShowMainMenu();
    }

    public void Show()
    {
        menuControl.Show();
        UpdateButtonTexts();
    }

    public void Hide()
    {
        menuControl.Hide();
    }
}