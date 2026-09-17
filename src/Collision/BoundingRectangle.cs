// Modified from CollisionExercise in CIS 580

using Microsoft.Xna.Framework;

namespace cis_580_game;

/// <summary>
/// A bounding rectangle for collision detection
/// </summary>
public struct BoundingRectangle
{
    public float X;

    public float Y;

    public float Width;

    public float Height;

    public float Left => X;

    public float Right => X + Width;

    public float Top => Y;

    public float Bottom => Y + Height;

    public BoundingRectangle(float x, float y, float width, float height)
    {
        X = x;
        Y = y;
        Width = width;
        Height = height;
    }

    public BoundingRectangle(Vector2 position, float width, float height)
    {
        X = position.X;
        Y = position.Y;
        Width = width;
        Height = height;
    }

    /// <summary>
    /// Tests for a collision between this and another bounding rectangle
    /// </summary>
    /// <param name="other">The other bounding rectangle</param>
    /// <returns>true for collision, false otherwise</returns>
    public bool CollidesWith(BoundingRectangle other)
    {
        return CollisionHelper.Collides(this, other);
    }
}
