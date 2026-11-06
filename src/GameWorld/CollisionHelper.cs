// Modified from CollisionExercise in CIS 580

using System;
using System.Linq;
using Microsoft.Xna.Framework;

namespace cis_580_game;

public static class CollisionHelper
{
    private static bool GetUValues(Vector2 start1, Vector2 dir1, Vector2 start2, Vector2 dir2, out float u1, out float u2)
    {
        // https://gamedev.stackexchange.com/a/172154
        float ud = Vector2.Cross(dir1, dir2);

        // Segments are parallel
        if (ud == 0f)
        {
            u1 = float.NaN;
            u2 = float.NaN;
            return false;
        }

        Vector2 startsDiff = start2 - start1;

        u1 = Vector2.Cross(dir2, startsDiff) / ud;
        u2 = Vector2.Cross(dir1, startsDiff) / ud;

        return true;
    }

    public static bool Collides(Circle c1, Circle c2, float epsilon = 0f)
    {
        float combinedRadius = c1.Radius + c2.Radius + epsilon;
        return combinedRadius*combinedRadius >= (c1.Center - c2.Center).LengthSquared();
    }

    public static bool Collides(RectangleF r1, RectangleF r2, float epsilon)
    {
        return !(r1.Right + epsilon < r2.Left || r1.Left - epsilon > r2.Right ||
                    r1.Top - epsilon > r2.Bottom || r1.Bottom + epsilon < r2.Top);
    }

    public static bool Collides(Vector2 v1, Vector2 v2, float epsilon = 1f)
    {
        return Vector2.DistanceSquared(v1, v2) <= epsilon*epsilon;
    }

    public static bool Collides(Line l1, Line l2, float epsilon = 0f)
    {
        return l1.DoesIntersect(l2);
    }

    public static bool Collides(LineSegment s1, LineSegment s2, float epsilon = 0f)
    {
        throw new NotImplementedException();
    }

    public static bool DoesIntersect(Ray b1, Ray b2, float epsilon = 0f)
    {
        throw new NotImplementedException();
    }

    public static bool Collides(Circle c, RectangleF r, float epsilon = 0f)
    {
        Vector2 nearestPosition = new(
            MathHelper.Clamp(c.Center.X, r.Left, r.Right),
            MathHelper.Clamp(c.Center.Y, r.Top, r.Bottom)
        );
        return c.Contains(nearestPosition, epsilon);
    }
    public static bool Collides(RectangleF r, Circle c, float epsilon = 0f) => Collides(c, r, epsilon);

    public static bool Collides(Circle c, Vector2 v, float epsilon = 1f)
    {
        return c.Contains(v, epsilon);
    }
    public static bool Collides(Vector2 v, Circle c, float epsilon = 1f) => Collides(c, v, epsilon);

    public static bool Collides(RectangleF r, Vector2 v, float epsilon = 1f)
    {
        return r.Contains(v, epsilon);
    }
    public static bool Collides(Vector2 v, RectangleF r, float epsilon = 1f) => Collides(r, v, epsilon);

    public static bool Collides(Circle c, Line l, float epsilon = 0f)
    {
        return c.Contains(l.ClosestPointTo(c.Center), epsilon);
    }
    public static bool Collides(Line l, Circle c, float epsilon = 0f) => Collides(c, l, epsilon);

    public static bool Collides(RectangleF r, Line l, float epsilon = 0f)
    {
        return r.Segments.Any(s => Collides(l, s, epsilon));
    }
    public static bool Collides(Line l, RectangleF r, float epsilon = 0f) => Collides(r, l, epsilon);

    public static bool Collides(Vector2 v, Line l, float epsilon = 1f)
    {
        return Collides(v, l.ClosestPointTo(v), epsilon);
    }
    public static bool Collides(Line l, Vector2 v, float epsilon = 1f) => Collides(v, l, epsilon);

    public static bool Collides(Circle c, LineSegment s, float epsilon = 0f)
    {
        return c.Contains(s.ClosestPointTo(c.Center), epsilon);
    }
    public static bool Collides(LineSegment s, Circle c, float epsilon = 0f) => Collides(c, s, epsilon);

    public static bool Collides(RectangleF r, LineSegment s, float epsilon = 0f)
    {
        RectangleF segmentAABB = s.AABB;
        if (!Collides(segmentAABB, r, epsilon)) return false;
        if (r.Contains(segmentAABB, epsilon)) return true;
        return r.Segments.Any(s2 => Collides(s, s2, epsilon));
    }
    public static bool Collides(LineSegment s, RectangleF r, float epsilon = 0f) => Collides(r, s, epsilon);

    public static bool Collides(Vector2 v, LineSegment s, float epsilon = 1f)
    {
        return Collides(v, s.ClosestPointTo(v), epsilon);
    }
    public static bool Collides(LineSegment s, Vector2 v, float epsilon = 1f) => Collides(v, s, epsilon);

    public static bool Collides(Line l, LineSegment s, float epsilon = 0f)
    {
        float scaled_epsilon = epsilon * l.DistanceFormulaMultiplier;
        float d1 = l.GeneralFormula(s.Start);
        float d2 = l.GeneralFormula(s.End);
        return d1 >= -scaled_epsilon && d2 <= scaled_epsilon || d2 >= -scaled_epsilon && d1 <= scaled_epsilon;
    }
    public static bool Collides(LineSegment s, Line l, float epsilon = 0f) => Collides(l, s, epsilon);

    public static bool Collides(Circle c, Ray b, float epsilon = 0f)
    {
        return c.Contains(b.ClosestPointTo(c.Center), epsilon);
    }
    public static bool Collides(Ray b, Circle c, float epsilon = 0f) => Collides(c, b, epsilon);

    public static bool Collides(RectangleF r, Ray b, float epsilon = 0f)
    {
        throw new NotImplementedException();
    }
    public static bool Collides(Ray b, RectangleF r, float epsilon = 0f) => Collides(r, b, epsilon);

    public static bool Collides(Vector2 v, Ray b, float epsilon = 1f)
    {
        throw new NotImplementedException();
    }
    public static bool Collides(Ray b, Vector2 v, float epsilon = 1f) => Collides(v, b, epsilon);

    public static bool Collides(Line l, Ray b, float epsilon = 0f)
    {
        throw new NotImplementedException();
    }
    public static bool Collides(Ray b, Line l, float epsilon = 0f) => Collides(l, b, epsilon);

    public static bool Collides(LineSegment s, Ray b, float epsilon = 0f)
    {
        throw new NotImplementedException();
    }
    public static bool Collides(Ray b, LineSegment s, float epsilon = 0f) => Collides(s, b, epsilon);
}
