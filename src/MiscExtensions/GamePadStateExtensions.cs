using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;

namespace cis_580_game;

static class GamePadStateExtensions
{
    extension(GamePadState source)
    {
        public List<Buttons> GetPressedButtons()
        {
            List<Buttons> pressedButtons = [];
            foreach (Buttons button in Enum.GetValues<Buttons>())
            {
                if ((source.Buttons.Buttons & button) == button) pressedButtons.Add(button);
            }
            return pressedButtons;
        }
    }

}
