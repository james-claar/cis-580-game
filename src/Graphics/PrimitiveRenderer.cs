using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using CollisionExercise.Collisions;

namespace CollisionExercise;

/// <summary>
/// A class used for drawing simple shapes
/// </summary>
public static class PrimitiveRenderer
{
    /// <summary>
    /// Draws a screen-aligned rectangle
    /// </summary>
    /// <param name="gameTime">The game time</param>
    /// <param name="spriteBatch">The spritebatch to render with</param>
    /// <param name="rectangle">The destination rectangle to draw</param>
    /// <param name="color">The color to draw with</param>
    public static void DrawRectangle(GameTime gameTime, SpriteBatch spriteBatch, Rectangle rectangle, Color color, float layerDepth)
    {
        spriteBatch.Draw(spriteBatch.BlankTexture(), rectangle, null, color, 0f, Vector2.Zero, SpriteEffects.None, layerDepth);
    }
}
