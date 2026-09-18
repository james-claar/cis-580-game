
using Microsoft.Xna.Framework;

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
    Centered = 1,
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
    Centered = 1,
    Bottom = 2
}


public struct Alignment
{
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
            HorizontalAlignment.Centered => alignmentX - (width / 2),
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
            HorizontalAlignment.Centered => alignmentX - (width / 2),
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
            VerticalAlignment.Centered => alignmentY - (height / 2),
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
            VerticalAlignment.Centered => alignmentY - (height / 2),
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
    /// The horizontal alignment of an element
    /// </summary>
    HorizontalAlignment Horizontal = HorizontalAlignment.Left;

    /// <summary>
    /// The vertical alignment of an element
    /// </summary>
    VerticalAlignment Vertical = VerticalAlignment.Top;

    public static bool operator ==(Alignment a, Alignment b)
    {
        return a.Horizontal == b.Horizontal && a.Vertical == b.Vertical;
    }

    public static bool operator !=(Alignment a, Alignment b)
    {
        return !(a == b);
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
