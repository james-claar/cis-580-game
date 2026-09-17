using System;
using System.Collections.Generic;

namespace cis_580_game;

public class KeybindEventArgs : EventArgs
{
    /// <summary>
    /// Whether the button was pressed (true) or released (false).
    /// Scroll wheel triggers are always considered pressed.
    /// </summary>
    public bool IsPressed { get; set; }

    /// <summary>
    /// Type of button that was pressed
    /// </summary>
    public ButtonType ButtonType { get; set; }
}

/// <summary>
/// Represents a set of actions to take when a particular key/button is pressed
/// </summary>
public class Keybind
{
    private HashSet<GenericButton> _buttons = [];

    public event EventHandler<KeybindEventArgs> TriggerEvent;

    public bool IsCurrentlyPressed { get; private set; }

    /// <summary>
    /// Default constructor
    /// </summary>
    public Keybind() {}

    /// <summary>
    /// Adds a generic trigger
    /// </summary>
    /// <param name="buttons">Button(s) to add</param>
    public void AddTriggerButtons(params GenericButton[] buttons)
    {
        foreach (GenericButton button in buttons) _buttons.Add(button);
    }

    /// <summary>
    /// Removes all triggers
    /// </summary>
    public void ClearTriggers()
    {
        _buttons.Clear();
    }

    /// <summary>
    /// Checks all triggers, and runs all actions if the keybind is triggered.
    /// </summary>
    /// <param name="input">The input handler class</param>
    /// <returns>true if the keybind was triggered, false otherwise</returns>
    public bool Process(InputHandler input)
    {
        bool wasPressed;
        bool isPressed;
        bool anyPressed = false;
        foreach (GenericButton button in _buttons)
        {
            wasPressed = false;
            isPressed = false;
            switch (button.Type)
            {
                case ButtonType.Keyboard:
                    wasPressed = input.PriorKeyboardState.IsKeyDown(button);
                    isPressed = input.CurrentKeyboardState.IsKeyDown(button);
                    break;
                case ButtonType.MouseButton:
                    wasPressed = input.PriorMouseState.IsButtonDown(button);
                    isPressed = input.CurrentMouseState.IsButtonDown(button);
                    break;
                case ButtonType.MouseScrollWheel:
                    wasPressed = false;

                    switch ((ScrollWheelChanges)button)
                    {
                        case ScrollWheelChanges.Up:
                            if (input.ScrollWheelBuffer > 0) isPressed = true;
                            break;
                        case ScrollWheelChanges.Down:
                            if (input.ScrollWheelBuffer < 0) isPressed = true;
                            break;
                        case ScrollWheelChanges.Left:
                            if (input.HorizontalScrollWheelBuffer < 0) isPressed = true;
                            break;
                        case ScrollWheelChanges.Right:
                            if (input.HorizontalScrollWheelBuffer > 0) isPressed = true;
                            break;
                    }
                    break;
                case ButtonType.Gamepad:
                    wasPressed = input.PriorGamePadState.IsButtonDown(button);
                    isPressed = input.CurrentGamePadState.IsButtonDown(button);
                    break;
            }
            if (isPressed)
            {
                anyPressed = true;
                IsCurrentlyPressed = true;
            }
            if (wasPressed != isPressed)
            {
                RunAllActions(isPressed, button.Type);
                return true;
            }
        }
        if (!anyPressed) IsCurrentlyPressed = false;
        return false;
    }

    private void RunAllActions(bool isKeyPressed, ButtonType inputType)
    {
        TriggerEvent?.Invoke(this, new KeybindEventArgs {IsPressed=isKeyPressed, ButtonType=inputType});
    }
}
