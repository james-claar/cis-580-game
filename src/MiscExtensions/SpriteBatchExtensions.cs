using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace cis_580_game;

static class SpriteBatchExtensions
{
    private static Texture2D _blankTexture;

    extension (SpriteBatch source)
    {
        public Texture2D BlankTexture
        {
            get
            {
                if (_blankTexture is null)
                {
                    // https://community.monogame.net/t/whats-the-simplest-way-to-draw-a-rectangular-outline-without-generating-the-texture/7818/5
                    _blankTexture = new Texture2D(source.GraphicsDevice, 1, 1);
                    _blankTexture.SetData(new[] {Color.White});
                }
                return _blankTexture;
            }
        }

        /// <summary>
        /// Submit a sprite for drawing in the current batch.
        /// </summary>
        /// <param name="texture">A texture.</param>
        /// <param name="destinationRectangle">The drawing bounds on screen.</param>
        /// <param name="sourceRectangle">An optional region on the texture which will be rendered. If null - draws full texture.</param>
        /// <param name="color">A color mask.</param>
        /// <param name="rotation">A rotation of this sprite, in radians.</param>
        /// <param name="origin">Center of the rotation. 0,0 by default.</param>
        /// <param name="effects">Modificators for drawing. Can be combined.</param>
        /// <param name="layerDepth">A depth of the layer of this sprite.</param>
        public void Draw(
            Texture2D texture,
            RectangleF destinationRectangle,
            Rectangle? sourceRectangle,
            Color color,
            float rotation,
            Vector2 origin,
            SpriteEffects effects,
            float layerDepth
        )
        {
            Vector2 newScale = new(
                destinationRectangle.Width / texture.Width,
                destinationRectangle.Height / texture.Height
            );
            source.Draw(
                texture,
                destinationRectangle.TopLeft,
                sourceRectangle,
                color,
                rotation,
                origin,
                newScale,
                effects,
                layerDepth
            );
        }

        /// <summary>
        /// Draws a primitive screen-aligned rectangle on the screen
        /// </summary>
        public void DrawPrimitiveRectangle(
            RectangleF destinationRectangle,
            Color color,
            float layerDepth
        )
        {
            source.Draw(source.BlankTexture, destinationRectangle, null, color, 0f, Vector2.Zero, SpriteEffects.None, layerDepth);
        }
    }
}
