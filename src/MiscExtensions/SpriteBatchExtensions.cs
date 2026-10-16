using System;
using System.Reflection;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace cis_580_game;

static class SpriteBatchExtensions
{
    private static Texture2D _blankTexture;

    private static SpriteEffect _spriteEffect;


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
        /// Transformation matrix currently applied to this SpriteBatch
        /// </summary>
        public Matrix TransformMatrix
        {
            get
            {
                // Get _spriteEffect private field by reference. The reference does not change per SpriteBatch, so it works to just grab it once.
                _spriteEffect ??= (SpriteEffect)typeof(SpriteBatch).GetField("_spriteEffect", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(source);

                return _spriteEffect.TransformMatrix.GetValueOrDefault();
            }
        }

        /// <summary>
        /// Inverse of the transformation matrix currently applied to this SpriteBatch
        /// </summary>
        public Matrix InverseTransformMatrix => Matrix.Invert(source.TransformMatrix);

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
        /// Draws a string in a box on the virtual screen
        /// </summary>
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
        public void DrawStringInBoxVirtualScreen(SpriteFont font, string text, Color color, float defaultScale, SpriteEffects effects, float layerDepth, RectangleF box, Alignment textAlignment, float minWallOffset, bool allowWrapping = false, bool rtl = false)
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

            source.DrawString(font, text, new(scaledTextBounds.X, scaledTextBounds.Y), color, 0f, Vector2.Zero, defaultScale * defaultScaleMultiplier * Vector2.One, effects, layerDepth, rtl);
        }

        /// <summary>
        /// Draws a primitive screen-aligned rectangle on the screen
        /// </summary>
        public void DrawFilledRectangle(
            RectangleF destinationRectangle,
            Color color,
            float layerDepth
        )
        {
            source.Draw(source.BlankTexture, destinationRectangle, null, color, 0f, Vector2.Zero, SpriteEffects.None, layerDepth);
        }

        /// <summary>
        /// Draws a primitive rectangle on the screen
        /// </summary>
        public void DrawFilledRectangle(
            RectangleF destinationRectangle,
            Color color,
            float rotation,
            Vector2 origin,
            float layerDepth
        )
        {
            source.Draw(source.BlankTexture, destinationRectangle, null, color, rotation, origin, SpriteEffects.None, layerDepth);
        }

        /// <summary>
        /// Draws a primitive screen-aligned rectangle on the screen
        /// </summary>
        public void DrawFilledRectangle(
            Rectangle destinationRectangle,
            Color color,
            float rotation,
            Vector2 origin,
            float layerDepth
        )
        {
            source.Draw(source.BlankTexture, destinationRectangle, null, color, rotation, origin, SpriteEffects.None, layerDepth);
        }

        public void DrawFilledArc(
            Vector2 center,
            float radius,
            float startAngle,
            float arcSizeAngle,
            Color color,
            float layerDepth,
            int verticesPerFullCircle = 256
        )
        {
            if (arcSizeAngle == 0) return;
            if (arcSizeAngle < 0)
            {
                arcSizeAngle = MathF.Abs(arcSizeAngle);
                startAngle -= arcSizeAngle;
            }
            if (verticesPerFullCircle < 4) verticesPerFullCircle = 4;
            if (int.IsOddInteger(verticesPerFullCircle)) verticesPerFullCircle++;

            float maxRectArcSize = MathHelper.TwoPi / verticesPerFullCircle;

            float remainderArc = arcSizeAngle % maxRectArcSize;
            int numRects = (int)MathF.Round((arcSizeAngle - remainderArc)/maxRectArcSize);

            float halfSpanHeight = radius * MathF.Sin(maxRectArcSize);
            float halfSpanWidth = MathF.Sqrt(radius*radius - halfSpanHeight*halfSpanHeight);
            RectangleF span = new(
                center.X,
                center.Y,
                halfSpanWidth,
                2f*halfSpanHeight
            );
            float halfFinalSpanHeight = radius * MathF.Sin(remainderArc);
            float halfFinalSpanWidth = MathF.Sqrt(radius*radius - halfFinalSpanHeight*halfFinalSpanHeight);
            RectangleF finalSpan = new(
                center.X,
                center.Y,
                halfFinalSpanWidth,
                2f*halfFinalSpanHeight
            );
            Vector2 origin = new(0f, 0.5f);

            float rotation = startAngle + 0.5f*maxRectArcSize;
            for (int i = 0; i < numRects; i++)
            {
                source.DrawFilledRectangle(span, color, rotation, origin, layerDepth);
                rotation += maxRectArcSize;
            }
            rotation += -0.5f*maxRectArcSize + 0.5f*remainderArc;
            source.DrawFilledRectangle(finalSpan, color, rotation, origin, layerDepth);
        }

        /// <summary>
        /// Draws a filled circle.
        /// </summary>
        /// <param name="center"></param>
        /// <param name="radius"></param>
        /// <param name="color"></param>
        /// <param name="layerDepth"></param>
        /// <param name="numVertices"></param>
        public void DrawFilledCircle(
            Vector2 center,
            float radius,
            Color color,
            float layerDepth,
            int numVertices = 128
        )
        {
            if (numVertices < 4) numVertices = 4;
            if (int.IsOddInteger(numVertices)) numVertices++;

            int numRects = numVertices / 2;

            float arc = MathHelper.Pi / numRects;
            float halfSpanHeight = radius * MathF.Sin(0.5f*arc);
            float halfSpanWidth = MathF.Sqrt(radius*radius - halfSpanHeight*halfSpanHeight);
            RectangleF span = new(
                center.X,
                center.Y,
                2f*halfSpanWidth,
                2f*halfSpanHeight
            );
            Vector2 origin = new(0.5f, 0.5f);

            for (int i = 0; i < numRects; i++)
            {
                source.DrawFilledRectangle(span, color, i * arc, origin, layerDepth);
            }
        }
    }
}
