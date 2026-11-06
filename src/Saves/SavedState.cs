
using System;
using Microsoft.Xna.Framework;

namespace cis_580_game;

public class SavedState
{
    public DateTime SavedAt { get; set; }

    public PlayerShip PlayerShip { get; set; }

    public SpritePool SpritePool { get; set; }
}
