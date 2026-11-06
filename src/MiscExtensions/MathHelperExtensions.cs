using Microsoft.Xna.Framework;

public static class MathHelperExtensions
{
    extension(MathHelper)
    {
        /// <summary>
        /// = 1 / pi ~= 0.31831f
        /// </summary>
        public static float OneOverPi => 1f / MathHelper.Pi;

        /// <summary>
        /// = 1 / 2pi ~= 0.15915f
        /// </summary>
        public static float OneOverTwoPi => 1f / MathHelper.TwoPi;
    }

}
