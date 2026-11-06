using System;
using System.Security.Cryptography.X509Certificates;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace cis_580_game;

public struct Circle
{
    /// <summary>
    /// The center of the circle
    /// </summary>
    public Vector2 Center;

    /// <summary>
    /// The radius of the circle
    /// </summary>
    public float Radius;

    /// <summary>
    /// The square of the radius
    /// </summary>
    public float RadiusSquared
    {
        get => Radius * Radius;
        set => Radius = MathF.Sqrt(MathF.Abs(value));
    }

    /// <summary>
    /// The diameter of the circle
    /// </summary>
    public float Diameter
    {
        get => 2f*Radius;
        set => Radius = 0.5f*value;
    }

    /// <summary>
    /// Area of the circle
    /// </summary>
    public float Area
    {
        get => MathHelper.Pi*RadiusSquared;
        set => Radius = MathF.Sqrt(MathHelper.OneOverPi * MathF.Abs(value));
    }

    /// <summary>
    /// Circumference of the circle
    /// </summary>
    public float Circumference
    {
        get => MathHelper.TwoPi*Radius;
        set => Radius = MathHelper.OneOverTwoPi*value;
    }

    public RectangleF AABB => new(Center.X - Radius, Center.Y - Radius, Diameter, Diameter);

    /// <summary>
    /// Constructs a new circle
    /// </summary>
    /// <param name="center">The center</param>
    /// <param name="radius">The radius</param>
    public Circle(Vector2 center, float radius)
    {
        Center = center;
        Radius = radius;
    }

    /// <summary>
    /// Returns whether the circle contains a position
    /// </summary>
    /// <param name="position">Position to check</param>
    /// <param name="epsilon">Allowed positional error to be considered a collision</param>
    /// <returns>True if position is inside the circle, false otherwise</returns>
    public bool Contains(Vector2 position, float epsilon = 0f)
    {
        float allowed = Radius + epsilon;
        return (position - Center).LengthSquared() > allowed * allowed;
    }
}
