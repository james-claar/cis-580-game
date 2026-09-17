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
    /// The aspect ratio of the virtual screen
    /// </summary>
    public static readonly float VirtualScreenAspectRatio = VirtualScreenWidth / VirtualScreenHeight;

    private float _windowAspectRatio;

    private RectangleF _windowBounds;
    public RectangleF _gameplayBounds;

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
        _gameplayBounds = CalculateGameplayRect();
        _scalingFactor = _gameplayBounds.Width / VirtualScreenWidth;

        // Calculate screen border boxes
        int verticalBarWidth = (int)(_gameplayBounds.Left - 1f);
        int horizontalBarHeight = (int)(_gameplayBounds.Top - 1f);
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
            (int)Math.Round(_gameplayBounds.Left + (sourceRect.Left * _scalingFactor)),
            (int)Math.Round(_gameplayBounds.Top + (sourceRect.Top * _scalingFactor)),
            (int)Math.Round(sourceRect.Width * _scalingFactor),
            (int)Math.Round(sourceRect.Height * _scalingFactor)
        );
    }

    /// <summary>
    /// Get the final destination rectangle from a source rectangle
    /// on the virtual screen.
    /// </summary>
    /// <param name="sourceRect">The virtual source rectangle</param>
    /// <returns>The final rectangle coordinates</returns>
    public Rectangle GetScaledRect(Rectangle sourceRect)
    {
        return new(
            (int)Math.Round(_gameplayBounds.Left + (sourceRect.Left * _scalingFactor)),
            (int)Math.Round(_gameplayBounds.Top + (sourceRect.Top * _scalingFactor)),
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
    public Vector2 GetScaledPos(Vector2 sourcePos)
    {
        return new(
            (int)Math.Round(_gameplayBounds.Left + (sourcePos.X * _scalingFactor)),
            (int)Math.Round(_gameplayBounds.Top + (sourcePos.Y * _scalingFactor))
        );
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
    public void Draw(Texture2D texture, Vector2 position, Rectangle? sourceRectangle, Color color, float rotation, Vector2 origin, float scale, SpriteEffects effects, float layerDepth)
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
    public void Draw(Texture2D texture, Rectangle destinationRectangle, Rectangle? sourceRectangle, Color color, float rotation, Vector2 origin, SpriteEffects effects, float layerDepth)
    {
        _spriteBatch.Draw(texture, GetScaledRect(destinationRectangle), sourceRectangle, color, rotation, origin, effects, layerDepth);
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
    public void Draw(Texture2D texture, Rectangle destinationRectangle, Rectangle? sourceRectangle, Color color)
    {
        _spriteBatch.Draw(texture, GetScaledRect(destinationRectangle), sourceRectangle, color);
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
    public void DrawString(SpriteFont spriteFont, string text, Vector2 position, Color color, float rotation, Vector2 origin, float scale, SpriteEffects effects, float layerDepth)
    {
        _spriteBatch.DrawString(spriteFont, text, GetScaledPos(position), color, rotation, origin, scale*_scalingFactor, effects, layerDepth);
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
    public void DrawString(SpriteFont spriteFont, string text, Vector2 position, Color color, float rotation, Vector2 origin, Vector2 scale, SpriteEffects effects, float layerDepth)
    {
        _spriteBatch.DrawString(spriteFont, text, GetScaledPos(position), color, rotation, origin, scale*_scalingFactor, effects, layerDepth);
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
    public void DrawString(SpriteFont spriteFont, string text, Vector2 position, Color color, float rotation, Vector2 origin, Vector2 scale, SpriteEffects effects, float layerDepth, bool rtl)
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
    public void DrawString(SpriteFont spriteFont, StringBuilder text, Vector2 position, Color color, float rotation, Vector2 origin, float scale, SpriteEffects effects, float layerDepth)
    {
        _spriteBatch.DrawString(spriteFont, text, GetScaledPos(position), color, rotation, origin, scale*_scalingFactor, effects, layerDepth);
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
    public void DrawString(SpriteFont spriteFont, StringBuilder text, Vector2 position, Color color, float rotation, Vector2 origin, Vector2 scale, SpriteEffects effects, float layerDepth)
    {
        _spriteBatch.DrawString(spriteFont, text, GetScaledPos(position), color, rotation, origin, scale*_scalingFactor, effects, layerDepth);
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
    public void DrawString(SpriteFont spriteFont, StringBuilder text, Vector2 position, Color color, float rotation, Vector2 origin, Vector2 scale, SpriteEffects effects, float layerDepth, bool rtl)
    {
        _spriteBatch.DrawString(spriteFont, text, GetScaledPos(position), color, rotation, origin, scale*_scalingFactor, effects, layerDepth, rtl);
    }

    // END SPRITEBATCH WRAPPER METHODS
}
