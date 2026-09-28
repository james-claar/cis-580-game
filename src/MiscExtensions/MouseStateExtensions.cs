using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;

namespace cis_580_game;

public enum MouseButtons
{
    None,
    LeftButton,
    MiddleButton,
    RightButton,
    SideButton1,
    SideButton2
}

public enum ScrollWheelChanges
{
    None,
    Up,
    Down,
    Left,
    Right
}

static class MouseStateExtensions
{
    public static bool IsButtonDown(this MouseState s, MouseButtons button)
    {
        switch(button)
        {
            case MouseButtons.LeftButton:
                return s.LeftButton == ButtonState.Pressed;
            case MouseButtons.MiddleButton:
                return s.MiddleButton == ButtonState.Pressed;
            case MouseButtons.RightButton:
                return s.RightButton == ButtonState.Pressed;
            case MouseButtons.SideButton1:
                return s.XButton1 == ButtonState.Pressed;
            case MouseButtons.SideButton2:
                return s.XButton2 == ButtonState.Pressed;
            default:
                return false;
        }
    }

    public static List<MouseButtons> GetPressedButtons(this MouseState s)
    {
        List<MouseButtons> pressedButtons = [];
        if (s.LeftButton == ButtonState.Pressed)   pressedButtons.Add(MouseButtons.LeftButton);
        if (s.MiddleButton == ButtonState.Pressed) pressedButtons.Add(MouseButtons.MiddleButton);
        if (s.RightButton == ButtonState.Pressed)  pressedButtons.Add(MouseButtons.RightButton);
        if (s.XButton1 == ButtonState.Pressed)     pressedButtons.Add(MouseButtons.SideButton1);
        if (s.XButton2 == ButtonState.Pressed)     pressedButtons.Add(MouseButtons.SideButton2);
        return pressedButtons;
    }
}
