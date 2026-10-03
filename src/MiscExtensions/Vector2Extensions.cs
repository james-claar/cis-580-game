

using System;
using Microsoft.Xna.Framework;

namespace cis_580_game;

public static class Vector2Extensions
{
    /// <summary>
    /// Copies this Vector2 to a new one, with both components' absolute value
    /// </summary>
    /// <param name="source">Vector to take components from</param>
    /// <returns>A new Vector2 with both values converted to absolute-value</returns>
    public static Vector2 GetAbs(this Vector2 source)
    {
        return new(Math.Abs(source.X), Math.Abs(source.Y));
    }
}
