using System;
using System.Collections.Generic;
using System.Linq;

namespace cis_580_game;

public class PressableGenericButton : GenericButton
{
    protected bool _isPressed = false;

    /// <summary>
    /// Whether the button is currently pressed
    /// </summary>
    public bool IsPressed => _isPressed;

    public PressableGenericButton(GenericButton button) : base(button) {}

    /// <summary>
    /// The button is considered pressed if any possible input is triggered
    /// </summary>
    /// <param name="input"></param>
    public void CheckInput(InputHandler input)
    {
        if (!_isPressed)
        {
            bool pressed = input.NewlyPressedButtons.Any(Contains);
            if (pressed) _isPressed = true;
        }
        else
        {
            bool released = !input.CurrentPressedButtons.Any(Contains);
            if (released) _isPressed = false;
        }
    }
}
