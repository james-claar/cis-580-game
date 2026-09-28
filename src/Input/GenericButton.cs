using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Input;

namespace cis_580_game;

public enum ButtonType
{
    Keyboard,
    MouseButton,
    MouseScrollWheel,
    Gamepad,
    Wildcard
}

public enum WildcardButtons
{
    None,
    AllKeyboardKeys,
    AllMouseButtons,
    AllScrollWheelChanges,
    AllGamepadButtons,
    AllButtons, // = AllKeyboardKeys + AllMouseButtons + AllGamepadButtons
    AllPossibleInputs // = AllButtons + AllScrollWheelChanges
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
    protected readonly int _button;

    public GenericButton(Keys key)
    {
        Type = ButtonType.Keyboard;
        _button = (int)key;
    }

    public GenericButton(MouseButtons button)
    {
        Type = ButtonType.MouseButton;
        _button = (int)button;
    }

    public GenericButton(ScrollWheelChanges change)
    {
        Type = ButtonType.MouseScrollWheel;
        _button = (int)change;
    }

    public GenericButton(Buttons button)
    {
        Type = ButtonType.Gamepad;
        _button = (int)button;
    }

    public GenericButton(WildcardButtons button)
    {
        Type = ButtonType.Wildcard;
        _button = (int)button;
    }

    public GenericButton(GenericButton button)
    {
        Type = button.Type;
        _button = button._button;
    }

    protected GenericButton(ButtonType type, int button)
    {
        Type = type;
        _button = button;
    }

    /// <summary>
    /// Gets every unique generic button possible
    /// </summary>
    /// <returns>A list of all possible unique generic buttons</returns>
    public static IEnumerable<GenericButton> GetAllPossible()
    {
        foreach (Keys               button in Enum.GetValues<Keys>())               yield return (GenericButton)button;
        foreach (MouseButtons       button in Enum.GetValues<MouseButtons>())       yield return (GenericButton)button;
        foreach (ScrollWheelChanges button in Enum.GetValues<ScrollWheelChanges>()) yield return (GenericButton)button;
        foreach (Buttons            button in Enum.GetValues<Buttons>())            yield return (GenericButton)button;
        foreach (WildcardButtons    button in Enum.GetValues<WildcardButtons>())    yield return (GenericButton)button;
    }

    /// <summary>
    /// Returns whether this button matches another button (equality or the other is a matching wildcard)
    /// </summary>
    /// <param name="other">Button to check against</param>
    /// <returns>Whether this button matches part of the other button</returns>
    public bool IsPartOf(GenericButton other)
    {
        if (this == other) return true;
        else if (other.Type == ButtonType.Wildcard) return (WildcardButtons)other switch
        {
            WildcardButtons.AllPossibleInputs     => true,
            WildcardButtons.AllButtons            => Type != ButtonType.MouseScrollWheel,
            WildcardButtons.AllKeyboardKeys       => Type == ButtonType.Keyboard,
            WildcardButtons.AllMouseButtons       => Type == ButtonType.MouseButton,
            WildcardButtons.AllScrollWheelChanges => Type == ButtonType.MouseScrollWheel,
            WildcardButtons.AllGamepadButtons     => Type == ButtonType.Gamepad,
            _ => false
        };
        else return false;
    }

    /// <summary>
    /// Whether another button is part of this button
    /// </summary>
    /// <param name="other">Button to check with</param>
    /// <returns>Whether the other button is covered by this</returns>
    public bool Contains(GenericButton other)
    {
        return other.IsPartOf(this);
    }

    // Implicit operators to get types out of the method
    public static explicit operator Keys(GenericButton b)               => b.Type == ButtonType.Keyboard         ? (Keys)b._button               : Keys.None;
    public static explicit operator MouseButtons(GenericButton b)       => b.Type == ButtonType.MouseButton      ? (MouseButtons)b._button       : MouseButtons.None;
    public static explicit operator ScrollWheelChanges(GenericButton b) => b.Type == ButtonType.MouseScrollWheel ? (ScrollWheelChanges)b._button : ScrollWheelChanges.None;
    public static explicit operator Buttons(GenericButton b)            => b.Type == ButtonType.Gamepad          ? (Buttons)b._button            : Buttons.None;
    public static explicit operator WildcardButtons(GenericButton b)    => b.Type == ButtonType.Wildcard         ? (WildcardButtons)b._button    : WildcardButtons.None;

    // Implicit operators to get GenericButton instances from different types
    public static implicit operator GenericButton(Keys key)                  => new(key);
    public static implicit operator GenericButton(MouseButtons button)       => new(button);
    public static implicit operator GenericButton(ScrollWheelChanges change) => new(change);
    public static implicit operator GenericButton(Buttons button)            => new(button);
    public static implicit operator GenericButton(WildcardButtons wild)      => new(wild);

    public static bool operator ==(GenericButton a, GenericButton b)
    {
        return a.Type == b.Type && a._button == b._button;
    }

    public static bool operator !=(GenericButton a, GenericButton b)
    {
        return !(a == b);
    }

    public override bool Equals(object obj)
    {
        if (obj is GenericButton button)
        {
            return this == button;
        }

        return false;
    }

    public override int GetHashCode()
    {
        return Type.GetHashCode() + _button.GetHashCode();
    }

    public override string ToString()
    {
        string typeEnumName = Type switch
        {
            ButtonType.Keyboard         => "Keys",
            ButtonType.MouseButton      => "MouseButtons",
            ButtonType.MouseScrollWheel => "ScrollWheelChanges",
            ButtonType.Gamepad          => "Buttons",
            ButtonType.Wildcard         => "WildcardButtons",
            _                           => "Unknown?"
        };
        string typeEnumValue = Type switch
        {
            ButtonType.Keyboard         => Enum.GetName((Keys)_button),
            ButtonType.MouseButton      => Enum.GetName((MouseButtons)_button),
            ButtonType.MouseScrollWheel => Enum.GetName((ScrollWheelChanges)_button),
            ButtonType.Gamepad          => Enum.GetName((Buttons)_button),
            ButtonType.Wildcard         => Enum.GetName((WildcardButtons)_button),
            _                           => "Unknown?"
        };

        StringBuilder output = new();
        output.Append("(GenericButton)");
        output.Append(typeEnumName);
        output.Append(".");
        output.Append(typeEnumValue);

        return output.ToString();
    }
}
