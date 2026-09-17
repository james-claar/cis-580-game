using System;
using System.Drawing;

static class RectangleFExtensions
{
    /// <summary>
    /// Returns a new rectangle
    /// </summary>
    /// <param name="s">The SpriteBatch to get the texture from</param>
    /// <returns>A 1x1 white pixel texture</returns>
    public static Microsoft.Xna.Framework.Rectangle ToRectangle(this RectangleF s)
    {
        return new Microsoft.Xna.Framework.Rectangle((int)Math.Round(s.X), (int)Math.Round(s.Y), (int)Math.Round(s.Width), (int)Math.Round(s.Height));
    }
}