

using System;
using System.Security.Cryptography.X509Certificates;
using Microsoft.Xna.Framework;

namespace cis_580_game;

public static class Vector2Extensions
{
    extension (Vector2 source)
    {
        /// <summary>
        /// Gets a new Vector2 from right-handed polar coordinates
        /// </summary>
        /// <param name="magnitude">Polar magnitude</param>
        /// <param name="angle">Polar direction</param>
        /// <returns>A new Vector2 with rectangular coordinates</returns>
        public static Vector2 FromPolar(float magnitude, float angle)
        {
            return new(
                magnitude * MathF.Cos(angle),
                magnitude * MathF.Sin(angle)
            );
        }

        /// <summary>
        /// Gets a new unit vector pointing in the specified direction
        /// </summary>
        /// <param name="angle">Angle of the vector</param>
        /// <returns>A unit vector with the specified angle</returns>
        public static Vector2 Unit(float angle)
        {
            return Vector2.FromPolar(1f, angle);
        }

        /// <summary>
        /// Normalizes an angle to the [0,2pi) range, or [-pi,pi) if symmetricOutput is true
        /// </summary>
        /// <param name="angle">Angle to normalize</param>
        /// <param name="symmetricOutput">If true, uses [-pi,pi) range instead of default [0,2pi)</param>
        /// <returns>Angle normalized to the output range</returns>
        public static float NormalizeAngle(float angle, bool symmetricOutput = false)
        {
            float normalized = (angle > 0 && angle < MathHelper.TwoPi) ? angle : angle - MathHelper.TwoPi*MathF.Floor(angle/MathHelper.TwoPi);
            if (symmetricOutput && normalized >= MathHelper.Pi) normalized -= MathHelper.TwoPi;
            return normalized;
        }

        /// <summary>
        /// A copy of this Vector2, with both components' absolute value
        /// </summary>
        public Vector2 AbsPos => new(MathF.Abs(source.X), MathF.Abs(source.Y));

        /// <summary>
        /// This vector's angle in a right-handed coordinate system
        /// </summary>
        public float Angle
        {
            get => MathF.Atan2(source.Y, source.X);
            set
            {
                float magnitude = source.Length();
                source.X = magnitude * MathF.Cos(value);
                source.Y = magnitude * MathF.Sin(value);
            }
        }

        /// <summary>
        /// Gets the angle for a position to point at another position in a right-handed coordinate system
        /// </summary>
        /// <param name="pointingAtPos">Position to point at</param>
        /// <returns></returns>
        public float AnglePointingAt(Vector2 pointingAtPos)
        {
            return (pointingAtPos - source).Angle;
        }

        /// <summary>
        /// Gets a copy of this vector with the same magnitude, pointing in the specified direction
        /// </summary>
        /// <param name="angle">Direction to point</param>
        /// <returns>A copy of the vector pointing in the new direction</returns>
        public Vector2 GetPointingInDirection(float angle)
        {
            if (source == Vector2.Zero) return Vector2.Zero;

            return Vector2.FromPolar(source.Length(), angle);
        }

        /// <summary>
        /// Gets a copy of this vector with the same magnitude, pointing in the specified direction
        /// </summary>
        /// <param name="angle">Line angle from X axis</param>
        /// <returns>A copy of this vector with the same magnitude, pointing in the specified direction</returns>
        public Vector2 GetRotatedOntoLine(float angle)
        {
            float normalizedRelativeAngle = Vector2.NormalizeAngle(angle - source.Angle, true);
            float newAngle = (MathF.Abs(normalizedRelativeAngle) < MathHelper.PiOver2) ? angle : angle + MathF.PI;
            return source.GetPointingInDirection(newAngle);
        }
    }
}
