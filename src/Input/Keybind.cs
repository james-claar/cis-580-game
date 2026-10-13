using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace cis_580_game;

public class KeybindEventArgs(Keybind sender, Resources resources)
{
    private Resources _resources = resources;

    /// <summary>
    /// The keybind that triggered the event
    /// </summary>
    public Keybind Sender = sender;

    /// <summary>
    /// Whether the keybind is currently pressed
    /// </summary>
    public bool IsPressed => Sender.IsCurrentlyPressed;

    /// <summary>
    /// Whether the keybind is a scroll wheel change
    /// </summary>
    public bool IsScrollWheel => Sender.LastTrigger is not null && Sender.LastTrigger.Type == ButtonType.MouseScrollWheel;

    /// <summary>
    /// Value used to multiply the effectiveness of the keybind, based on a scroll wheel change. Is equal to 1 if not a scroll wheel.
    /// </summary>
    public float ScrollWheelMultiplier
    {
        get
        {
            if (!IsScrollWheel) return 1f;

            return _resources.Input.ScrollWheelSensitivity * (ScrollWheelChanges)Sender.LastTrigger switch
            {
                ScrollWheelChanges.Up => _resources.Input.DeltaScrollWheel,
                ScrollWheelChanges.Down => -_resources.Input.DeltaScrollWheel,
                ScrollWheelChanges.Left => -_resources.Input.DeltaHorizontalScrollWheel,
                ScrollWheelChanges.Right => _resources.Input.DeltaHorizontalScrollWheel,
                _ => 1f
            };
        }
    }
}

/// <summary>
/// Represents a set of actions to take when a particular key/button is pressed
/// </summary>
public class Keybind
{
    private Resources _resources;

    private Dictionary<GenericButton, PressableGenericButton> _pressableButtons;

    private HashSet<PressableGenericButton> _triggers = [];

    /// <summary>
    /// Triggered when the keybind changes state
    /// </summary>
    public event EventHandler<KeybindEventArgs> StateChangedEvent;

    /// <summary>
    /// Triggered when the keybind is pressed
    /// </summary>
    public event EventHandler<KeybindEventArgs> PressedEvent;

    /// <summary>
    /// Triggered when the keybind is released
    /// </summary>
    public event EventHandler<KeybindEventArgs> ReleasedEvent;

    private bool _isCurrentlyPressed = false;

    /// <summary>
    /// Whether this keybind is currently triggered
    /// </summary>
    public bool IsCurrentlyPressed => _isCurrentlyPressed;

    /// <summary>
    /// The last trigger that caused an event
    /// </summary>
    public PressableGenericButton LastTrigger = null;

    /// <summary>
    /// Whether this keybind is a dummy keybind
    /// </summary>
    public bool IsDummyKeybind = false;

    /// <summary>
    /// Default constructor
    /// </summary>
    public Keybind(Resources resources, Dictionary<GenericButton, PressableGenericButton> pressableButtons = null)
    {
        _resources = resources;
        _pressableButtons = pressableButtons is not null ? pressableButtons : _resources.Input.PressableButtons;
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
            _triggers.Add(_pressableButtons[button]);
        }
    }

    /// <summary>
    /// Removes all triggers
    /// </summary>
    public void ClearTriggers()
    {
        LastTrigger = null;
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

        bool pressed = false;
        foreach (PressableGenericButton button in _triggers) if (button.IsPressed)
        {
            pressed = true;
            LastTrigger = button;
            break;
        }
        if (pressed != _isCurrentlyPressed)
        {
            _isCurrentlyPressed = pressed;
            if (_isCurrentlyPressed)
                PressedEvent?.Invoke(this, new(this, _resources));
            else
                ReleasedEvent?.Invoke(this, new(this, _resources));
            StateChangedEvent?.Invoke(this, new(this, _resources));
            return true;
        }
        return false;
    }
}
