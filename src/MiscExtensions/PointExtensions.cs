using Microsoft.Xna.Framework;

namespace cis_580_game;

static class PointExtensions
{
    // Converts this point to a Vector2
    public static Vector2 ToVector2(this Point p)
    {
        return new Vector2(p.X, p.Y);
    }
}
