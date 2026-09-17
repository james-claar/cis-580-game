using Microsoft.Xna.Framework;
using System.Drawing;

namespace cis_580_game;

/// <summary>
/// Represents a horizontal alignment setting
/// - Left alignment places the left edge of the bounding box on the boundary
/// - Center alignment places the center of the bounding box on the boundary
/// - Right alignment places the right edge of the bounding box on the boundary
/// </summary>
public enum HorizontalAlignment
{
    Left = 0,
    Centered = 1,
    Right = 2
}

/// <summary>
/// Represents a horizontal alignment setting
/// - Top alignment places the top edge of the bounding box on the boundary
/// - Center alignment places the center of the bounding box on the boundary
/// - Bottom alignment places the bottom edge of the bounding box on the boundary
/// </summary>
public enum VerticalAlignment
{
    Top = 0,
    Centered = 1,
    Bottom = 2
}


/// <summary>
/// A parent class for all renderable GUI elements
/// </summary>
public class GuiElement
{
    protected Resources _resources;
    protected RectangleF _boundingBoxCache = new();
    protected bool _boundingBoxDirtyFlag = true;

    protected float _height = 0;

    /// <summary>
    /// The element's height
    /// </summary>
    public float Height
    {
        get => _height;
        set
        {
            if (value != _height)
            {
                _height = value;
                _boundingBoxDirtyFlag = true;
            }
        }
    }

    protected float _width = 0;

    /// <summary>
    /// The element's width
    /// </summary>
    public float Width
    {
        get => _width;
        set
        {
            if (value != _width)
            {
                _width = value;
                _boundingBoxDirtyFlag = true;
            }
        }
    }

    protected Vector2 _position = Vector2.Zero;

    /// <summary>
    /// The element's position to align to
    /// </summary>
    public Vector2 Position
    {
        get => _position;
        set
        {
            if (value != _position)
            {
                _position = value;
                _boundingBoxDirtyFlag = true;
            }
        }
    }

    protected HorizontalAlignment _horizontalAlignment = HorizontalAlignment.Left;

    /// <summary>
    /// The horizontal alignment of the element
    /// </summary>
    public HorizontalAlignment HorizontalAlignment
    {
        get => _horizontalAlignment;
        set
        {
            if (value != _horizontalAlignment)
            {
                _horizontalAlignment = value;
                _boundingBoxDirtyFlag = true;
            }
        }
    }

    protected VerticalAlignment _verticalAlignment = VerticalAlignment.Top;

    /// <summary>
    /// The vertical alignment of the element
    /// </summary>
    public VerticalAlignment VerticalAlignment
    {
        get => _verticalAlignment;
        set
        {
            if (value != _verticalAlignment)
            {
                _verticalAlignment = value;
                _boundingBoxDirtyFlag = true;
            }
        }
    }

    /// <summary>
    /// Whether the element should be selectable by mouse/controller input
    /// </summary>
    public bool IsSelectable = false;

    /// <summary>
    /// Whether this element is selected. This should be managed by the responsible class.
    /// </summary>
    public bool IsSelected = false;

    /// <summary>
    /// A get-only representation of the bounding box around this element
    /// </summary>
    public RectangleF BoundingBox
    {
        get
        {
            if (_boundingBoxDirtyFlag)
            {
                // Calculate bounding box position
                float top_left_x = 0;
                float top_left_y = 0;

                switch (HorizontalAlignment)
                {
                    case HorizontalAlignment.Left:
                        top_left_x = Position.X;
                        break;
                    case HorizontalAlignment.Centered:
                        top_left_x = Position.X - (Width / 2f);
                        break;
                    case HorizontalAlignment.Right:
                        top_left_x = Position.X - Width;
                        break;
                }

                switch (VerticalAlignment)
                {
                    case VerticalAlignment.Top:
                        top_left_y = Position.Y;
                        break;
                    case VerticalAlignment.Centered:
                        top_left_y = Position.Y - (Height / 2f);
                        break;
                    case VerticalAlignment.Bottom:
                        top_left_y = Position.Y - Height;
                        break;
                }

                // Cache calculation result
                _boundingBoxCache.X = top_left_x;
                _boundingBoxCache.Y = top_left_y;
                _boundingBoxCache.Width = Width;
                _boundingBoxCache.Height = Height;

                // Clear dirty flag
                _boundingBoxDirtyFlag = false;
            }
            return _boundingBoxCache;
        }
    }
}
