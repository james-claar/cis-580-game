using System;
using Microsoft.Xna.Framework;

namespace cis_580_game;

public struct Ray
{
    /// <summary>
    /// Starting point of the ray
    /// </summary>
    public Vector2 Start;

    private float _angle;

    /// <summary>
    /// Angle of this ray
    /// </summary>
    public float Angle
    {
        get => _angle;
        set
        {
            _angle = value;
            _unitDirection = Vector2.Unit(value);
        }
    }

    private Vector2 _unitDirection;

    /// <summary>
    /// Unit vector in the direction of this ray
    /// </summary>
    public readonly Vector2 UnitDirection => _unitDirection;

    /// <summary>
    /// The line this ray falls on
    /// </summary>
    public Line Line => new(Start, Start + UnitDirection);

    public Ray(Vector2 start, float angle)
    {
        Start = start;
        Angle = angle;
    }

    public Vector2 ClosestPointTo(Vector2 point)
    {
        // Formula from https://dassencio.org/41
        float lambdaS = Vector2.Dot(point - Start, UnitDirection);
        if (lambdaS <= 0f) return Start;
        return point + lambdaS * UnitDirection;
    }
}
