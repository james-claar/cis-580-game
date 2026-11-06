using System;
using Microsoft.Xna.Framework;

namespace cis_580_game;

public struct LineSegment
{
    /// <summary>
    /// First point of the segment
    /// </summary>
    public Vector2 Start;

    /// <summary>
    /// Second point of the segment
    /// </summary>
    public Vector2 End;

    /// <summary>
    /// The segment, relative to the start
    /// </summary>
    public Vector2 Difference => End - Start;

    /// <summary>
    /// The segment's length
    /// </summary>
    public float Length => Difference.Length();

    /// <summary>
    /// The square of this segment's length
    /// </summary>
    public float LengthSquared => Difference.LengthSquared();

    /// <summary>
    /// The angle of this segment, measured counterclockwise from the +X axis
    /// </summary>
    public float Angle => Difference.Angle;

    /// <summary>
    /// The line this segment falls on
    /// </summary>
    public Line Line => new(Start, End);

    public RectangleF AABB => new(
        MathF.Min(Start.X, End.X),
        MathF.Min(Start.Y, End.Y),
        MathF.Abs(Difference.X),
        MathF.Abs(Difference.Y)
    );

    public LineSegment(Vector2 start, Vector2 end)
    {
        Start = start;
        End = end;
    }

    public Vector2 ClosestPointTo(Vector2 point)
    {
        // Formula from https://dassencio.org/41
        float lambdaS = Vector2.Dot(point - Start, Difference) / Difference.LengthSquared();
        if (lambdaS <= 0f) return Start;
        if (lambdaS >= 1f) return End;
        return point + lambdaS * Difference;
    }
}
