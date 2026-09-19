using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Text;

namespace cis_580_game;

/// <summary>
/// Virtual renderer that automatically translates and scales shapes/text
/// to fit in a 16:9 space in the center of the window.
///
/// This is a wrapper for SpriteBatch.
/// </summary>
public class ScaledRenderer
{
    private SpriteBatch _spriteBatch;

    // The draw methods apply to a virtual 4K screen, which
    // is scaled down/up to the actual screen before
    // the rendering process.

    /// <summary>
    /// The width of the virtual screen.
    /// </summary>
    public static readonly float VirtualScreenWidth = 3840f;

    /// <summary>
    /// The height of the virtual screen
    /// </summary>
    public static readonly float VirtualScreenHeight = 2160f;

    /// <summary>
    /// The horizontal center of the virtual screen
    /// </summary>
    public static readonly float VirtualScreenHorizontalCenter = VirtualScreenWidth / 2f;

    /// <summary>
    /// The vertical center of the virtual screen
    /// </summary>
    public static readonly float VirtualScreenVerticalCenter = VirtualScreenHeight / 2f;

    /// <summary>
    /// The center of the virtual screen
    /// </summary>
    public static readonly Vector2 VirtualScreenCenter = new(VirtualScreenHorizontalCenter, VirtualScreenVerticalCenter);

    /// <summary>
    /// The aspect ratio of the virtual screen
    /// </summary>
    public static readonly float VirtualScreenAspectRatio = VirtualScreenWidth / VirtualScreenHeight;

    /// <summary>
    /// RectangleF representing the virtual screen
    /// </summary>
    public static readonly RectangleF VirtualScreenRectangle = new(0, 0, VirtualScreenWidth, VirtualScreenHeight);

    private float _windowAspectRatio;

    private RectangleF _windowBounds;
    public RectangleF GameplayBounds;

    private float _scalingFactor;

    /// <summary>
    /// Rectanglular blocks surrounding the game screen
    /// </summary>
    private Rectangle _leftBar;
    private Rectangle _rightBar;
    private Rectangle _topBar;
    private Rectangle _bottomBar;

    public ScaledRenderer(float windowWidth, float windowHeight, SpriteBatch spriteBatch)
    {
        _windowBounds = new();
        _spriteBatch = spriteBatch;
        _leftBar = new();
        _rightBar = new();
        _topBar = new();
        _bottomBar = new();
        SetWindowSize(windowWidth, windowHeight);
    }

    public void SetWindowSize(float windowWidth, float windowHeight)
    {
        _windowBounds.X = 0f;
        _windowBounds.Y = 0f;
        _windowBounds.Width = (float)Math.Round(windowWidth);
        _windowBounds.Height = (float)Math.Round(windowHeight);
        _windowAspectRatio = _windowBounds.Width / Math.Max(_windowBounds.Height, 1f);
        GameplayBounds = CalculateGameplayRect();
        _scalingFactor = GameplayBounds.Width / VirtualScreenWidth;
        if (_scalingFactor == 0) _scalingFactor = float.Epsilon; // Avoid division by zero

        // Calculate screen border boxes
        int verticalBarWidth = (int)(GameplayBounds.Left - 1f);
        int horizontalBarHeight = (int)(GameplayBounds.Top - 1f);
        _leftBar.X = 0;
        _leftBar.Y = 0;
        _leftBar.Width = verticalBarWidth;
        _leftBar.Height = (int)_windowBounds.Height;
        _rightBar.X = (int)_windowBounds.Width - verticalBarWidth;
        _rightBar.Y = 0;
        _rightBar.Width = verticalBarWidth;
        _rightBar.Height = (int)_windowBounds.Height;
        _topBar.X = 0;
        _topBar.Y = 0;
        _topBar.Width = (int)_windowBounds.Width;
        _topBar.Height = horizontalBarHeight;
        _bottomBar.X = 0;
        _bottomBar.Y = (int)_windowBounds.Height - horizontalBarHeight;
        _bottomBar.Width = (int)_windowBounds.Width;
        _bottomBar.Height = horizontalBarHeight;
    }

    /// <summary>
    /// Get the final destination rectangle from a source rectangle
    /// on the virtual screen.
    /// </summary>
    /// <param name="sourceRect">The virtual source rectangle</param>
    /// <returns>The final rectangle coordinates</returns>
    public Rectangle GetScaledRect(RectangleF sourceRect)
    {
        return new(
            (int)Math.Round(GameplayBounds.Left + (sourceRect.Left * _scalingFactor)),
            (int)Math.Round(GameplayBounds.Top + (sourceRect.Top * _scalingFactor)),
            (int)Math.Round(sourceRect.Width * _scalingFactor),
            (int)Math.Round(sourceRect.Height * _scalingFactor)
        );
    }

    /// <summary>
    /// Get the final destination position from a source position
    /// on the virtual screen.
    /// </summary>
    /// <param name="sourcePos">The virtual source position</param>
    /// <returns>The final position coordinates</returns>
    public Vector2 GetScaledPos(Vector2 sourcePos, SpriteEffects effects = SpriteEffects.None)
    {
        return new(
            GameplayBounds.Left + (sourcePos.X * _scalingFactor),
            GameplayBounds.Top + (sourcePos.Y * _scalingFactor)
        );
        // TODO: Implement SpriteEffects handling
    }

    /// <summary>
    /// Translates window-relative position to virtual screen position
    /// </summary>
    /// <param name="scaledPos">A position in the outer game window</param>
    /// <param name="effects">SpriteEffects to account for</param>
    /// <returns></returns>
    public Vector2 GetVirtualPosFromScaled(Vector2 scaledPos, SpriteEffects effects = SpriteEffects.None)
    {
        if (_scalingFactor == 0) return Vector2.Zero;

        return new(
            (scaledPos.X - GameplayBounds.Left) / _scalingFactor,
            (scaledPos.Y - GameplayBounds.Top) / _scalingFactor
        );
        // TODO: Implement SpriteEffects handling
    }

    /// <summary>
    /// Gets a bounding rectangle for axis-aligned text on the virtual screen
    /// </summary>
    /// <param name="sourcePos"></param>
    /// <param name="text"></param>
    /// <param name="font"></param>
    /// <param name="alignment"></param>
    /// <param name="effects"></param>
    /// <param name="rtl"></param>
    /// <returns></returns>
    public static RectangleF GetTextBoundingRectangle(Vector2 sourcePos, string text, SpriteFont font, Alignment alignment, float scale, SpriteEffects effects = SpriteEffects.None, bool rtl = false)
    {
        Vector2 size = font.MeasureString(text);
        Alignment newAlignment = (rtl ? new(Alignment.Flipped(alignment.Horizontal), alignment.Vertical) : alignment) * effects;
        return newAlignment.GetScaledRectangle(newAlignment.GetAlignedBoundingBox(sourcePos, size.X, size.Y), scale);
    }

    /// <summary>
    /// Draws the border bars around the game screen.
    /// </summary>
    /// <param name="gameTime"></param>
    /// <param name="color">The color of the bars</param>
    /// <param name="layerDepth">Layer depth to draw at</param>
    public void DrawScreenBorderBars(GameTime gameTime, Color color, float layerDepth)
    {
        if (_leftBar.Width > 0)
        {
            PrimitiveRenderer.DrawRectangle(gameTime, _spriteBatch, _leftBar, color, layerDepth);
            PrimitiveRenderer.DrawRectangle(gameTime, _spriteBatch, _rightBar, color, layerDepth);
        }
        if (_topBar.Height > 0)
        {
            PrimitiveRenderer.DrawRectangle(gameTime, _spriteBatch, _topBar, color, layerDepth);
            PrimitiveRenderer.DrawRectangle(gameTime, _spriteBatch, _bottomBar, color, layerDepth);
        }
    }

    private RectangleF CalculateGameplayRect()
    {
        // Calculate gameplay boundary
        if (_windowAspectRatio >= VirtualScreenAspectRatio)
        {
            // Window width is too high
            float screenWidth = _windowBounds.Height * VirtualScreenAspectRatio;
            return new(
                (float)Math.Round(0.5f * (_windowBounds.Width - screenWidth)),
                (float)Math.Round(0f),
                (float)Math.Round(screenWidth),
                (float)Math.Round(_windowBounds.Height)
            );
        }
        else
        {
            // Window height is too high
            float screenHeight = _windowBounds.Width / VirtualScreenAspectRatio;
            return new(
                (float)Math.Round(0f),
                (float)Math.Round(0.5f * (_windowBounds.Height - screenHeight)),
                (float)Math.Round(_windowBounds.Width),
                (float)Math.Round(screenHeight)
            );
        }
    }

    /// <summary>
    /// Draws a string in a box
    /// </summary>
    /// <param name="font">SpriteFont to draw using</param>
    /// <param name="text">String to draw to draw</param>
    /// <param name="color">Color of the string</param>
    /// <param name="defaultScale">Scaling to use if the text fits</param>
    /// <param name="effects">Effects to draw with</param>
    /// <param name="LayerDepth">Depth to draw at</param>
    /// <param name="box">Box to draw in</param>
    /// <param name="textAlignment">Text alignment inside box</param>
    /// <param name="minWallOffset">Minimum space between text and walls of box</param>
    /// <param name="allowWrapping">Whether to allow automatic text wrapping</param>
    /// <param name="rtl">Whether to render text right-to-left</param>
    public void DrawStringInBox(SpriteFont font, string text, Color color, float defaultScale, SpriteEffects effects, float layerDepth, RectangleF box, Alignment textAlignment, float minWallOffset, bool allowWrapping = false, bool rtl = false)
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

        RectangleF defaultTextBounds = GetTextBoundingRectangle(new(alignmentX, alignmentY), text, font, textAlignment, defaultScale, effects, rtl);
        float defaultScaleMultiplier = 1f;

        if (defaultTextBounds.Width > paddedBox.Width || defaultTextBounds.Height > paddedBox.Height)
        {
            // Text is too big to fit in box, scale it down or wrap it

            // TODO: Consider wrapping the text

            defaultScaleMultiplier = Math.Min(paddedBox.Width / defaultTextBounds.Width, paddedBox.Height / defaultTextBounds.Height);
        }

        RectangleF scaledTextBounds = textAlignment.GetScaledRectangle(defaultTextBounds, defaultScaleMultiplier);

        DrawString(font, text, new(scaledTextBounds.X, scaledTextBounds.Y), color, 0f, Vector2.Zero, defaultScale * defaultScaleMultiplier * Vector2.One, effects, layerDepth, rtl);
    }

    // BEGIN SPRITEBATCH WRAPPER METHODS

    //
    // Summary:
    //     Submit a sprite for drawing in the current batch.
    //
    // Parameters:
    //   texture:
    //     A texture.
    //
    //   position:
    //     The drawing location on screen.
    //
    //   sourceRectangle:
    //     An optional region on the texture which will be rendered. If null - draws full
    //     texture.
    //
    //   color:
    //     A color mask.
    //
    //   rotation:
    //     A rotation of this sprite.
    //
    //   origin:
    //     Center of the rotation. 0,0 by default.
    //
    //   scale:
    //     A scaling of this sprite.
    //
    //   effects:
    //     Modificators for drawing. Can be combined.
    //
    //   layerDepth:
    //     A depth of the layer of this sprite.
    //
    //   alignmnet:
    //     Text alignment
    public void Draw(Texture2D texture, Vector2 position, Rectangle? sourceRectangle, Color color, float rotation, Vector2 origin, Vector2 scale, SpriteEffects effects, float layerDepth)
    {
        _spriteBatch.Draw(texture, GetScaledPos(position), sourceRectangle, color, rotation, origin, scale*_scalingFactor, effects, layerDepth);
    }

    //
    // Summary:
    //     Submit a sprite for drawing in the current batch.
    //
    // Parameters:
    //   texture:
    //     A texture.
    //
    //   destinationRectangle:
    //     The drawing bounds on screen.
    //
    //   sourceRectangle:
    //     An optional region on the texture which will be rendered. If null - draws full
    //     texture.
    //
    //   color:
    //     A color mask.
    //
    //   rotation:
    //     A rotation of this sprite.
    //
    //   origin:
    //     Center of the rotation. 0,0 by default.
    //
    //   effects:
    //     Modificators for drawing. Can be combined.
    //
    //   layerDepth:
    //     A depth of the layer of this sprite.
    //
    //   alignmnet:
    //     Text alignment
    public void Draw(Texture2D texture, Rectangle destinationRectangle, Rectangle? sourceRectangle, Color color, float rotation, Vector2 origin, SpriteEffects effects, float layerDepth)
    {
        _spriteBatch.Draw(texture, GetScaledRect(destinationRectangle), sourceRectangle, color, rotation, origin, effects, layerDepth);
    }

    //
    // Summary:
    //     Submit a text string of sprites for drawing in the current batch.
    //
    // Parameters:
    //   spriteFont:
    //     A font.
    //
    //   text:
    //     The text which will be drawn.
    //
    //   position:
    //     The drawing location on screen.
    //
    //   color:
    //     A color mask.
    //
    //   rotation:
    //     A rotation of this string.
    //
    //   origin:
    //     Center of the rotation. 0,0 by default.
    //
    //   scale:
    //     A scaling of this string.
    //
    //   effects:
    //     Modificators for drawing. Can be combined.
    //
    //   layerDepth:
    //     A depth of the layer of this string.
    //
    //   rtl:
    //     Text is Right to Left.
    //
    //   alignmnet:
    //     Text alignment
    public void DrawString(SpriteFont spriteFont, string text, Vector2 position, Color color, float rotation, Vector2 origin, Vector2 scale, SpriteEffects effects, float layerDepth, bool rtl = false)
    {
        _spriteBatch.DrawString(spriteFont, text, GetScaledPos(position), color, rotation, origin, scale*_scalingFactor, effects, layerDepth, rtl);
    }

    //
    // Summary:
    //     Submit a text string of sprites for drawing in the current batch.
    //
    // Parameters:
    //   spriteFont:
    //     A font.
    //
    //   text:
    //     The text which will be drawn.
    //
    //   position:
    //     The drawing location on screen.
    //
    //   color:
    //     A color mask.
    //
    //   rotation:
    //     A rotation of this string.
    //
    //   origin:
    //     Center of the rotation. 0,0 by default.
    //
    //   scale:
    //     A scaling of this string.
    //
    //   effects:
    //     Modificators for drawing. Can be combined.
    //
    //   layerDepth:
    //     A depth of the layer of this string.
    //
    //   rtl:
    //     Text is Right to Left.
    //
    //   alignmnet:
    //     Text alignment
    public void DrawString(SpriteFont spriteFont, StringBuilder text, Vector2 position, Color color, float rotation, Vector2 origin, Vector2 scale, SpriteEffects effects, float layerDepth, bool rtl = false)
    {
        _spriteBatch.DrawString(spriteFont, text, GetScaledPos(position), color, rotation, origin, scale*_scalingFactor, effects, layerDepth, rtl);
    }

    // END SPRITEBATCH WRAPPER METHODS
}
