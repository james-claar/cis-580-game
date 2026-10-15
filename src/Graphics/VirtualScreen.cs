
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace cis_580_game;

public class VirtualScreen
{
    private Resources _resources;

    /// <summary>
    /// The width of the virtual screen.
    /// </summary>
    public static readonly float Width = 3840f;

    /// <summary>
    /// The height of the virtual screen
    /// </summary>
    public static readonly float Height = 2160f;

    /// <summary>
    /// The horizontal center of the virtual screen
    /// </summary>
    public static readonly float HorizontalCenter = Width / 2f;

    /// <summary>
    /// The vertical center of the virtual screen
    /// </summary>
    public static readonly float VerticalCenter = Height / 2f;

    /// <summary>
    /// The center of the virtual screen
    /// </summary>
    public static readonly Vector2 Center = new(HorizontalCenter, VerticalCenter);

    /// <summary>
    /// The aspect ratio of the virtual screen
    /// </summary>
    public static readonly float AspectRatio = Width / Height;

    /// <summary>
    /// RectangleF representing the virtual screen
    /// </summary>
    public static readonly RectangleF Rectangle = new(0, 0, Width, Height);

    private float _windowAspectRatio = 1;
    private RectangleF _windowBounds = new(0,0,1,1);

    /// <summary>
    /// The real window boundary
    /// </summary>
    public RectangleF WindowBounds;

    /// <summary>
    /// The real boundary of the virtual screen
    /// </summary>
    public RectangleF ScreenBounds;

    private float _scalingFactor;

    /// <summary>
    /// The ratio of the virtual to real screen size
    /// </summary>
    public float ScalingFactor;

    /// <summary>
    /// The ratio of real to virtual screen size.
    /// </summary>

    private Matrix _transformationMatrix;

    /// <summary>
    /// Transformation matrix to map virtual points to real ones
    /// </summary>
    public Matrix TransformationMatrix => _transformationMatrix;

    private Matrix _inverseTransformationMatrix;

    /// <summary>
    /// Reverse transformation matrix to map real points to virtual ones
    /// </summary>
    public Matrix InverseTransformationMatrix => _inverseTransformationMatrix;

    /// <summary>
    /// Rectanglular blocks surrounding the game screen
    /// </summary>
    private Rectangle _leftBar;
    private Rectangle _rightBar;
    private Rectangle _topBar;
    private Rectangle _bottomBar;

    public VirtualScreen(Resources resources)
    {
        _resources = resources;
        _windowBounds = new();
        _leftBar = new();
        _rightBar = new();
        _topBar = new();
        _bottomBar = new();
        SetWindowSize(_resources.Game.Window.ClientBounds.Width, _resources.Game.Window.ClientBounds.Height);
        _resources.Game.Window.ClientSizeChanged += Window_ClientSizeChanged;
    }

    private void SetWindowSize(float windowWidth, float windowHeight)
    {
        // Calculate window bounds and scaling factor
        _windowBounds.X = 0f;
        _windowBounds.Y = 0f;
        _windowBounds.Width = (float)Math.Round(windowWidth);
        _windowBounds.Height = (float)Math.Round(windowHeight);
        _windowAspectRatio = _windowBounds.Width / Math.Max(_windowBounds.Height, 1f);

        RecalculateBorders();
    }

    public void RecalculateBorders()
    {
        ScreenBounds = CalculateGameplayRect();
        _scalingFactor = ScreenBounds.Width / Width;
        if (_scalingFactor == 0) _scalingFactor = float.Epsilon; // Avoid division by zero

        // Calculate screen border boxes
        int verticalBarWidth = (int)(ScreenBounds.Left - 1f);
        int horizontalBarHeight = (int)(ScreenBounds.Top - 1f);
        _leftBar = new()
        {
            X = 0,
            Y = 0,
            Width = verticalBarWidth,
            Height = (int)_windowBounds.Height
        };
        _rightBar = new()
        {
            X = (int)_windowBounds.Width - verticalBarWidth,
            Y = 0,
            Width = verticalBarWidth,
            Height = (int)_windowBounds.Height
        };
        _topBar = new()
        {
            X = 0,
            Y = 0,
            Width = (int)_windowBounds.Width,
            Height = horizontalBarHeight
        };
        _bottomBar = new()
        {
            X = 0,
            Y = (int)_windowBounds.Height - horizontalBarHeight,
            Width = (int)_windowBounds.Width,
            Height = horizontalBarHeight
        };

        // Create transformation matrices
        _transformationMatrix = Matrix.CreateScale(_scalingFactor) * Matrix.CreateTranslation(ScreenBounds.X, ScreenBounds.Y, 0);
        _inverseTransformationMatrix = Matrix.Invert(_transformationMatrix);
    }

    private RectangleF CalculateGameplayRect()
    {
        // Calculate gameplay boundary
        if (_windowAspectRatio >= AspectRatio)
        {
            // Window width is too high
            float screenWidth = _windowBounds.Height * AspectRatio;
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
            float screenHeight = _windowBounds.Width / AspectRatio;
            return new(
                (float)Math.Round(0f),
                (float)Math.Round(0.5f * (_windowBounds.Height - screenHeight)),
                (float)Math.Round(_windowBounds.Width),
                (float)Math.Round(screenHeight)
            );
        }
    }

    /// <summary>
    /// Get the final destination position from a source position
    /// on the virtual screen.
    /// </summary>
    /// <param name="sourcePos">The virtual source position</param>
    /// <returns>The final position coordinates</returns>
    public Vector2 GetScaledPos(Vector2 sourcePos)
    {
        return Vector2.Transform(sourcePos, TransformationMatrix);
    }

    /// <summary>
    /// Translates window-relative position to virtual screen position
    /// </summary>
    /// <param name="scaledPos">A position in the outer game window</param>
    /// <param name="effects">SpriteEffects to account for</param>
    /// <returns></returns>
    public Vector2 GetVirtualPosFromScaled(Vector2 scaledPos)
    {
        return Vector2.Transform(scaledPos, InverseTransformationMatrix);
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
    /// Event triggered when the user resizes the game window
    /// </summary>
    /// <param name="sender">The sender of the event</param>
    /// <param name="e">The event arguments</param>
    protected void Window_ClientSizeChanged(object sender, EventArgs e)
    {
        _resources.Game.Window.ClientSizeChanged -= Window_ClientSizeChanged;

        SetWindowSize(_resources.Game.Window.ClientBounds.Width, _resources.Game.Window.ClientBounds.Height);

        _resources.Game.Window.ClientSizeChanged += Window_ClientSizeChanged;
    }


    /// <summary>
    /// Draws the border bars around the game screen.
    /// </summary>
    /// <param name="gt">GameTime to use</param>
    /// <param name="sb">SpriteBatch to use</param>
    public void DrawBorderBars(GameTime gt, SpriteBatch sb)
    {
        if (_leftBar.Width > 0)
        {
            PrimitiveRenderer.DrawRectangle(_resources, gt, sb, _leftBar, GameColors.WindowBorderColor, Layers.ForcedFront);
            PrimitiveRenderer.DrawRectangle(_resources, gt, sb, _rightBar, GameColors.WindowBorderColor, Layers.ForcedFront);
        }
        if (_topBar.Height > 0)
        {
            PrimitiveRenderer.DrawRectangle(_resources, gt, sb, _topBar, GameColors.WindowBorderColor, Layers.ForcedFront);
            PrimitiveRenderer.DrawRectangle(_resources, gt, sb, _bottomBar, GameColors.WindowBorderColor, Layers.ForcedFront);
        }
    }
}
