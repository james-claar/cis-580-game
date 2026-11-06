using System;
using Microsoft.Xna.Framework;

namespace cis_580_game;

public struct Line
{
    private static readonly Line horizontalThroughOrigin = new Line(0f, 1f, 0f);

    private static readonly Line verticalThroughOrigin = new Line(1f, 0f, 0f);

    private static readonly Line identity = new Line(-1f, 1f, 0f);

    /// <summary>
    /// A horizontal line through the origin
    /// </summary>
    public static Line HorizontalThroughOrigin => horizontalThroughOrigin;

    /// <summary>
    /// A vertical line through the origin
    /// </summary>
    public static Line VerticalThroughOrigin => verticalThroughOrigin;

    /// <summary>
    /// A line representing y = x
    /// </summary>
    public static Line Identity => identity;

    /// <summary>
    /// "A" coefficient in general equation Ax + By + C = 0
    /// </summary>
    public float A;

    /// <summary>
    /// "B" coefficient in general equation Ax + By + C = 0
    /// </summary>
    public float B;

    /// <summary>
    /// "C" coefficient in general equation Ax + By + C = 0
    /// </summary>
    public float C;

    /// <summary>
    /// In the specific equation y = mx + b, returns the value of m (which may be +infinity for vertical lines)
    /// </summary>
    public float Slope => DYOverDX;

    public float DYOverDX
    {
        get
        {
            if (B == 0) return float.PositiveInfinity;
            else return -A / B;
        }
    }

    public float DXOverDY
    {
        get
        {
            if (B == 0) return float.PositiveInfinity;
            else return -B / A;
        }
    }

    public float YIntercept
    {
        get
        {
            if (B == 0) return float.NaN;
            return -C / B;
        }
    }

    public float XIntercept
    {
        get
        {
            if (A == 0) return float.NaN;
            return -C / A;
        }
    }

    /// <summary>
    /// The angle of this line counterclockwise from +X axis
    /// </summary>
    public float Angle => MathF.Atan2(-A, B);

    /// <summary>
    /// A unit vector in the direction of this line
    /// </summary>
    public Vector2 UnitDirection => Vector2.Unit(Angle);

    /// <summary>
    /// Whether the line is invalid (contains every point)
    /// </summary>
    public bool IsInvalid => A == 0f && B == 0f;

    /// <summary>
    /// Whether the line is vertical
    /// </summary>
    public bool IsVertical => A != 0f && B == 0f;

    /// <summary>
    /// Whether the line is horizontal
    /// </summary>
    public bool IsHorizontal => A == 0f && B != 0f;

    /// <summary>
    /// = 1f / sqrt(A^2 + B^2)
    /// </summary>
    public float DistanceFormulaMultiplier => MathF.ReciprocalSqrtEstimate(A*A + B*B);

    private void SetFromPoints(Vector2 point1, Vector2 point2)
    {
        A = point2.Y - point1.Y;
        B = point2.X - point1.X;
        C = point1.Y * B - point1.X * A;
    }

    private void SetFromLine(Line line)
    {
        A = line.A;
        B = line.B;
        C = line.C;
    }

    /// <summary>
    /// Creates a line with the equation ax + by + c = 0
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <param name="c"></param>
    public Line(float a, float b, float c)
    {
        A = a;
        B = b;
        C = c;
        if (IsInvalid) SetFromLine(HorizontalThroughOrigin);
    }

    /// <summary>
    /// Creates a line through the point with an angle counterclockwise from the +X axis, in radians
    /// </summary>
    /// <param name="pointOnLine"></param>
    /// <param name="angle"></param>
    public Line(Vector2 pointOnLine, float angle)
    {
        SetFromPoints(pointOnLine, pointOnLine + Vector2.Unit(angle));
    }

    /// <summary>
    /// Creates a line that passes through two points. If the points are equal, returns a line following the X axis.
    /// </summary>
    /// <param name="point1"></param>
    /// <param name="point2"></param>
    public Line(Vector2 point1, Vector2 point2)
    {
        if (point1 == point2) SetFromPoints(point1, point1 + Vector2.UnitX);
        else SetFromPoints(point1, point2);
    }

    /// <summary>
    /// Returns the Y position for a given X. Returns float.NaN if the line is vertical.
    /// </summary>
    /// <param name="x">X position</param>
    /// <returns>Y position on the line, or NaN if the line is vertical</returns>
    public float Y(float x)
    {
        if (B == 0) return float.NaN;
        else return -(C + A*x) / B;
    }

    /// <summary>
    /// Returns the X position for a given Y. Returns float.NaN if the line is horizontal.
    /// </summary>
    /// <param name="y">Y position</param>
    /// <returns>X position on the line, or NaN if the line is horizontal</returns>
    public float X(float y)
    {
        if (A == 0) return float.NaN;
        else return -(C + B*y) / A;
    }

    /// <summary>
    /// Gets a line parallel to this one, that passes through a point
    /// </summary>
    /// <param name="point">Point the new line must pass through</param>
    /// <returns>A line parallel to this, passing through the point</returns>
    public Line ParallelThroughPoint(Vector2 point)
    {
        return new Line(
            A,
            B,
            -A*point.X - B*point.Y
        );
    }

    /// <summary>
    /// Gets a line perpendicular to this one, that passes through a point
    /// </summary>
    /// <param name="point">Point the new line must pass through</param>
    /// <returns>A line perpendicular to this, passing through the point</returns>
    public Line PerpendicularThroughPoint(Vector2 point)
    {
        return new Line(
            -A,
            B,
            A*point.X - B*point.Y
        );
    }

    /// <summary>
    /// Returns Ax + By + C evaluated at a position
    /// </summary>
    /// <param name="position">Position to evaluate</param>
    /// <returns>The result of Ax + By + C</returns>
    public float GeneralFormula(Vector2 position)
    {
        return A*position.X + B*position.Y + C;
    }

    /// <summary>
    /// Returns the distance from this line to a position
    /// </summary>
    /// <param name="position">Position to check from</param>
    /// <returns>The distance from this line to the position</returns>
    public float DistanceTo(Vector2 position)
    {
        return MathF.Abs(GeneralFormula(position) * DistanceFormulaMultiplier);
    }

    /// <summary>
    /// Returns whether this line intersects another line
    /// </summary>
    /// <param name="other">Line to check against</param>
    /// <returns>True if lines intersect, false if they are parallel</returns>
    public bool DoesIntersect(Line other)
    {
        return (A*other.B - other.A*B) != 0f;
    }

    /// <summary>
    /// Gets the intersection point between two lines. If the lines are parallel, both components will be NaN!
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    public Vector2 Intersection(Line other, out bool didIntersect)
    {
        float denominator = A*other.B - other.A*B;
        didIntersect = denominator != 0f;
        if (didIntersect)
        {
            float multiplier = 1f / denominator;
            return new(
                multiplier * (B*other.C - other.B*C),
                multiplier * (other.A*C - A*other.C)
            );
        }
        else return Vector2.NaN;
    }

    /// <summary>
    /// Gets the nearest point on the line to a point
    /// </summary>
    /// <param name="source">Source point</param>
    /// <returns>The nearest point on the line to the source</returns>
    public Vector2 ClosestPointTo(Vector2 source)
    {
        if (A == 0f && B == 0f) return Vector2.Zero;
        float t = (A*source.X + B*source.Y + C) / (A*A + B*B);
        return new(
            source.X - A*t,
            source.Y - B*t
        );
    }
}
