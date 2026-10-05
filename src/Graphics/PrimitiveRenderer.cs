using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace cis_580_game;

/// <summary>
/// A class used for drawing simple shapes
/// </summary>
public static class PrimitiveRenderer
{
    /// <summary>
    /// Draws a screen-aligned rectangle, without scaling
    /// </summary>
    /// <param name="gameTime">The game time</param>
    /// <param name="spriteBatch">The spritebatch to render with</param>
    /// <param name="rectangle">The destination rectangle to draw</param>
    /// <param name="color">The color to draw with</param>
    /// <param name="layerDepth">The layer depth to draw in</param>
    public static void DrawRectangle(GameTime gameTime, SpriteBatch spriteBatch, Rectangle rectangle, Color color, float layerDepth)
    {
        spriteBatch.Draw(spriteBatch.BlankTexture(), rectangle, null, color, 0f, Vector2.Zero, SpriteEffects.None, layerDepth);
    }

    /// <summary>
    /// Draws a string in a box
    /// </summary>
    /// <param name="gt">GameTime to use</param>
    /// <param name="sb">SpriteBatch to use</param>
    /// <param name="font">SpriteFont to draw using</param>
    /// <param name="text">String to draw to draw</param>
    /// <param name="color">Color of the string</param>
    /// <param name="defaultScale">Scaling to use if the text fits</param>
    /// <param name="effects">Effects to draw with</param>
    /// <param name="layerDepth">Depth to draw at</param>
    /// <param name="box">Box to draw in</param>
    /// <param name="textAlignment">Text alignment inside box</param>
    /// <param name="minWallOffset">Minimum space between text and walls of box</param>
    /// <param name="allowWrapping">Whether to allow automatic text wrapping</param>
    /// <param name="rtl">Whether to render text right-to-left</param>
    public static void DrawStringInBox(GameTime gt, SpriteBatch sb, SpriteFont font, string text, Color color, float defaultScale, SpriteEffects effects, float layerDepth, RectangleF box, Alignment textAlignment, float minWallOffset, bool allowWrapping = false, bool rtl = false)
    {
        // Calculate constrained boundaries with wall offset
        RectangleF paddedBox = new(box.Left + minWallOffset, box.Top + minWallOffset, box.Width - 2*minWallOffset, box.Height - 2*minWallOffset);
        if (string.IsNullOrEmpty(text) || paddedBox.Width <= 0 || paddedBox.Height <= 0) return;

        float alignmentX = textAlignment.Horizontal switch
        {
            HorizontalAlignment.Left => paddedBox.Left,
            HorizontalAlignment.Center => paddedBox.Center.X,
            HorizontalAlignment.Right => paddedBox.Right,
            _ => paddedBox.Left
        };
        float alignmentY = textAlignment.Vertical switch
        {
            VerticalAlignment.Top => paddedBox.Top,
            VerticalAlignment.Center => paddedBox.Center.Y,
            VerticalAlignment.Bottom => paddedBox.Bottom,
            _ => paddedBox.Top
        };

        RectangleF defaultTextBounds = VirtualScreen.GetTextBoundingRectangle(new(alignmentX, alignmentY), text, font, textAlignment, defaultScale, effects, rtl);
        float defaultScaleMultiplier = 1f;

        if (defaultTextBounds.Width > paddedBox.Width || defaultTextBounds.Height > paddedBox.Height)
        {
            // Text is too big to fit in box, scale it down or wrap it

            // TODO: Consider wrapping the text

            defaultScaleMultiplier = MathF.Min(paddedBox.Width / defaultTextBounds.Width, paddedBox.Height / defaultTextBounds.Height);
        }

        RectangleF scaledTextBounds = textAlignment.GetScaledRectangle(defaultTextBounds, defaultScaleMultiplier);

        sb.DrawString(font, text, new(scaledTextBounds.X, scaledTextBounds.Y), color, 0f, Vector2.Zero, defaultScale * defaultScaleMultiplier * Vector2.One, effects, layerDepth, rtl);
    }
}
