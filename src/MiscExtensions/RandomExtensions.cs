
using System;
using Microsoft.Xna.Framework;

namespace cis_580_game;

public static class RandomExtensions
{
    extension(Random random)
    {
        /// <summary>
        /// Returns a random floating-point number that is greater than or equal to <paramref name="min"/>, and less than <paramref name="max"/>.
        /// </summary>
        /// <param name="min">The minimum end of the range</param>
        /// <param name="max">The maximum end of the range</param>
        /// <returns>A single-precision floating-point number between <paramref name="min"/> and <paramref name="max"/></returns>
        public float NextSingle(float min, float max)
        {
            return min + random.NextSingle() * (max - min);
        }

        /// <summary>
        /// Returns a unit vector pointing in a random direction.
        /// </summary>
        /// <returns>A unit vector in any direction</returns>
        public Vector2 NextVector2()
        {
            return random.NextVector2(0, MathHelper.TwoPi);
        }

        /// <summary>
        /// Returns a unit vector pointing in a random direction between two right-handed angles.
        /// <summary>
        /// </summary>
        /// <param name="minAngle">Minimum angle</param>
        /// <param name="maxAngle">Maximum angle</param>
        /// <returns>A unit vector pointing between the two angles</returns>
        public Vector2 NextVector2(float minAngle, float maxAngle)
        {
            return Vector2.UnitRH(random.NextSingle(minAngle, maxAngle));
        }

        /// <summary>
        /// Returns a position inside the bounds
        /// </summary>
        /// <param name="bounds">Bounds the position must fall in</param>
        /// <returns>A position inside the bounds</returns>
        public Vector2 NextVector2(RectangleF bounds)
        {
            return new(
                random.NextSingle(bounds.Left, bounds.Right),
                random.NextSingle(bounds.Top, bounds.Bottom)
            );
        }

        /// <summary>
        /// Generates a random number between 0.0 and 1.0, and returns true if the number is smaller than <paramref name="probability"/>.
        /// </summary>
        /// <param name="probability">Probability between 0.0 and 1.0</param>
        /// <returns>A boolean representing whether the chance occured.</returns>
        public bool Chance(float probability)
        {
            return random.NextSingle() < probability;
        }
    }
}
