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
    extension (MouseState source)
    {
        public bool IsButtonDown(MouseButtons button)
        {
            switch(button)
            {
                case MouseButtons.LeftButton:
                    return source.LeftButton == ButtonState.Pressed;
                case MouseButtons.MiddleButton:
                    return source.MiddleButton == ButtonState.Pressed;
                case MouseButtons.RightButton:
                    return source.RightButton == ButtonState.Pressed;
                case MouseButtons.SideButton1:
                    return source.XButton1 == ButtonState.Pressed;
                case MouseButtons.SideButton2:
                    return source.XButton2 == ButtonState.Pressed;
                default:
                    return false;
            }
        }

        public List<MouseButtons> GetPressedButtons()
        {
            List<MouseButtons> pressedButtons = [];
            if (source.LeftButton == ButtonState.Pressed)   pressedButtons.Add(MouseButtons.LeftButton);
            if (source.MiddleButton == ButtonState.Pressed) pressedButtons.Add(MouseButtons.MiddleButton);
            if (source.RightButton == ButtonState.Pressed)  pressedButtons.Add(MouseButtons.RightButton);
            if (source.XButton1 == ButtonState.Pressed)     pressedButtons.Add(MouseButtons.SideButton1);
            if (source.XButton2 == ButtonState.Pressed)     pressedButtons.Add(MouseButtons.SideButton2);
            return pressedButtons;
        }
    }
}
