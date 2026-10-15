using System.Runtime.CompilerServices;
using Microsoft.Xna.Framework;

namespace cis_580_game;

static class PointExtensions
{
    extension (Point point)
    {
        /// <summary>
        /// Converts this point to a Vector2
        /// </summary>
        /// <returns>This point as a Vector2</returns>
        public Vector2 ToVector2()
        {
            return new Vector2(point.X, point.Y);
        }
    }
}
