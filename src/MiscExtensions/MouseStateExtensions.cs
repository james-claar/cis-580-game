using Microsoft.Xna.Framework.Input;

namespace cis_580_game;

public enum MouseButtons
{
    LeftButton = 0,
    MiddleButton = 1,
    RightButton = 2,
    SideButton1 = 3,
    SideButton2 = 4
}

public enum ScrollWheelChanges
{
    Up = 0,
    Down = 1,
    Left = 2,
    Right = 3
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
}
