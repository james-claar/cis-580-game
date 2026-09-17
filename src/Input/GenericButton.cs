using Microsoft.Xna.Framework.Input;

namespace cis_580_game;

public enum ButtonType
{
    Keyboard = 0,
    MouseButton = 1,
    MouseScrollWheel = 2,
    Gamepad = 3
}

/// <summary>
/// Polymorphic wrapper for all button types across mouse, keyboard, and controller
/// </summary>
public class GenericButton
{
    /// <summary>
    /// The button's type
    /// </summary>
    public ButtonType Type { get; private set; }


    /// <summary>
    /// The button to represent
    /// </summary>
    private readonly object _button;

    public GenericButton(Keys key)
    {
        Type = ButtonType.Keyboard;
        _button = key;
    }

    public GenericButton(MouseButtons button)
    {
        Type = ButtonType.MouseButton;
        _button = button;
    }

    public GenericButton(ScrollWheelChanges change)
    {
        Type = ButtonType.MouseScrollWheel;
        _button = change;
    }

    public GenericButton(Buttons button)
    {
        Type = ButtonType.MouseButton;
        _button = button;
    }

    // Implicit operators to get types out of the method
    public static implicit operator Keys(GenericButton genericButton) => (Keys)genericButton._button;
    public static implicit operator MouseButtons(GenericButton genericButton) => (MouseButtons)genericButton._button;
    public static implicit operator ScrollWheelChanges(GenericButton genericButton) => (ScrollWheelChanges)genericButton._button;
    public static implicit operator Buttons(GenericButton genericButton) => (GenericButton)genericButton._button;

    // Implicit operators to get GenericButton instances from different types
    public static implicit operator GenericButton(Keys key) => new(key);
    public static implicit operator GenericButton(MouseButtons button) => new(button);
    public static implicit operator GenericButton(ScrollWheelChanges change) => new(change);
    public static implicit operator GenericButton(Buttons button) => new(button);
}