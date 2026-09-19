
using System.Runtime.Serialization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace cis_580_game;

/// <summary>
/// Represents a horizontal alignment setting
/// - Left alignment places the left edge of the bounding box at a position
/// - Center alignment places the center of the bounding box at a position
/// - Right alignment places the right edge of the bounding box at a position
/// </summary>
public enum HorizontalAlignment
{
    Left = 0,
    Center = 1,
    Right = 2
}

/// <summary>
/// Represents a horizontal alignment setting
/// - Top alignment places the top edge of the bounding box at a position
/// - Center alignment places the center of the bounding box at a position
/// - Bottom alignment places the bottom edge of the bounding box at a position
/// </summary>
public enum VerticalAlignment
{
    Top = 0,
    Center = 1,
    Bottom = 2
}


public struct Alignment
{

    /// <summary>
    /// The horizontal alignment of an element
    /// </summary>
    public HorizontalAlignment Horizontal = HorizontalAlignment.Left;

    /// <summary>
    /// The vertical alignment of an element
    /// </summary>
    public VerticalAlignment Vertical = VerticalAlignment.Top;

    public Alignment() {}

    public Alignment(HorizontalAlignment h)
    {
        Horizontal = h;
    }

    public Alignment(VerticalAlignment v)
    {
        Vertical = v;
    }

    public Alignment(HorizontalAlignment h, VerticalAlignment v)
    {
        Horizontal = h;
        Vertical = v;
    }

    public static readonly Alignment _defaultAlignment = new();

    public static readonly Alignment _trueCenteredAlignment = new(HorizontalAlignment.Center, VerticalAlignment.Center);

    /// <summary>
    /// Creates the default alignment
    /// </summary>
    public static Alignment Default => _defaultAlignment;

    /// <summary>
    /// Creates an alignment centered both horizontally and vertically
    /// </summary>
    public static Alignment TrueCentered => _trueCenteredAlignment;

    /// <summary>
    /// Gets the X position of the left side of an element
    /// </summary>
    /// <param name="alignmentX">The X position to align to</param>
    /// <param name="width">The width of the element</param>
    /// <returns>The X position of the left of the element</returns>
    public int GetLeftX(int alignmentX, int width)
    {
        return Horizontal switch
        {
            HorizontalAlignment.Left => alignmentX,
            HorizontalAlignment.Center => alignmentX - (width / 2),
            HorizontalAlignment.Right => alignmentX - width,
            _ => alignmentX,
        };
    }

    /// <summary>
    /// Gets the X position of the left side of an element
    /// </summary>
    /// <param name="alignmentX">The X position to align to</param>
    /// <param name="width">The width of the element</param>
    /// <returns>The X position of the left of the element</returns>
    public float GetLeftX(float alignmentX, float width)
    {
        return Horizontal switch
        {
            HorizontalAlignment.Left => alignmentX,
            HorizontalAlignment.Center => alignmentX - (width / 2),
            HorizontalAlignment.Right => alignmentX - width,
            _ => alignmentX,
        };
    }

    /// <summary>
    /// Gets the Y position of the top side of an element
    /// </summary>
    /// <param name="alignmentY">The Y position to align to</param>
    /// <param name="height">The height of the element</param>
    /// <returns>The Y position of the top of the element</returns>
    public int GetTopY(int alignmentY, int height)
    {
        return Vertical switch
        {
            VerticalAlignment.Top => alignmentY,
            VerticalAlignment.Center => alignmentY - (height / 2),
            VerticalAlignment.Bottom => alignmentY - height,
            _ => alignmentY,
        };
    }

    /// <summary>
    /// Gets the Y position of the top side of an element
    /// </summary>
    /// <param name="alignmentY">The Y position to align to</param>
    /// <param name="height">The height of the element</param>
    /// <returns>The Y position of the top of the element</returns>
    public float GetTopY(float alignmentY, float height)
    {
        return Vertical switch
        {
            VerticalAlignment.Top => alignmentY,
            VerticalAlignment.Center => alignmentY - (height / 2),
            VerticalAlignment.Bottom => alignmentY - height,
            _ => alignmentY,
        };
    }

    /// <summary>
    /// Gets the top left position of an element
    /// </summary>
    /// <param name="alignmentPosition">Position to align to</param>
    /// <param name="width">Width of the element</param>
    /// <param name="height">Height of the element</param>
    /// <returns>The top left position of an element</returns>
    public Point GetTopLeftPosition(Point alignmentPosition, int width, int height)
    {
        return new(GetLeftX(alignmentPosition.X, width), GetTopY(alignmentPosition.Y, height));
    }

    /// <summary>
    /// Gets the top left position of an element
    /// </summary>
    /// <param name="alignmentPosition">Position to align to</param>
    /// <param name="elementBoundingBox">Bounding box of the element</param>
    /// <returns>The top left position of an element</returns>
    public Point GetTopLeftPosition(Point alignmentPosition, Rectangle elementBoundingBox)
    {
        return new(GetLeftX(alignmentPosition.X, elementBoundingBox.Width), GetTopY(alignmentPosition.Y, elementBoundingBox.Height));
    }

    /// <summary>
    /// Gets the top left position of an element
    /// </summary>
    /// <param name="alignmentPosition">Position to align to</param>
    /// <param name="width">Width of the element</param>
    /// <param name="height">Height of the element</param>
    /// <returns>The top left position of an element</returns>
    public Vector2 GetTopLeftPosition(Vector2 alignmentPosition, float width, float height)
    {
        return new(GetLeftX(alignmentPosition.X, width), GetTopY(alignmentPosition.Y, height));
    }

    /// <summary>
    /// Gets the top left position of an element
    /// </summary>
    /// <param name="alignmentPosition">Position to align to</param>
    /// <param name="elementBoundingBox">Bounding box of the element</param>
    /// <returns>The top left position of an element</returns>
    public Vector2 GetTopLeftPosition(Vector2 alignmentPosition, RectangleF elementBoundingBox)
    {
        return new(GetLeftX(alignmentPosition.X, elementBoundingBox.Width), GetTopY(alignmentPosition.Y, elementBoundingBox.Height));
    }

    /// <summary>
    /// Gets a bounding box aligned to a position
    /// </summary>
    /// <param name="alignmentPosition">Position to align to</param>
    /// <param name="elementBoundingBox">Bounding box of the element</param>
    /// <returns>Aligned bounding box</returns>
    public Rectangle GetAlignedBoundingBox(Point alignmentPosition, Rectangle elementBoundingBox)
    {
        return new(GetLeftX(alignmentPosition.X, elementBoundingBox.Width), GetTopY(alignmentPosition.Y, elementBoundingBox.Height), elementBoundingBox.Width, elementBoundingBox.Height);
    }

    /// <summary>
    /// Gets a bounding box aligned to a position
    /// </summary>
    /// <param name="alignmentPosition">Position to align to</param>
    /// <param name="width">Width of the element</param>
    /// <param name="height">Height of the element</param>
    /// <returns>Aligned bounding box</returns>
    public Rectangle GetAlignedBoundingBox(Point alignmentPosition, int width, int height)
    {
        return new(GetLeftX(alignmentPosition.X, width), GetTopY(alignmentPosition.Y, height), width, height);
    }

    /// <summary>
    /// Gets a bounding box aligned to a position
    /// </summary>
    /// <param name="alignmentPosition">Position to align to</param>
    /// <param name="elementBoundingBox">Bounding box of the element</param>
    /// <returns>Aligned bounding box</returns>
    public RectangleF GetAlignedBoundingBox(Vector2 alignmentPosition, RectangleF elementBoundingBox)
    {
        return new(GetLeftX(alignmentPosition.X, elementBoundingBox.Width), GetTopY(alignmentPosition.Y, elementBoundingBox.Height), elementBoundingBox.Width, elementBoundingBox.Height);
    }

    /// <summary>
    /// Gets a bounding box aligned to a position
    /// </summary>
    /// <param name="alignmentPosition">Position to align to</param>
    /// <param name="width">Width of the element</param>
    /// <param name="height">Height of the element</param>
    /// <returns>Aligned bounding box</returns>
    public RectangleF GetAlignedBoundingBox(Vector2 alignmentPosition, float width, float height)
    {
        return new(GetLeftX(alignmentPosition.X, width), GetTopY(alignmentPosition.Y, height), width, height);
    }

    /// <summary>
    /// Creates a scaled copy of a rectangle, while keeping it aligned
    /// </summary>
    /// <param name="rectangle">Rectangle to change the scale of</param>
    /// <param name="scale">Scaling factor</param>
    /// <returns>A new scaled rectangle</returns>
    public RectangleF GetScaledRectangle(RectangleF rectangle, float scale)
    {
        if (scale == 1f) return new(rectangle.X, rectangle.Y, rectangle.Width, rectangle.Height);

        float newWidth = rectangle.Width * scale;
        float newHeight = rectangle.Height * scale;

        float newLeftX = Horizontal switch
        {
            HorizontalAlignment.Left => rectangle.Left,
            HorizontalAlignment.Center => rectangle.Left - (newWidth - rectangle.Width) / 2,
            HorizontalAlignment.Right => rectangle.Left - (newWidth - rectangle.Width),
            _ => rectangle.Left
        };
        float newTopY = Vertical switch
        {
            VerticalAlignment.Top => rectangle.Top,
            VerticalAlignment.Center => rectangle.Top - (newHeight - rectangle.Height) / 2,
            VerticalAlignment.Bottom => rectangle.Top - (newHeight - rectangle.Height),
            _ => rectangle.Top
        };

        return new(newLeftX, newTopY, newWidth, newHeight);
    }

    /// <summary>
    /// Flips a horizontal alignment
    /// </summary>
    /// <param name="h">Alignment to flip</param>
    /// <returns>Flipped horizontal alignment</returns>
    public static HorizontalAlignment Flipped(HorizontalAlignment h)
    {
        return h switch
        {
            HorizontalAlignment.Left => HorizontalAlignment.Right,
            HorizontalAlignment.Center => HorizontalAlignment.Center,
            HorizontalAlignment.Right => HorizontalAlignment.Left,
            _ => HorizontalAlignment.Left
        };
    }

    /// <summary>
    /// Flips a vertical alignment
    /// </summary>
    /// <param name="h">Alignment to flip</param>
    /// <returns>Flipped vertical alignment</returns>
    public static VerticalAlignment Flipped(VerticalAlignment v)
    {
        return v switch
        {
            VerticalAlignment.Top => VerticalAlignment.Bottom,
            VerticalAlignment.Center => VerticalAlignment.Center,
            VerticalAlignment.Bottom => VerticalAlignment.Top,
            _ => VerticalAlignment.Top
        };
    }

    public static bool operator ==(Alignment a, Alignment b)
    {
        return a.Horizontal == b.Horizontal && a.Vertical == b.Vertical;
    }

    public static bool operator !=(Alignment a, Alignment b)
    {
        return !(a == b);
    }

    public static Alignment operator *(Alignment a, SpriteEffects e)
    {
        return new(
            e.HasFlag(SpriteEffects.FlipHorizontally) ? Flipped(a.Horizontal) : a.Horizontal,
            e.HasFlag(SpriteEffects.FlipVertically) ? Flipped(a.Vertical) : a.Vertical
        );
    }

    public static Alignment operator *(SpriteEffects e, Alignment a)
    {
        return a * e;
    }

    public override bool Equals(object obj)
    {
        if (obj is Alignment)
        {
            return this == (Alignment)obj;
        }

        return false;
    }

    public bool Equals(Alignment other)
    {
        return this == other;
    }

    public override int GetHashCode()
    {
        return Horizontal.GetHashCode() + Vertical.GetHashCode();
    }

    public static implicit operator Alignment(HorizontalAlignment h) => new(h);
    public static implicit operator Alignment(VerticalAlignment v) => new(v);
}
