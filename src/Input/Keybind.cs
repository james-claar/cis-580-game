using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace cis_580_game;

/// <summary>
/// Represents a set of actions to take when a particular key/button is pressed
/// </summary>
public class Keybind
{
    private InputHandler _input;

    private HashSet<PressableGenericButton> _triggers = [];

    /// <summary>
    /// Triggered when the keybind changes state
    /// </summary>
    public event EventHandler StateChangedEvent;

    /// <summary>
    /// Triggered when the keybind is pressed
    /// </summary>
    public event EventHandler PressedEvent;

    /// <summary>
    /// Triggered when the keybind is released
    /// </summary>
    public event EventHandler ReleasedEvent;

    private bool _isCurrentlyPressed = false;

    /// <summary>
    /// Whether this keybind is currently triggered
    /// </summary>
    public bool IsCurrentlyPressed => _isCurrentlyPressed;

    /// <summary>
    /// Whether this keybind is a dummy keybind
    /// </summary>
    public bool IsDummyKeybind = false;

    /// <summary>
    /// Default constructor
    /// </summary>
    public Keybind(InputHandler input)
    {
        _input = input;
    }

    /// <summary>
    /// Adds a generic trigger
    /// </summary>
    /// <param name="buttons">Button(s) to add</param>
    public void AddTriggerButtons(params GenericButton[] buttons)
    {
        if (IsDummyKeybind) return;
        foreach (GenericButton button in buttons)
        {
            _triggers.Add(_input.PressableButtons[button]);
        }
    }

    /// <summary>
    /// Removes all triggers
    /// </summary>
    public void ClearTriggers()
    {
        _triggers.Clear();
    }

    /// <summary>
    /// Checks all triggers, and runs all actions if the keybind is triggered.
    /// </summary>
    /// <param name="gt">The GameTime</param>
    /// <returns>true if the keybind was triggered, false otherwise</returns>
    public bool Update(GameTime gt)
    {
        if (IsDummyKeybind) return false;
        bool pressed = _triggers.Any(button => button.IsPressed);
        if (pressed != _isCurrentlyPressed)
        {
            _isCurrentlyPressed = pressed;
            if (_isCurrentlyPressed)
                PressedEvent?.Invoke(this, EventArgs.Empty);
            else
                ReleasedEvent?.Invoke(this, EventArgs.Empty);
            StateChangedEvent?.Invoke(this, EventArgs.Empty);
            return true;
        }
        return false;
    }
}
