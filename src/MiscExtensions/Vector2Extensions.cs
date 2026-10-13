

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
        return new(MathF.Abs(source.X), MathF.Abs(source.Y));
    }

    /// <summary>
    /// Gets this vector's theta angle in a right-handed coordinate system
    /// </summary>
    /// <param name="source">Source vector</param>
    /// <returns>This vector's theta angle</returns>
    public static float AngleRH(this Vector2 source)
    {
        return MathF.Atan2(source.Y, source.X);
    }

    /// <summary>
    /// Gets the angle for a position to point at another position in a right-handed coordinate system
    /// </summary>
    /// <param name="sourcePos">Origin position to point from</param>
    /// <param name="pointingAtPos">Position to point at</param>
    /// <returns></returns>
    public static float AnglePointingAtRH(this Vector2 sourcePos, Vector2 pointingAtPos)
    {
        return (pointingAtPos - sourcePos).AngleRH();
    }

    /// <summary>
    /// Gets a copy of this vector with the same magnitude, pointing in the specified direction
    /// </summary>
    /// <param name="sourcePos">Origin vector</param>
    /// <param name="pointingAtPos">Direction to point</param>
    /// <returns></returns>
    public static Vector2 GetPointingInDirectionRH(this Vector2 source, float direction)
    {
        if (source == Vector2.Zero) return Vector2.Zero;

        Vector2 result = new(source.X, source.Y);
        result.Rotate(direction - source.AngleRH());
        return result;
    }

    /// <summary>
    /// Gets a copy of this vector with the same magnitude, pointing in the specified direction
    /// </summary>
    /// <param name="sourcePos">Origin vector</param>
    /// <param name="pointingAtPos">Direction to point</param>
    /// <returns></returns>
    public static Vector2 GetRotatedOntoLineRH(this Vector2 source, float direction)
    {
        Vector2 unitDirection = new(MathF.Cos(direction), MathF.Sin(direction));
        int scalarProjectionSign = MathF.Sign(Vector2.Dot(source, unitDirection));
        return (scalarProjectionSign >= 0) ? source.GetPointingInDirectionRH(direction) : source.GetPointingInDirectionRH(direction + MathHelper.Pi);
    }
}
