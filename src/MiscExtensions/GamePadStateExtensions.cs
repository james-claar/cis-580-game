using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;

namespace cis_580_game;

static class GamePadStateExtensions
{
    public static List<Buttons> GetPressedButtons(this GamePadState s)
    {
        List<Buttons> pressedButtons = [];
        foreach (Buttons button in Enum.GetValues<Buttons>())
        {
            if ((s.Buttons.Buttons & button) == button) pressedButtons.Add(button);
        }
        return pressedButtons;
    }
}
