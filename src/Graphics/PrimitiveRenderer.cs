using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

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
    /// Draws a screen-aligned rectangle, with scaling using ScaledRenderer
    /// </summary>
    /// <param name="renderer">The renderer to use</param>
    /// <param name="gameTime">The game time</param>
    /// <param name="spriteBatch">The spritebatch to render with</param>
    /// <param name="rectangle">The destination rectangle to draw</param>
    /// <param name="color">The color to draw with</param>
    /// <param name="layerDepth">The layer depth to draw in</param>
    public static void DrawRectangle(ScaledRenderer renderer, GameTime gameTime, SpriteBatch spriteBatch, Rectangle rectangle, Color color, float layerDepth)
    {
        renderer.Draw(spriteBatch.BlankTexture(), rectangle, null, color, 0f, Vector2.Zero, SpriteEffects.None, layerDepth);
    }
}
